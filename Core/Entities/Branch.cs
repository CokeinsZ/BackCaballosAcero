namespace Core.Entities;

public class Branch
{
    public int id { get; set; }
    public required string name { get; set; }
    public required string country { get; set; }
    public required string city { get; set; }
    public required string address { get; set; }

}