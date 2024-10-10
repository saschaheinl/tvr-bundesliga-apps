namespace TVR.Bundesliga.API.Domain.Models;

public class V2Ticket(
    string id,
    string guestName,
    string? guestEmail,
    int totalVisits,
    int remainingVisits,
    decimal price,
    bool wasSentByEmail,
    DateTimeOffset created,
    string createdBy,
    QrCodeDetails qrCode,
    List<Scan> scans)
{
    public string Id { get; set; } = id;

    public string GuestName { get; set; } = guestName;

    public string? GuestEmail { get; set; } = guestEmail;

    public QrCodeDetails QrCode { get; set; } = qrCode;

    public int TotalVisits { get; set; } = totalVisits;

    public int RemainingVisits { get; set; } = remainingVisits;

    public List<Scan> Scans { get; set; } = scans;

    public decimal Price { get; set; } = price;

    public bool WasSentByEmail { get; set; } = wasSentByEmail;

    public DateTimeOffset Created { get; set; } = created;

    public DateTimeOffset LastModified { get; set; } = created;

    public string CreatedBy { get; set; } = createdBy;
}
