using BookShop.Application.Binding.Contracts.Request;
using FluentValidation;

namespace BookShop.Application.Binding.Validation;

public sealed class CreateBindingRequestValidator : AbstractValidator<CreateBindingRequest>
{
    public CreateBindingRequestValidator() => RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
}

public sealed class UpdateBindingRequestValidator : AbstractValidator<UpdateBindingRequest>
{
    public UpdateBindingRequestValidator() => RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
}
