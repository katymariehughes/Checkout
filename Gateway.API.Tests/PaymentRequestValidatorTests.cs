using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using Gateway.API.Infrastructure.Validation;
using Gateway.API.Models;
using Moq;
using Xunit;

namespace Gateway.API.Tests
{
    public class PaymentRequestValidatorTests
    {
        private readonly PaymentRequestValidator _validator;

        public PaymentRequestValidatorTests()
        {
            _validator = new PaymentRequestValidator(Mock.Of<IValidator<CardDetails>>(), Mock.Of<IValidator<Amount>>());
        }

        [Fact]
        public void ChildValidatorsAreSet()
        {
            _validator.ShouldHaveChildValidator(x => x.CardDetails, typeof(IValidator<CardDetails>));
            _validator.ShouldHaveChildValidator(x => x.Amount, typeof(IValidator<Amount>));
        }

        [Fact]
        public void NullValuesNotAllowed()
        {
            var result = _validator.TestValidate(new PaymentRequest
            {
                CardDetails = null,
                Amount = null
            });

            result.ShouldHaveValidationErrorFor(x => x.CardDetails);
            result.ShouldHaveValidationErrorFor(x => x.Amount);
        }
    }
}