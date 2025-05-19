using Core.Entities;

namespace Core.Interfaces.PopulatedEntities;

public class PopulatedBill: Bill
{
    public User? User { get; set; }
    public IEnumerable<PopulatedMotoInventory> MotoInventories { get; set; }

    public PopulatedBill(Bill bill, User? user, IEnumerable<PopulatedMotoInventory> motos) : base()
    {
        this.id = bill.id;
        this.user_id = bill.user_id;
        this.amount = bill.amount;
        this.discount = bill.discount;
        this.payment_method = bill.payment_method;
        this.created_at = bill.created_at;

        this.User = user;
        this.MotoInventories = motos;
    }
}