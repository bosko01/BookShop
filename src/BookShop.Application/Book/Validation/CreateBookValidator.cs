using System;
using System.Collections.Generic;
using System.Text;
using BookShop.Application.Book.Contracts;
using FluentValidation;

namespace BookShop.Application.Book.Validation
{
    public sealed class CreateBookValidator : AbstractValidator<CreateBookRequest>
    {
        public CreateBookValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title is too long.");

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.PublisherId)
                .GreaterThan(0);
        }
    }
}
