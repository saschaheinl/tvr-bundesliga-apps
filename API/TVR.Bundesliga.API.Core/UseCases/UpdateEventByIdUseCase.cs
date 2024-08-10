using MediatR;
using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Core.Commands;
using TVR.Bundesliga.API.Core.Context;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.UseCases;

public class UpdateEventByIdUseCase (EventDb context) : IRequestHandler<UpdateEventByIdCommand>
{
    public async Task Handle(UpdateEventByIdCommand request, CancellationToken cancellationToken)
    {
        var eventToChange = await context.Events.FirstOrDefaultAsync(e => e.Id == request.EventId, cancellationToken);
        if (eventToChange is null)
        {
            throw new ArgumentException("Nothing found to change.");
        }

        eventToChange.Date = request.Timepoint;
        eventToChange.Name = request.Name;
        eventToChange.League = request.League;

        await context.SaveChangesAsync(cancellationToken);
    }
}
