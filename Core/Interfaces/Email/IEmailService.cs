using Core.Entities;

namespace Core.Interfaces.Email;

public interface IEmailService
{
    Task SendPurchaseNotification(User user, MotoInventory moto, Branch branch);
    Task SendStatusUpdateEmail(User user, MotoInventory moto, Branch branch);
    Task SendReadyToPickupEmail(User user, MotoInventory moto, Branch branch);
    Task SendCancellationNotification(User user, MotoInventory moto);
    Task SendResetPasswordEmail(User user, string code);
    Task SendVerificationEmail(User user, string code);
}