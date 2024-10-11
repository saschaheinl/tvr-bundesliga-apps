namespace TVR.Bundesliga.API.Contracts.Requests;

/// <summary>
/// Request for creating a new ticket.
/// </summary>
/// <param name="guestName">Name of the guest who this ticket is for.</param>
/// <param name="guestEmail">Email address of this guest to send the ticket to (optional).</param>
/// <param name="shouldSendEmail">Defines whether an email is sent.</param>
/// <param name="includedVisits">Amount of included visits</param>
/// <param name="createdBy">User who created the ticket</param>
public class CreateV2TicketRequest(string guestName, string? guestEmail, bool shouldSendEmail, int includedVisits, string createdBy)
{
    /// <summary>
    /// Name of the guest who this ticket is for.
    /// </summary>
    public string GuestName { get; set; } = guestName;
    
    /// <summary>
    /// Email address of this guest to send the ticket to (optional).
    /// </summary>
    public string? GuestEmail { get; set; } = guestEmail;

    /// <summary>
    /// Defines whether an email is sent.
    /// </summary>
    public bool ShouldSendEmail { get; set; } = shouldSendEmail;
    
    /// <summary>
    /// Amount of included visits.
    /// </summary>
    public int IncludedVisits { get; set; } = includedVisits;

    /// <summary>
    /// User who creates this ticket.
    /// </summary>
    public string CreatedBy { get; set; } = createdBy;
}
