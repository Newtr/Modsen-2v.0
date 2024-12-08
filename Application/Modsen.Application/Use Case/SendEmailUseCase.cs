using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace Modsen.Application
{
public class SendEmailUseCase
{
    private readonly EmailSettings _emailSettings;

    public SendEmailUseCase(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public void Execute(string toEmail, string subject, string body)
    {
        using (var mail = new MailMessage())
        {
            mail.From = new MailAddress(_emailSettings.FromEmail);
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = false;

            using (var smtp = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
            {
                smtp.Credentials = new NetworkCredential(_emailSettings.FromEmail, _emailSettings.Password);
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
        }
    }
}

}