using BookShop.Application.Invoice.Contracts.Request;
using FluentValidation;

namespace BookShop.Application.Invoice.Validation;

public sealed class CreateInvoiceRequestValidator : AbstractValidator<CreateInvoiceRequest>
{
    public CreateInvoiceRequestValidator()
    {
        RuleFor(x => x.OrderId).GreaterThan(0);
        RuleFor(x => x.PaymentMethodId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Provider).MaximumLength(100).When(x => x.Provider is not null);
    }
}

public sealed class MarkInvoicePaidRequestValidator : AbstractValidator<MarkInvoicePaidRequest>
{
    public MarkInvoicePaidRequestValidator()
        => RuleFor(x => x.ProviderReference).MaximumLength(200).When(x => x.ProviderReference is not null);
}
