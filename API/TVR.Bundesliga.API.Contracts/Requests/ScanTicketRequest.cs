namespace TVR.Bundesliga.API.Contracts.Requests;

/// <summary>
/// Request for scanning a ticket.
/// </summary>
/// <param name="guests">Amount of guests who used this ticket.</param>
/// <param name="username">User who is scanning.</param>
public class ScanTicketRequest(int guests, string username)
{
    /// <summary>
    /// Amount of guests who used this ticket.
    /// </summary>
    public int Guests { get; set; } = guests;

    /// <summary>
    /// User who is scanning.
    /// </summary>
    public string Username { get; set; } = username;
}
