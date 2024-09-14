namespace TVR.Bundesliga.API.Contracts.Requests;

public class UpdateTicketRequest
{
    /// <summary>
    /// ID of the event the ticket is valid for
    /// </summary>
    public int? EventId { get; set; }

    /// <summary>
    /// ID of the Guest of that ticket
    /// </summary>
    public int GuestId { get; set; }
    
    /// <summary>
    /// Number of visits that remain with that ticket
    /// </summary>
    public int RemainingVisits {get; set;}

    /// <summary>
    /// Price of that ticket
    /// </summary>
    public decimal? Price { get; set; }
}
