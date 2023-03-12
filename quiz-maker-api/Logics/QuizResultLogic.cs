using quiz_maker_api.Helpers;
using quiz_maker_models;
using quiz_maker_models.Models;
using quiz_maker_models.OutfaceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_maker_api.Logics
{
    public class QuizResultLogic : BaseLogic<QuizResult>
    {
        public QuizResultLogic(QuizMakerDbContext context, ICurrentScope current)
            : base(context, current)
        {

        }

        public async Task<QuizResult> SubmitQuizTestAsync(SubmitQuizOutfaceModel model)
        {
            var quiz = await new QuizLogic(_db, _current).FindAsync(model.QuizId, true);
            if (quiz == null)
            {
                throw new LogicException(ErrorRexs.MsgCannotFindQuiz);
            }
            return new QuizResult();
        }
    }
}
