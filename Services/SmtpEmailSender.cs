using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Academico.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string to, string subject, string htmlMessage)
        {
            var smtpHost = "smtp.gmail.com";
            var smtpPort = 587;
            var smtpUser = "natanaelcrim@gmail.com";
            var smtpPass = "lmdq fnvo pdwt ljgs";

            var mail = new MailMessage
            {
                From = new MailAddress(smtpUser, "CentralAcademico"),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            mail.To.Add(to);

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            return client.SendMailAsync(mail);
        }
    }
}