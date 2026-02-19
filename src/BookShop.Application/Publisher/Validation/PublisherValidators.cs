using BookShop.Application.Publisher.Contracts.Request;
using FluentValidation;

namespace BookShop.Application.Publisher.Validation;

public sealed class CreatePublisherRequestValidator : AbstractValidator<CreatePublisherRequest>
{
    public CreatePublisherRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Address).MaximumLength(200).When(x => x.Address is not null);
        RuleFor(x => x.City).MaximumLength(100).When(x => x.City is not null);
        RuleFor(x => x.Country).MaximumLength(100).When(x => x.Country is not null);
        RuleFor(x => x.PhoneNumber).MaximumLength(30).When(x => x.PhoneNumber is not null);
    }
}

public sealed class UpdatePublisherRequestValidator : AbstractValidator<UpdatePublisherRequest>
{
    public UpdatePublisherRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Address).MaximumLength(200).When(x => x.Address is not null);
        RuleFor(x => x.City).MaximumLength(100).When(x => x.City is not null);
        RuleFor(x => x.Country).MaximumLength(100).When(x => x.Country is not null);
        RuleFor(x => x.PhoneNumber).MaximumLength(30).When(x => x.PhoneNumber is not null);
    }
}

public sealed class UpdatePublisherContactRequestValidator : AbstractValidator<UpdatePublisherContactRequest>
{
    public UpdatePublisherContactRequestValidator()
    {
        RuleFor(x => x.Address).MaximumLength(200).When(x => x.Address is not null);
        RuleFor(x => x.City).MaximumLength(100).When(x => x.City is not null);
        RuleFor(x => x.Country).MaximumLength(100).When(x => x.Country is not null);
        RuleFor(x => x.PhoneNumber).MaximumLength(30).When(x => x.PhoneNumber is not null);
    }
}
