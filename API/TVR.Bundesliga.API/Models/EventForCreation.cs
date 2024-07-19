namespace TVR.Bundesliga.API.Models;

public class EventForCreation
{
    public string Name { get; set; }

    public string? League { get; set; }

    public DateTimeOffset? Date { get; set; }
}
