using System.Threading.Tasks;

namespace Infrastructure.EmailService
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message);
    }
}