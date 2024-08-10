using MediatR;
using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Core.Context;
using TVR.Bundesliga.API.Core.Requests;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.UseCases;

public class GetEventByIdUseCase(EventDb context) : IRequestHandler<GetEventByIdQuery, Event?>
{
    public Task<Event?> Handle(GetEventByIdQuery query, CancellationToken cancellationToken)
    {
        return context.Events.FirstOrDefaultAsync(e => e.Id == query.EventId, cancellationToken);
    }
}
