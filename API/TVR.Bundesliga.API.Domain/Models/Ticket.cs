namespace TVR.Bundesliga.API.Domain.Models;

public class Ticket
{
    public int Id { get; set; }
    
    public TicketType Type { get; set; }
    
    public int IncludedVisits { get; set; }
    
    public decimal? Price { get; set; }
    
    public int RemainingVisits { get; set; }
    
    
    public Event? Event { get; set; }
    
    public int? EventId { get; set; }
    
    public int GuestId { get; set; } 

    public Ticket(Event? @event, TicketType type, int includedVisits, int guestId, decimal? price, int remainingVisits)
    {
        Event = @event;
        Type = type;
        IncludedVisits = includedVisits;
        GuestId = guestId;
        Price = price;
        RemainingVisits = remainingVisits;
    }
    
#pragma warning disable CS8618 
    private Ticket() { }
#pragma warning restore CS8618 
}

public enum TicketType
{
    Free,
    Season,
    Single
}

