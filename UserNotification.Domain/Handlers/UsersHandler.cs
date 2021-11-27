using System.Threading.Tasks;
using UserNotification.Domain.Commands;
using UserNotification.Shared.Handler;
using UserNotification.Shared.Interfaces;
using UserNotification.Domain.Interfaces.Repositories;
using UserNotification.Domain.Entities;
using System.Collections.Generic;
using UserNotification.Shared.Commands;
using System.Collections.ObjectModel;
using UserNotification.Domain.Interfaces.Services;
using UserNotification.Shared.Entities;
using System.Linq;
using UserNotification.Domain.Validators;

namespace UserNotification.Domain.Handlers
{
    public class UsersHandler : IHandler<LoginCommand>, IHandler<CreateUsersCommand>, IHandler<UpdateUsersCommand>, IHandler<IdCommand>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IEmailServices _emailServices;

        private readonly ICollection<string> childList = new Collection<string>() { "UsersNotifications" };

        public UsersHandler(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        public UsersHandler(IEmailServices emailServices)
        {
            _emailServices = emailServices;
        }

        public async Task<ICommand> Handle(LoginCommand loginCommand)
        {
            Users user = await _usersRepository.DoLogin(loginCommand);
            if (user == null)
                return new CommandResult(400, new List<string>() { "Usuário ou Senha inválidos." });
            else
            {
                user.EncodePassWord();
                return new CommandResultObject<Users>(200, new List<string>() { "Login efetuado com sucesso." }, user);
            }
        }

        public async Task<ICommand> Handle(CreateUsersCommand createUserCommand)
        {

            Users user = new Users(0, createUserCommand.Nick, createUserCommand.PassWord, createUserCommand.Name, createUserCommand.Phone, createUserCommand.Email);
            user.AddNotifications(createUserCommand.UsersNotifications);

            await _usersRepository.Insert(user);

            return new CommandResult(200, new List<string>() { "Usuário cadastrado com sucesso." });
        }

        public async Task<ICommand> Handle(UpdateUsersCommand updateUserCommand)
        {
            if (await _usersRepository.Any(updateUserCommand.Id))
            {
                Users user = await _usersRepository.Select(updateUserCommand.Id, childList);

                user.MergeUpdate(user, updateUserCommand);

                await _usersRepository.Update(user);
                return new CommandResult(200, new List<string>() { "Usuário alterado com sucesso." });
            }
            else
                return new CommandResult(400, new List<string>() { "Usuário não existe." });
        }

        public async Task<ICommand> Handle(IdCommand removeUserCommand)
        {
            if (await _usersRepository.Any(removeUserCommand.Id))
            {
                await _usersRepository.Delete(removeUserCommand.Id);
                return new CommandResult(200, new List<string>() { "Usuário excluído com sucesso." });
            }
            else
                return new CommandResult(400, new List<string>() { "Usuário não existe." });
        }

        public async Task<ICommand> Handle(ICollection<EmailUsersCommand> listEmailUsersCommand)
        {
            string subject = "";
            string body = "";
            var emailsToSend = listEmailUsersCommand.ToList().Where(x => x.Notify.ToString().Contains("Email"));

            if (!emailsToSend.Any())
                return new CommandResult(400, new List<string>() { "Nenhuma notificação configurada para envio de Email." });

            List<string> listErrosValidator = new List<string>();
            var validator = new EmailUsersCommandValidator();
            foreach (var email in emailsToSend)
            {
                var resultValidate = validator.Validate(email);
                resultValidate.Errors.ForEach(x => listErrosValidator.Add(x.ErrorMessage));
            }
            if (listErrosValidator.Any())
                return new CommandResult(400, listErrosValidator);

            foreach (var emailUsersCommand in emailsToSend)
            {
                try
                {
                    subject = $@"Reminder of {emailUsersCommand.Description}";

                    if (emailUsersCommand.Type.ToString() == "Bills")
                        body = EmailTemplates.UsersBills(emailUsersCommand.Description, emailUsersCommand.PaymentDate, emailUsersCommand.BarCode, emailUsersCommand.Value);
                    else
                        body = EmailTemplates.UsersNotificationOnly(emailUsersCommand.Description, emailUsersCommand.PaymentDate);

                    Email email = new Email(emailUsersCommand.Email, subject, body);
                    await _emailServices.SendEmail(email);
                }
                catch (System.Exception)
                {
                    return new CommandResult(400, new List<string>() { "Email não enviado." });
                }
            }
            return new CommandResult(200, new List<string>() { "Email enviado com sucesso." });
        }
    }
}
