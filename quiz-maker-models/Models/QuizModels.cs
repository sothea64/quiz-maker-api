using quiz_maker_models.Enums;
using System;
using System.Collections.Generic;

namespace quiz_maker_models.Models
{
    public class Quiz : BaseModel
    {
        public string Code { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Duration that allow to do the quiz, in minutes
        /// default 60 minutes
        /// </summary>
        public decimal QuizDuration { get; set; } = 60;

        public int RandomQuestions { get; set; }

        /// <summary>
        /// Determine which department that quiz belong to
        /// </summary>
        public int DepartmentId { get; set; }

        public List<Question> Questions { get; set; } = new List<Question>();

        public override string ToString()
        {
            return Name;
        }

        public DateTime GetFinishTime(DateTime startDateTime)
        {
            return startDateTime.AddMinutes(decimal.ToDouble(QuizDuration));
        }
    }

    public class Question : BaseModel
    {
        public string QuestionTitle { get; set; } = string.Empty;
        public string Explaination { get; set; } = string.Empty;
        public int QuizId { get; set; }
        public decimal Score { get; set; } = 1;
        /// <summary>
        /// Ordering for question
        /// </summary>
        public int Order { get; set; }
        public QuestionTypes QuestionType { get; set; }
        
        public List<AnswerSet> AnswerSets { get; set; }

        public override string ToString()
        {
            return QuestionTitle;
        }

        /// <summary>
        /// Require question that will show in test
        /// mark as false to make it not require, but when get for 
        /// test we will random question that not require for test for 
        /// amount that user set in Quiz.RandomQuestions
        /// </summary>
        public bool IsRequire { get; set; } = true;
    }

    public class AnswerSet : BaseModel
    {
        public string Title { get; set; } = string.Empty;
        public int QuestionId { get; set; }

        /// <summary>
        /// For question type like multiple choice, examnee can choose multiple correct answer
        /// so we need to divide score of that question for each correct answer
        /// </summary>
        public decimal ScoreWeight { get; set; }

        /// <summary>
        /// For question type like multiple choices, checked box 
        /// to check is this will mark as correct answer
        /// </summary>
        public bool IsCorrectAnswer { get; set; }

        /// <summary>
        /// For question type like multiple choices, checked box 
        /// that can have multiple answers, so can re-arrange ordering
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// For question type like text 
        /// use can write answer tip for view later
        /// </summary>
        public string AnswerTip { get; set; } = string.Empty;
    }

    public class QuizResult : BaseModel
    {
        public int QuizId { get; set; }
        /// <summary>
        /// Id of User that complete the quiz
        /// </summary>
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishedTime { get; set; }
        public decimal TotalScore { get; set; }
        /// <summary>
        /// When quiz has question type as text, it will be need to be scored
        /// so mark it to true to make admin know that they need to scored the quiz
        /// </summary>
        public bool IsScored { get; set; } = false;
        public string ScoredBy { get; set; }
        public int ScoredByUserId { get; set; }
        public string Extdata { get; set; }
        public List<QuizResultAnswer> QuizResultAnswers { get; set; } = new List<QuizResultAnswer>();
        public int FinishDuration()
        {
            TimeSpan timeSpan = StartTime - FinishedTime;
            return timeSpan.Minutes;
        }
    }

    public class QuizResultAnswer : BaseModel
    {
        public int QuizResultId { get; set; }
        public int QuestionId { get; set; }
        /// <summary>
        /// Feedback by corrector, after quiz, could be blank
        /// </summary>
        public string Feedback { get; set; } = string.Empty;
        /// <summary>
        /// for question that is type multiple choices, or checked box
        /// we will save result with score weight so if the weight is negative
        /// the total score of the quiz could be negative too 
        /// </summary>
        public decimal ScoreWeight { get; set; }
        /// <summary>
        /// For question that is type text, we will save score
        /// after quiz scored by admin
        /// </summary>
        public decimal Score { get; set; }
        /// <summary>
        /// For Question that is type text
        /// </summary>
        public string AnswerText { get; set; } = string.Empty;
        /// <summary>
        /// For Question that is Multiple Choice or Checkedbox
        /// </summary>
        public int SelectedAnswerId { get; set; }
    }
}
