using Microsoft.EntityFrameworkCore;
using quiz_maker_api.Helpers;
using quiz_maker_models;
using quiz_maker_models.Enums;
using quiz_maker_models.ListModels;
using quiz_maker_models.Models;
using quiz_maker_models.SearchModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_maker_api.Logics
{
    public class QuizLogic : BaseLogic<Quiz>
    {
        public QuizLogic(QuizMakerDbContext context, ICurrentScope current)
            : base(context, current)
        {
            
        }

        public async override Task<Quiz> AddAsync(Quiz entity)
        {
            entity = await base.AddAsync(entity);
            if (entity.Questions?.Any() ?? false)
            {
                var answerSets = new List<AnswerSet>();

                entity.Questions.ForEach(x => x.QuizId = entity.Id);
                await AddRangeAsync(entity.Questions);
                //
                foreach (var question in entity.Questions)
                {
                    question.AnswerSets = question.AnswerSets ?? new List<AnswerSet>();
                    //Validate Question
                    validateQuestion(question);

                    var questionAnswerSets = question.AnswerSets;
                    if (questionAnswerSets.Any())
                    {
                        foreach(var answerset in questionAnswerSets)
                        {
                            answerset.QuestionId = question.Id;
                            answerSets.Add(answerset);
                        }
                    }
                }
                //
                await AddRangeAsync(answerSets);
            }
            return entity;
        }

        public async override Task<Quiz> UpdateAsync(Quiz entity)
        {
            entity = await base.UpdateAsync(entity);
            entity.Questions = entity.Questions ?? new List<Question>();
            //Validate Each Question
            entity.Questions.ForEach(x => validateQuestion(x));
            ////Update old questions
            var oldQuestions = entity.Questions.Where(x => x.Id != 0);
            if (oldQuestions.Any())
            {
                await UpdateRangeAsync(oldQuestions);
            }
            //Add new questions
            var newQuestions = entity.Questions.Where(x => x.Id == 0);
            if (newQuestions.Any())
            {
                foreach(var newQuestion in newQuestions)
                {
                    newQuestion.QuizId = entity.Id;
                }
                await AddRangeAsync(newQuestions);
            }
            //Update quiz id into answer set
            foreach(var question in entity.Questions)
            {
                foreach(var answerSet in question.AnswerSets)
                {
                    answerSet.QuestionId = question.Id;
                }
            }
            //
            var allAnswerSets = entity.Questions.SelectMany(x => x.AnswerSets).ToList();
            //Update old answer sets
            var oldAnswerSets = allAnswerSets.Where(x => x.Id != 0);
            if (oldAnswerSets.Any())
            {
                await UpdateRangeAsync(oldAnswerSets);
            }
            //Add new answer sets
            var newAnswerSets = allAnswerSets.Where(x => x.Id == 0);
            if (newAnswerSets.Any())
            {
                await AddRangeAsync(newAnswerSets);
            }
            return entity;
        }

        public async override Task<Quiz> RemoveAsync(Quiz entity)
        {
            entity = await base.RemoveAsync(entity);
            if (entity.Questions?.Any() ?? false)
            {
                //Update questions
                var questions = await _db.Questions.Where(x => x.Id == entity.Id).ToListAsync();
                questions.ForEach(x =>
                {
                    x.Active = false;
                    x.ModifiedDate = DateTime.Now;
                });
                //Update answer sets
                var questionIds = questions.Select(x => x.Id).Distinct().ToList();
                var allAnswerSets = await _db.AnswerSets.Where(x => x.Active && questionIds.Contains(x.QuestionId)).ToListAsync();
                if (allAnswerSets.Any())
                {
                    allAnswerSets.ForEach(x =>
                    {
                        x.Active = false;
                        x.ModifiedDate = DateTime.Now;
                    });
                }
            }

            await _db.SaveChangesAsync();
            return entity;
        }

        public async override Task<bool?> IsExistsAsync(Quiz entity)
        {
            return await _db.Quizzes.AnyAsync(x => x.Code == entity.Code && x.Id != entity.Id);
        }

        public override IQueryable<Quiz> Search(ISearchModel search)
        {
            var param = search as QuizSearchModel;

            var qQuizes = _db.Quizzes.Where(x => x.Id != 0);

            if (!string.IsNullOrEmpty(param.Search))
            {
                var lowersearch = param.Search.ToLower().Trim();
                qQuizes = qQuizes.Where(x => (x.Code+","+x.Name).ToLower().Trim().Contains(lowersearch));
            }

            if (param.ShowDeactivateQuiz)
            {
                qQuizes = qQuizes.Where(x => x.Active == false);
            }
            else
            {
                qQuizes = qQuizes.Where(x => x.Active);
            }

            return qQuizes;
        }

        public async Task<List<QuizListModel>> ListAsync(QuizSearchModel search)
        {
            var qQuizes = from q in Search(search)
                          select new QuizListModel
                          {
                              Id = q.Id,
                              Code = q.Code,
                              Name = q.Name,
                              Description = q.Description
                          };

            return await qQuizes.ToListAsync();
        }

        public async override Task<Quiz> FindAsync(int id, bool include = false)
        {
            Quiz entity = await base.FindAsync(id);

            //If request to include object
            if (entity != null && include)
            {
                //Select questions
                var questions = await _db.Questions.Where(x => x.Active && x.QuizId == entity.Id).OrderBy(x => x.Order).ToListAsync();
                entity.Questions = questions ?? new List<Question>();
                if (questions.Any())
                {
                    var questionIds = questions.Select(x => x.Id).Distinct().ToList();
                    var answerSets = await _db.AnswerSets.Where(x => x.Active && questionIds.Contains(x.QuestionId)).ToListAsync();
                    //Set answer set into each questions
                    foreach(Question question in entity.Questions)
                    {
                        question.AnswerSets = answerSets.Where(x => x.Active && x.QuestionId == question.Id).OrderBy(x => x.Order).ToList();
                    }
                }
            }

            return entity;
        }

        public async Task<Quiz> GetTestQuizAsync(string quizCode)
        {
            Quiz quiz = await _db.Quizzes.FirstOrDefaultAsync(x => x.Code == quizCode);
            if (quiz == null)
            {
                throw new LogicException(string.Format(ErrorRexs.MsgCannotFindQuizWithCode_, quizCode));
            }
            quiz = await FindAsync(quiz.Id, true);
            //Filter Questions that is required for test
            List<Question> questions = new List<Question>();
            questions.AddRange(quiz.Questions.Where(x => x.IsRequire).ToList());
            //Check if need to random question
            if (quiz.RandomQuestions != 0)
            {
                var notRequireQuestions = quiz.Questions.Where(x => x.IsRequire == false);
                var selectedQuestions = new List<Question>();
                //
                Random rand = new Random();
                while(selectedQuestions.Count() != quiz.RandomQuestions)
                {
                    var index = rand.Next(notRequireQuestions.Count());
                    var question = notRequireQuestions.ElementAt(index);
                    if (!selectedQuestions.Any(x => x.Id == question.Id))
                    {
                        selectedQuestions.Add(question);
                    }
                }
                //
                questions.AddRange(selectedQuestions);
            }

            quiz.Questions = questions;
            return quiz;
        }

        private void validateQuestion(Question question)
        {
            if (question == null)
            {
                return;
            }
            int countCorrectAnswer = 0;
            /*
             * 1.1, if question is CheckBox or Multiple Choices, we require it to have at least one answer set
             * 1.2, check if any correct answer provided
             * 1.3, check if score provided
             */
            if (question.QuestionType == QuestionTypes.CheckedBox || question.QuestionType == QuestionTypes.MultipleChoices)
            {
                if (!(question.AnswerSets?.Any()??false))
                {
                    throw new LogicException(string.Format(ErrorRexs._MsgMultipleChoiceOrCheckBoxAnswerRequireAnswerSet, question.QuestionTitle));
                }
                countCorrectAnswer = question.AnswerSets.Where(x => x.IsCorrectAnswer && x.Active).Count();
                if (countCorrectAnswer == 0)
                {
                    throw new LogicException(string.Format(ErrorRexs._MsgNoCorrrectAnswerFound, question.QuestionTitle));
                }
            }
            if (question.Score <= 0)
            {
                throw new LogicException(string.Format(ErrorRexs._MsgScoreNotProvided, question.QuestionTitle));
            }
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            if (question.QuestionType == QuestionTypes.CheckedBox)
            {
                //2, Validate correct answer should not have more than one
                if (countCorrectAnswer > 1)
                {
                    throw new LogicException(string.Format(ErrorRexs._MsgCorrectAnswerCannotBeMoreThanOne, question.QuestionTitle));
                }
            }
            else if (question.QuestionType == QuestionTypes.MultipleChoices)
            {
                //3, Validate the weight of correct answer, make sure it will add up to full score
                var correctAnswer = question.AnswerSets.Where(x => x.IsCorrectAnswer && x.Active).ToList();
                var totalScoreWeight = correctAnswer.Sum(x => x.ScoreWeight);
                if (totalScoreWeight < question.Score)
                {
                    throw new LogicException(string.Format(ErrorRexs._MsgScoreWeightNotEnough, question.QuestionTitle));
                }
                else if (totalScoreWeight > question.Score)
                {
                    throw new LogicException(string.Format(ErrorRexs._MsgScoreWeightExceedMaximum, question.QuestionTitle));
                }
            }
        }
    }
}
