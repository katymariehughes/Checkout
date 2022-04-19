using FluentValidation;
using Gateway.API.Models;
using Gateway.API.Services;

namespace Gateway.API.Infrastructure.Validation
{
    public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
    {
        public PaymentRequestValidator(IValidator<CardDetails> cardDetailsValidator, IValidator<Amount> amountValidator)
        {
            RuleFor(x => x.CardDetails)
                .NotNull()
                .SetValidator(cardDetailsValidator);

            RuleFor(x => x.Amount)
               .NotNull()
               .SetValidator(amountValidator);
        }
    }
    public class CardDetailsValidator : AbstractValidator<CardDetails>
    {
        public CardDetailsValidator(IValidator<ExpiryDate> expiryDateValidator)
        {
            RuleFor(x => x.CardNumber)
                .NotEmpty()
                .CreditCard();

            RuleFor(x => x.ExpiryDate)
                .NotNull()
                .SetValidator(expiryDateValidator);

            RuleFor(x => x.CVV)
                .NotEmpty()
                .Matches("^[0-9]{3,4}$"); // 3 or 4 digits
        }
    }

    public class ExpiryDateValidator : AbstractValidator<ExpiryDate>
    {
        public ExpiryDateValidator(IDateTimeOracle dateTime)
        {
            RuleFor(x => x.Month)
                .NotEmpty()
                .InclusiveBetween(1, 12)
                .GreaterThanOrEqualTo(dateTime.Now.Month)
                .When((expiryDate, _) => expiryDate.Year == dateTime.Now.Year);

            RuleFor(x => x.Year)
                .NotEmpty()
                .Must(x => x.ToString().Length == 4)
                .GreaterThanOrEqualTo(dateTime.Now.Year);
        }
    }

    public class AmountValidator : AbstractValidator<Amount>
    {
        public AmountValidator()
        {
            RuleFor(x => x.Currency)
                .NotEmpty();

            RuleFor(x => x.Value)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
