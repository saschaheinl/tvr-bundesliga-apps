namespace TVR.Bundesliga.API.Contracts.Requests.Event;

public class UpdateEventRequest(string name, string? league, DateTimeOffset? date)
{
    /// <summary>
    /// Name of the event which is to be created.
    /// </summary>
    public string Name { get; set; } = name;

    /// <summary>
    /// Name of the league the event takes place in.
    /// </summary>
    public string? League { get; set; } = league;

    /// <summary>
    /// Timepoint the event takes place.
    /// </summary>
    public DateTimeOffset? Date { get; set; } = date;
}
