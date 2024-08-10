using MediatR;
using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Core.Context;
using TVR.Bundesliga.API.Core.Queries;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.UseCases;

public class GetAllEventsUseCase(EventDb context) : IRequestHandler<GetAllEventsQuery, List<Event>>
{
    public Task<List<Event>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        return context.Events.ToListAsync(cancellationToken);
    }
}
