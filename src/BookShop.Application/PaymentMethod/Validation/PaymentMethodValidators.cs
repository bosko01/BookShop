using BookShop.Application.PaymentMethod.Contracts.Request;
using FluentValidation;

namespace BookShop.Application.PaymentMethod.Validation;

public sealed class CreatePaymentMethodRequestValidator : AbstractValidator<CreatePaymentMethodRequest>
{
    public CreatePaymentMethodRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(250).When(x => x.Description is not null);
    }
}

public sealed class UpdatePaymentMethodRequestValidator : AbstractValidator<UpdatePaymentMethodRequest>
{
    public UpdatePaymentMethodRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(250).When(x => x.Description is not null);
    }
}
