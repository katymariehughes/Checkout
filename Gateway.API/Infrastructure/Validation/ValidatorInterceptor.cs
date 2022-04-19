using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Gateway.API.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.API.Infrastructure.Validation
{
    public class ValidatorInterceptor : IValidatorInterceptor
    {
        public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
        {
            if (!result.IsValid)
            {
                throw new RequestValidationException(result.Errors.Select(e => e.ErrorMessage));
            }

            return result;
        }

        public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
        {
            return commonContext;
        }
    }
}
