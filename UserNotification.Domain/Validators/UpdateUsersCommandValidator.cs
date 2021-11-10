using FluentValidation;
using UserNotification.Domain.Commands;

namespace UserNotification.Domain.Validators
{
    public sealed class UpdateUsersCommandValidator : AbstractValidator<UpdateUsersCommand>
    {
        public UpdateUsersCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("O Id deve ser informado.");
            RuleFor(x => x.Nick)
                .NotEmpty().WithMessage("O Nick deve ser informado.")
                .Length(5, 20).WithMessage("O Nick deve conter entre 5 e 20 caracteres.");
            RuleFor(x => x.PassWord)
                .NotEmpty().WithMessage("A Senha deve ser informada.")
                .Length(6, 20).WithMessage("A Senha deve conter entre 5 e 20 caracteres.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O Nome deve ser informado.")
                .MaximumLength(80).WithMessage("O Nome deve conter no máximo 80 caracteres.");
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("O Telefone deve ser informado.")
                .MaximumLength(15).WithMessage("O Nome deve conter no máximo 15 caracteres.");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O Email deve ser informado.")
                .EmailAddress().WithMessage("Endereço de Email inválido.")
                .MaximumLength(50).WithMessage("O Email deve conter no máximo 50 caracteres.");
            RuleFor(x => x.UsersNotifications).ForEach(x => x.SetValidator(new UpdateUsersNotificationCommandValidator()));
        }
    }
}
