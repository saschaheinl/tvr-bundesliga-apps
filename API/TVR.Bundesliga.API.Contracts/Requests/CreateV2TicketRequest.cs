namespace TVR.Bundesliga.API.Contracts.Requests;

public class CreateV2TicketRequest(string guestName, string? guestEmail, int includedVisits, string createdBy)
{
    public string GuestName { get; set; } = guestName;

    public bool ShouldSendEmail { get; set; } = false;

    public string? GuestEmail { get; set; } = guestEmail;

    public int IncludedVisits { get; set; } = includedVisits;

    public string CreatedBy { get; set; } = createdBy;
}
