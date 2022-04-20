using FluentValidation;
using FluentValidation.TestHelper;
using Gateway.API.Infrastructure.Validation;
using Gateway.API.Models;
using Gateway.API.Services;
using Moq;
using System;
using Xunit;

namespace Gateway.API.Tests
{
    public class CardDetailsValidatorTests
    {
        private readonly CardDetailsValidator _validator;

        public CardDetailsValidatorTests()
        {
            _validator = new CardDetailsValidator(Mock.Of<IValidator<ExpiryDate>>());
        }

        [Fact]
        public void ChildValidatorsAreSet()
        {
            _validator.ShouldHaveChildValidator(x => x.ExpiryDate, typeof(IValidator<ExpiryDate>));
        }

        [Theory]
        [InlineData("")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("    ")]
        [InlineData(null)]
        public void EmptyValuesNotAllowed(string value)
        {
            var result = _validator.TestValidate(new CardDetails
            {
                CardNumber = value,
                ExpiryDate = null,
                CVV = value
            });

            result.ShouldHaveValidationErrorFor(x => x.CardNumber);
            result.ShouldHaveValidationErrorFor(x => x.ExpiryDate);
            result.ShouldHaveValidationErrorFor(x => x.CVV);
        }

        [Theory]
        [InlineData("dsfdsfetjb")]
        public void InvalidCardNumberNotAllowed(string value)
        {
            var result = _validator.TestValidate(new CardDetails
            {
                CardNumber = value
            });

            result.ShouldHaveValidationErrorFor(x => x.CardNumber);
        }

        [Theory]
        [InlineData("5555555555554444")]
        [InlineData("5105105105105100")]
        [InlineData("4111111111111111")]
        [InlineData("4012888888881881")]
        public void ValidCardNumberAllowed(string value)
        {
            var result = _validator.TestValidate(new CardDetails
            {
                CardNumber = value
            });

            result.ShouldNotHaveValidationErrorFor(x => x.CardNumber);
        }

        [Theory]
        [InlineData("dsfdsfetjb")]
        [InlineData("1111222233334444")]
        [InlineData("asbc")]
        [InlineData("abc")]
        [InlineData("12345")]
        public void InvalidCVVNotAllowed(string value)
        {
            var result = _validator.TestValidate(new CardDetails
            {
                CVV = value
            });

            result.ShouldHaveValidationErrorFor(x => x.CVV);
        }


        [Theory]
        [InlineData("123")]
        [InlineData("1234")]
        public void ValidCVVAllowed(string value)
        {
            var result = _validator.TestValidate(new CardDetails
            {
                CVV = value
            });

            result.ShouldNotHaveValidationErrorFor(x => x.CVV);
        }
    }

    public class ExpiryDateValidatorTests
    {
        private readonly Mock<IDateTimeOracle> _dateTime;
        private readonly DateTime _today;
        private readonly ExpiryDateValidator _validator;

        public ExpiryDateValidatorTests()
        {
            _dateTime = new Mock<IDateTimeOracle>();
            _today = new DateTime(2022, 4, 20);
            _dateTime.SetupGet(x => x.Now).Returns(_today);
            _validator = new ExpiryDateValidator(_dateTime.Object);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(13)]
        [InlineData(100)]
        public void InvalidMonthNotAllowed(int value)
        {
            var result = _validator.TestValidate(new ExpiryDate
            {
                Month = value,
                Year = _today.Year
            });

            result.ShouldHaveValidationErrorFor(x => x.Month);
        }

        [Fact]
        public void MonthMustBeThisMonthOrInFutureWhenExpiryYearIsThisYear()
        {
            var result = _validator.TestValidate(new ExpiryDate
            {
                Month = _today.Month - 1,
                Year = _today.Year
            });

            result.ShouldHaveValidationErrorFor(x => x.Month);
        }

        [Fact]
        public void MonthDoesNotNeedToBeInFutureWhenExpiryYearIsAfterThisYear()
        {
            var result = _validator.TestValidate(new ExpiryDate
            {
                Month = _today.Month - 1,
                Year = _today.AddYears(1).Year
            });

            result.ShouldNotHaveValidationErrorFor(x => x.Month);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(13)]
        [InlineData(100)]
        [InlineData(10000)]
        public void InvalidYearNotAllowed(int value)
        {
            var result = _validator.TestValidate(new ExpiryDate
            {
                Year = value
            });

            result.ShouldHaveValidationErrorFor(x => x.Year);
        }

        [Theory]
        [InlineData(2022)]
        [InlineData(3000)]
        public void ValidYearIsAllowed(int value)
        {
            var result = _validator.TestValidate(new ExpiryDate
            {
                Month = value
            });

            result.ShouldNotHaveValidationErrorFor(x => x.Month);
        }

        [Fact]
        public void YearMustBeThisYearOrInFuture()
        {
            var result = _validator.TestValidate(new ExpiryDate
            {
                Year = _today.AddYears(-1).Year
            });

            result.ShouldHaveValidationErrorFor(x => x.Year);
        }
    }

    public class AmountValidatorTests
    {
        private readonly AmountValidator _validator;

        public AmountValidatorTests()
        {
            _validator = new AmountValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("    ")]
        [InlineData(null)]
        public void EmptyCurrencyNotAllowed(string value)
        {
            var result = _validator.TestValidate(new Amount
            {
                Currency = value
            });

            result.ShouldHaveValidationErrorFor(x => x.Currency);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void InvalidAmountNotAllowed(int value)
        {
            var result = _validator.TestValidate(new Amount
            {
                Value = value
            });

            result.ShouldHaveValidationErrorFor(x => x.Value);
        }
    }
}