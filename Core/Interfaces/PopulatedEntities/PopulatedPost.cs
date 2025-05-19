using System.Collections;
using Core.Entities;

namespace Core.Interfaces.PopulatedEntities;

public class PopulatedPost: Post
{
    public Branch? branch { get; set; }
    public IEnumerable<MotoInventory> MotoInventories { get; set; }
    public PopulatedPost(Post p, Branch? b, IEnumerable<MotoInventory> m) : base()
    {
        this.branch = b;
        this.MotoInventories = m;
        this.id = p.id;
        this.branch_id = b.id;
        this.price = p.price;
        this.status = p.status;
        this.created_at = p.created_at;
        this.updated_at = p.updated_at;
    }
}