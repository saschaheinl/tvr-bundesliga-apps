namespace TVR.Bundesliga.API.Domain.Models;

public class Event(string name, string? league, DateTimeOffset? date)
{
    public int Id { get; set; }
    
    public string Name { get; set; } = name;

    public string? League { get; set; } = league;

    public DateTimeOffset? Date { get; set; } = date;
}
