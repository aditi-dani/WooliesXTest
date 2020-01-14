using FluentValidation;
using WooliesXTest.Data.Models;

namespace WooliesXTest.Api.Validators
{
    public class TrolleyRequestValidator : AbstractValidator<TrolleyRequest>
    {
        public TrolleyRequestValidator()
        {
            RuleFor(request => request.Products)
                .NotNull().WithMessage("The Products field is required.");

            RuleFor(request => request.Specials)
                .NotNull().WithMessage("The Specials field is required.");

            RuleFor(request => request.Quantities)
                .NotNull().WithMessage("The Quantities field is required.");
        }
    }
}
