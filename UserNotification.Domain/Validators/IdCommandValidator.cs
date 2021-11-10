using FluentValidation;
using UserNotification.Domain.Commands;

namespace UserNotification.Domain.Validators
{
    public sealed class IdCommandValidator : AbstractValidator<IdCommand>
    {
        public IdCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O Id deve ser informado.")
                .GreaterThanOrEqualTo(0).WithMessage("O Id deve ser maior que zero.");
        }
    }
}
