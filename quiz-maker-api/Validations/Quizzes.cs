using FluentValidation;
using quiz_maker_models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_maker_api.Validations
{
    public class QuizzesValidation : AbstractValidator<Quiz>
    {
        public QuizzesValidation()
        {
            RuleFor(x => x.Code).MaximumLength(50).NotNull().NotEmpty();
            RuleFor(x => x.Name).MaximumLength(200).NotNull().NotEmpty();
            RuleFor(x => x.Description).MaximumLength(1000).NotNull();
        }
    }

    public class QuestionValidation : AbstractValidator<Question>
    {
        public QuestionValidation()
        {
            RuleFor(x => x.QuestionTitle).NotNull().NotEmpty();
            RuleFor(x => x.Explaination).NotNull().NotEmpty();
        }
    }

    public class AnswerSetValidation : AbstractValidator<AnswerSet>
    {
        public AnswerSetValidation()
        {
            RuleFor(x => x.Title).MaximumLength(300).NotNull().NotEmpty();
        }
    }
}
