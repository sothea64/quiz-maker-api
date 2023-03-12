using FluentValidation;
using quiz_maker_models.OutfaceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quiz_maker_api.Validations
{
    public class Authentication : AbstractValidator<LoginOutfaceModel>
    {
        public Authentication()
        {
            RuleFor(x => x.Username).MaximumLength(50).NotNull().NotEmpty();
            RuleFor(x => x.Password).MaximumLength(200).NotNull().NotEmpty();
        }
    }
}
