using BookShop.Application.Genre.Contracts.Request;
using FluentValidation;

namespace BookShop.Application.Genre.Validation;

public sealed class CreateGenreRequestValidator : AbstractValidator<CreateGenreRequest>
{
    public CreateGenreRequestValidator() => RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
}

public sealed class UpdateGenreRequestValidator : AbstractValidator<UpdateGenreRequest>
{
    public UpdateGenreRequestValidator() => RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
}
