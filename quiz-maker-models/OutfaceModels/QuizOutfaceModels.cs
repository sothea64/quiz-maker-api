using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quiz_maker_models.OutfaceModels
{
    public class SubmitQuizOutfaceModel
    {
        public int QuizId { get; set; }
        public int ExamneeId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime FinishDateTime { get; set; }
        //public decimal TotalScore { get; set; }
        public List<SubmitQuestionAnswerModel> QuestionAnswerModels { get; set; } = new List<SubmitQuestionAnswerModel>();
    }

    public class SubmitQuestionAnswerModel
    {
        public int QuestionId { get; set; }
        //public decimal Score { get; set; }
        /// <summary>
        /// For question that is Text type, fill the answer into this property
        /// </summary>
        public string AnswerText { get; set; }
        /// <summary>
        /// For question that is multiple choices or checked box, all answer will 
        /// </summary>
        public List<int> SelectedAnswer { get; set; }
    }
}
