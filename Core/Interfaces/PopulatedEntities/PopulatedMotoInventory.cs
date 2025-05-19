using Core.Entities;

namespace Core.Interfaces.PopulatedEntities;

public class PopulatedMotoInventory: MotoInventory
{
    public required Motorcycle _moto { get; set; }
    public required Branch _branch { get; set; }

    public PopulatedMotoInventory(MotoInventory motoInventory, Motorcycle moto, Branch branch): base()
    {
        _moto = moto;
        _branch = branch;
        
        this.id = motoInventory.id;
        this.moto_id = moto.id;
        this.branch_id = branch.id;
        this.license_plate = motoInventory.license_plate;
        this.km = motoInventory.km;
        this.customizations = motoInventory.customizations;
        this.status = motoInventory.status;
        this.created_at = motoInventory.created_at;
        this.updated_at = motoInventory.updated_at;
    }
}