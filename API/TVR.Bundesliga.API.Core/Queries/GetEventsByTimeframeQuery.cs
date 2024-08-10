using MediatR;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.Queries;

public class GetEventsByTimeframeQuery(DateTimeOffset startpoint, Timeframe timeframe) : IRequest<List<Event>>
{
    public DateTimeOffset Startpoint { get; set; } = startpoint;

    public Timeframe Timeframe { get; set; } = timeframe;
}

public enum Timeframe
{
    Previous,
    Upcoming
} 
