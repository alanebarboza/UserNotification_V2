using FluentValidation;
using UserNotification.Domain.Commands;

namespace UserNotification.Domain.Validators
{
    public sealed class EmailUsersCommandValidator : AbstractValidator<EmailUsersCommand>
    {
        public EmailUsersCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Obrigatório informar um endereço de Email").EmailAddress().WithMessage("Endereço de Email Inválido.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Obrigatório informar uma Descrição");
            RuleFor(x => x.Type).NotEmpty().WithMessage("Obrigatório informar o Tipo").IsInEnum().WithMessage("O Tipo informado é Inválido.");
            RuleFor(x => x.Notify).NotEmpty().WithMessage("Obrigatório informar onde será enviada a notificação.").IsInEnum().WithMessage("Local de envio informado inválido."); 
        }
    }
}
