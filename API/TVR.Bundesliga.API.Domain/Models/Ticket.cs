namespace TVR.Bundesliga.API.Domain.Models;

public class Ticket
{
    public int Id { get; set; }
    
    public TicketType Type { get; set; }
    public int IncludedVisits { get; set; }
    public decimal? Price { get; set; }
    public int RemainingVisits { get; set; }
    
    public Event? Event { get; set; }
    public Guest Guest { get; set; }
    
    public Ticket() { }
    
    public Ticket(Event? @event, TicketType type, int includedVisits, Guest guest, decimal? price, int remainingVisits)
    {
        Event = @event;
        Type = type;
        IncludedVisits = includedVisits;
        Guest = guest;
        Price = price;
        RemainingVisits = remainingVisits;
    }
}

public enum TicketType
{
    Free,
    Season,
    Single
}

