using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace WarOfHeroesAPI.Validation
{
    public class LoginValidator : AbstractValidator<User>
    {
        public LoginValidator()
        {
            RuleFor(user => user.Username).NotNull().NotEmpty();
        }
    }
}
