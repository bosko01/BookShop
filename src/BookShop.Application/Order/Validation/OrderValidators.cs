using BookShop.Application.Order.Contracts.Request;
using FluentValidation;

namespace BookShop.Application.Order.Validation;

public sealed class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator() => RuleFor(x => x.UserId).GreaterThan(0);
}

public sealed class ChangeOrderItemQuantityRequestValidator : AbstractValidator<ChangeOrderItemQuantityRequest>
{
    public ChangeOrderItemQuantityRequestValidator()
    {
        RuleFor(x => x.OrderItemId).GreaterThan(0);
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}
