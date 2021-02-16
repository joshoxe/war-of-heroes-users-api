using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace WarOfHeroesAPI.Validation
{
    public interface IValidator<T>
    {
        public ValidationResult Validate(object body);
    }
}
