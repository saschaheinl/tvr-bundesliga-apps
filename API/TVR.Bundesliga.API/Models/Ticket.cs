namespace TVR.Bundesliga.API.Models;

public class Ticket
{
    public int Id { get; set; }
    
    public Event Event { get; set; }
    
    public TicketType Type { get; set; }
    
    public int IncludedVisits { get; set; }
    
    public Guest Guest { get; set; }
    
    public decimal? Price { get; set; }
    
    public int RemainingVisits { get; set; }
}

public enum TicketType
{
    Free,
    Season,
    Single
}
