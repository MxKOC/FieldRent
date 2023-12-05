namespace FieldRent.Entity;


public class BasketRent
{
    public int BasketRentId { get; set; }
    public int? FieldId { get; set; }
    public int? Price { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;


    public int MapId { get; set; }
    public Map Map { get; set; } = null!;
}
