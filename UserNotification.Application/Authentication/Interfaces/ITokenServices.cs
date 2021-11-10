using UserNotification.Domain.Commands;

namespace UserNotification.Application.Authentication.Interfaces
{
    public interface ITokenServices
    {
        string CreateToken(LoginCommand command);
    }
}
