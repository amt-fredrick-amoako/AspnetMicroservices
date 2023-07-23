using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutCommandValidator()
        {
            RuleFor(user => user.UserName)
                .NotEmpty().WithMessage("{userName} is required")
                .NotNull()
                .MaximumLength(50).WithMessage("{UserName} shouldn't exceed the maximum length");

            RuleFor(user => user.EmailAddress)
                .NotEmpty().WithMessage("{EmailAddress} is required.");

            RuleFor(user => user.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} is required.")
                .GreaterThan(0).WithMessage("{TotalPrice} must be more than 0.");
        }
    }
}
