namespace Core.Entities;

public class Card
{
    public int id { get; set; }
    public int user_id { get; set; }
    public required string owner { get; set; }
    public required string pan { get; set; }
    public required string cvv { get; set; }
    public CardType type { get; set; }
    public required string expiration_date { get; set; }
    public CardStatus status { get; set; }
}

public enum CardStatus
{
    Active,
    Inactive
}

public enum CardType
{
    Credit,
    Debit
}