using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using WarOfHeroesUsersAPI.Users.Models;

namespace WarOfHeroesUsersAPI.Validation
{
    public interface IUserValidator
    {
        ValidationResult Validate(GoogleUser googleUser);
    }
}
