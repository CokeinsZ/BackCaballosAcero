using Core.Entities;
using Core.Interfaces.Email;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
namespace Infrastructure.Email;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public Task SendPurchaseNotification(User user, MotoInventory moto)
    {
        throw new NotImplementedException();
    }

    public Task SendStatusUpdateEmail(User user, MotoInventory moto)
    {
        throw new NotImplementedException();
    }

    public Task SendResetPasswordEmail(User user, string code)
    {
        throw new NotImplementedException();
    }

    public async Task SendVerificationEmail(User user, string code)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.Username));
        message.To.Add(new MailboxAddress("", user.email));
        message.Subject = "Verificacion de Correo";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = HtmlMessage.GetVerificationEmailTemplate(user.name, code)
        };

        message.Body = bodyBuilder.ToMessageBody();

        try
        {
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.Auto);
            await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine("" + _emailSettings.SmtpServer + _emailSettings.Port + _emailSettings.Username + _emailSettings.Password);
            Console.WriteLine($"Error al enviar correo: {ex.Message}");
        }

    }
}