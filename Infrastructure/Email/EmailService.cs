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

    public async Task SendPurchaseNotification(User user, MotoInventory moto, Branch branch, Bill bill)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.Username));
        message.To.Add(new MailboxAddress(user.name, user.email));
        message.Subject = "Confirmación de Compra – Caballos de Acero";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = HtmlMessage.GetPurchaseNotificationTemplate(user, moto, branch, bill)
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
            Console.WriteLine($"Error al enviar notificación de compra: {ex.Message}");
        }
    }
    
    public async Task SendStatusUpdateEmail(User user, MotoInventory moto, Branch branch)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.Username));
        message.To.Add(new MailboxAddress(user.name, user.email));
        message.Subject = "Actualización de Estado – Caballos de Acero";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = HtmlMessage.GetStatusUpdateTemplate(user, moto, branch)
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
            Console.WriteLine($"Error al enviar actualización de estado: {ex.Message}");
        }
    }

    public async Task SendCancellationNotification(User user, MotoInventory moto)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.Username));
        message.To.Add(new MailboxAddress(user.name, user.email));
        message.Subject = "Cancelación de Compra – Caballos de Acero";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = HtmlMessage.GetCancellationTemplate(user, moto)
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
            Console.WriteLine($"Error al enviar notificación de cancelación: {ex.Message}");
        }
    }

    public async Task SendReadyToPickupEmail(User user, MotoInventory moto, Branch branch)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.Username));
        message.To.Add(new MailboxAddress(user.name, user.email));
        message.Subject = "Tu motocicleta está lista – Caballos de Acero";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = HtmlMessage.GetReadyToPickupTemplate(user, moto, branch)
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
            Console.WriteLine($"Error al enviar correo de recogida: {ex.Message}");
        }
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