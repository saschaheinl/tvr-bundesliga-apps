namespace TVR.Bundesliga.API.Domain.Models;

public class Scan(string username, DateTimeOffset timestamp, int update)
{
    public string Username { get; set; } = username;
    
    public DateTimeOffset Timestamp { get; set; } = timestamp;

    public int UpdatedGuests { get; set; } = update;
}
