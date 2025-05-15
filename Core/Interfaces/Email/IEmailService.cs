using Core.Entities;

namespace Core.Interfaces.Email;

public interface IEmailService
{
    Task SendPurchaseNotification(User user, string motoDetails);
    Task SendConfirmationEmail(User user, string motoDetails, bool isConfirmed);
    Task SendResetPasswordEmail(User user, string code);
    Task SendVerificationEmail(User user, string code);
}