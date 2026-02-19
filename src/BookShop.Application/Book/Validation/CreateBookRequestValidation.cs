using BookShop.Application.Book.Contracts.Request;
using FluentValidation;

namespace BookShop.Application.Book.Validation;

public sealed class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
{
    public CreateBookRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(250);

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.QuantityInStock)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.PageCount)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.PublisherId)
            .GreaterThan(0);

        RuleFor(x => x.AuthorId)
            .GreaterThan(0);

        RuleFor(x => x.GenreId)
            .GreaterThan(0);

        RuleFor(x => x.BindingId)
            .GreaterThan(0);

        RuleFor(x => x.Description)
            .MaximumLength(4000)
            .When(x => x.Description is not null);

        RuleFor(x => x.ImageUrl)
            .MaximumLength(500)
            .When(x => x.ImageUrl is not null);
    }
}
