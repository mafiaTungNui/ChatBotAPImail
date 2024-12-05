using MailKit.Net.Smtp;
using MimeKit;

public class EmailService
{
    private readonly string _smtpServer = "smtp.gmail.com";
    private readonly int _smtpPort = 587;
    private readonly string _smtpUsername = "itdoesanything@gmail.com";
    private readonly string _smtpPassword = "**********";

    public void SendEmail(string toEmail, string subject, string body)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("ChatBot Server", _smtpUsername));
        emailMessage.To.Add(new MailboxAddress("Admin", toEmail));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("plain") { Text = body };

        using (var client = new SmtpClient())
        {
            client.Connect(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            client.Authenticate(_smtpUsername, _smtpPassword);
            client.Send(emailMessage);
            client.Disconnect(true);
        }
    }
}
