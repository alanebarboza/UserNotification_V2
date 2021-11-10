using System.Threading.Tasks;
using UserNotification.Shared.Entities;

namespace UserNotification.Domain.Interfaces.Services
{
    public interface IEmailServices
    {
        Task SendEmail(Email email);
    }
}
