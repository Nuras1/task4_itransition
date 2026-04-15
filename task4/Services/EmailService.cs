using System.Net;
using System.Net.Mail;
namespace task4.Services
{
    public class EmailService
    {
        public async Task Send(string to, string subject, string body)
        {
            var smtp = new SmtpClient("smtp.mail.ru", 587)
            {
                Credentials = new NetworkCredential("nurasbekmurad@mail.ru", "6p3J4QMeN7eF7U4bcSAa"),
                EnableSsl = true
            };

            var message = new MailMessage("nurasbekmurad@mail.ru", to, subject, body);

            await smtp.SendMailAsync(message);
        }
    }
}
