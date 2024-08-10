using MediatR;
using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Core.Context;
using TVR.Bundesliga.API.Core.Queries;
using TVR.Bundesliga.API.Core.Requests;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.UseCases;

public class GetEventsByTimeframeUseCase(EventDb context) : IRequestHandler<GetEventsByTimeframeQuery, List<Event>>
{
    public Task<List<Event>> Handle(GetEventsByTimeframeQuery query, CancellationToken cancellationToken)
    {
        return query.Timeframe switch
        {
            Timeframe.Previous => context.Events.Where(e => e.Date < DateTimeOffset.UtcNow)
                .ToListAsync(cancellationToken),
            
            _ => context.Events.Where(e => e.Date > DateTimeOffset.UtcNow)
                .ToListAsync(cancellationToken),
        };
    }
}
