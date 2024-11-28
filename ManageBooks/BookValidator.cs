using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageBooks
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(b => b.Title)
                .NotNull().WithMessage("Title cannot be null.") 
                .NotEmpty().WithMessage("Title cannot be empty.")
                .Must(t => !string.IsNullOrWhiteSpace(t)).WithMessage("Title cannot contain only whitespace.");

            RuleFor(b => b.Author)
                .NotNull().WithMessage("Author cannot be null.") 
                .NotEmpty().WithMessage("Author cannot be empty.")
                .Must(a => !string.IsNullOrWhiteSpace(a)).WithMessage("Author cannot contain only whitespace.");

            RuleFor(b => b.PublicationYear)
                .GreaterThan(0).WithMessage("Publication year must be a positive number.");
        }
    }
}
