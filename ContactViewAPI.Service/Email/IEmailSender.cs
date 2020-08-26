namespace ContactViewAPI.Service.Email
{
    using System.Threading.Tasks;

    public interface IEmailSender
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);
    }
}
