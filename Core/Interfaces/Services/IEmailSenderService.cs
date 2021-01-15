using System.Threading.Tasks;
namespace Core.Interfaces.Services
{
    public interface IEmailSenderService
    {
        Task SendConfirmationEmailAsync(string to, string token, string username);
        Task SendPasswordResetEmailAsync(string to, string token, string username);
    }
}