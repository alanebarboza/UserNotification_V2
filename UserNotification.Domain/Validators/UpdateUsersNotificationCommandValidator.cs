using FluentValidation;
using UserNotification.Domain.Commands;

namespace UserNotification.Domain.Validators
{
    public sealed class UpdateUsersNotificationCommandValidator : AbstractValidator<UpdateUsersNotificationCommand>
    {
        public UpdateUsersNotificationCommandValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("A Descrição deve ser informada.")
                .MaximumLength(100).WithMessage("A Descrição deve conter no máximo 100 caracteres.");
            RuleFor(x => x.Type).IsInEnum().WithMessage("O Tipo informado é Inválido.");
            RuleFor(x => x.BarCode)
                .MaximumLength(60).WithMessage("O Código de Barras deve conter no máximo 60 caracteres.");
            RuleFor(x => x.PaymentDate)
                .NotEmpty().WithMessage("O Data para Pagamento deve ser informada.");
            RuleFor(x => x.PaidBy)
                .MaximumLength(100).WithMessage("O Local de Pagamento deve conter no máximo 50 caracteres.");
            RuleFor(x => x.Notify).IsInEnum().WithMessage("O Valor informado se será enviado Notificação é inválido.");
        }
    }
}
