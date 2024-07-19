namespace TVR.Bundesliga.API.Models;

public class Event
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string? League { get; set; }
    
    public DateTimeOffset? Date { get; set; }
}

