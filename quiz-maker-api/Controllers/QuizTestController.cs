using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using quiz_maker_api.Logics;
using quiz_maker_models.Models;
using quiz_maker_models.OutfaceModels;

namespace quiz_maker_api.Controllers
{
    public class QuizTestController : EmptyBaseController
    {
        QuizResultLogic _logic;
        QuizLogic _quizLogic;

        public QuizTestController(QuizResultLogic logic, QuizLogic quizLogic)
        {
            _logic = logic;
            _quizLogic = quizLogic;
        }

        /// <summary>
        /// Get quiz for test
        /// </summary>
        [HttpGet("GetQuiz/{QuizCode}")]
        public async Task<ActionResult<Quiz>> GetExamQuizAsync(string QuizCode)
        {
            return Ok(await _quizLogic.GetTestQuizAsync(QuizCode));
        }

        [HttpPost("SubmitQuiz")]
        public async Task<IActionResult> SubmitQuiz([FromBody] SubmitQuizOutfaceModel submitModel)
        {
            return Ok(await _logic.ExecuteAsync(() => _logic.SubmitQuizTestAsync(submitModel)));
        }
    }
}