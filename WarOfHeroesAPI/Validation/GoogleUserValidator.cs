using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using WarOfHeroesUsersAPI.Users.Models;

namespace WarOfHeroesUsersAPI.Validation
{
    public class GoogleUserValidator : AbstractValidator<GoogleUser>, IUserValidator
    {
        public GoogleUserValidator()
        {
            RuleFor(user => user.Provider).NotEmpty();
            RuleFor(user => user.ID).NotEmpty();
            RuleFor(user => user.Email).NotEmpty();
            RuleFor(user => user.Name).NotEmpty();
            RuleFor(user => user.FirstName).NotEmpty();
            RuleFor(user => user.AuthToken).NotEmpty();
            RuleFor(user => user.IdToken).NotEmpty();
            RuleFor(user => user.Response).NotEmpty();
        }
    }
}
