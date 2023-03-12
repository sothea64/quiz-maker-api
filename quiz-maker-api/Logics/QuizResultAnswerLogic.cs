using quiz_maker_api.Helpers;
using quiz_maker_models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_maker_api.Logics
{
    public class QuizResultAnswerLogic : BaseLogic<QuizResultAnswer>
    {
        public QuizResultAnswerLogic(QuizMakerDbContext context, ICurrentScope current)
            : base(context, current)
        {

        }
    }
}
