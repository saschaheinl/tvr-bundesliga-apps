using MediatR;
using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Core.Commands;
using TVR.Bundesliga.API.Core.Context;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.UseCases;

public class AddNewEventUseCase(EventDb context) : IRequestHandler<AddNewEventCommand, Event>
{
    private readonly EventDb _context = context;
    
    public async Task<Event> Handle(AddNewEventCommand request, CancellationToken cancellationToken)
    {
        if (request.Timepoint is not null)
        {
            var sameDayEvent = await _context.Events.FirstOrDefaultAsync(e => e.Date == request.Timepoint,
                cancellationToken: cancellationToken);
            if (sameDayEvent is not null && sameDayEvent.Name == request.Name)
            {
                throw new ArgumentException($"Event already exists under id {sameDayEvent.Id}");
            }
        }

        var sameNameEvents = await _context.Events.Where(e => e.Name == request.Name).ToListAsync(cancellationToken);
        if (sameNameEvents.Count > 0 && request.Timepoint is null)
        {
            throw new ArgumentException("Event with that name exists, please Add a date");
        }

        var eventForCreation = new Event(request.Name, request.League, request.Timepoint);
        await _context.Events.AddAsync(eventForCreation, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return eventForCreation;
    }
}
