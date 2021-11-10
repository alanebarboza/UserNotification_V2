using FluentValidation;
using UserNotification.Domain.Commands;

namespace UserNotification.Domain.Validators
{
    public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Nick)
                .NotEmpty().WithMessage("O Nick deve ser informado.")
                .Length(5, 20).WithMessage("O Nick deve conter entre 5 e 20 caracteres.");
            RuleFor(x => x.PassWord)
                .NotEmpty().WithMessage("A Senha deve ser informada.")
                .Length(6, 20).WithMessage("A Senha deve conter entre 5 e 20 caracteres.");
        }
    }
}
