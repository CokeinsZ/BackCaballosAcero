using Core.Entities;

namespace Core.Interfaces.PopulatedEntities;

public class PopulatedMotoInventory: MotoInventory
{
    public required Motorcycle moto { get; set; }
    public required Branch branch { get; set; }
    public required Post post { get; set; }

}