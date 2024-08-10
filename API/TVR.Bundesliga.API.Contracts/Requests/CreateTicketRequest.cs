namespace TVR.Bundesliga.API.Contracts.Requests;

public class CreateTicketRequest(
    int? eventId,
    CreateTicketRequest.TicketType type,
    int guestId,
    int includedVisits,
    decimal? price,
    int remainingVisits)
{
    /// <summary>
    /// ID of the event the ticket is valid for
    /// </summary>
    public int? EventId { get; set; } = eventId;

    /// <summary>
    /// Type of the ticket
    /// </summary>
    public TicketType Type { get; set; } = type;

    /// <summary>
    /// ID of the Guest of that ticket
    /// </summary>
    public int GuestId { get; set; } = guestId;

    /// <summary>
    /// Number of visits included with that ticket
    /// </summary>
    public int IncludedVisits { get; set; } = includedVisits;

    /// <summary>
    /// Price of that ticket
    /// </summary>
    public decimal? Price { get; set; } = price;

    public enum TicketType
    {
        Free,
        Season,
        Single
    }
}
