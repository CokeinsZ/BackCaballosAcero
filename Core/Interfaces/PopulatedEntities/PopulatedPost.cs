using Core.Entities;

namespace Core.Interfaces.PopulatedEntities;

public class PopulatedPost: Post
{
    public required Branch branch { get; set; }
}