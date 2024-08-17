using MediatR;
using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Contracts.Requests;
using TVR.Bundesliga.API.Core.Commands;
using TVR.Bundesliga.API.Core.Context;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.UseCases;

public class AddNewTicketUseCase(TicketDb ticketContext, EventDb eventContext, GuestDb guestContext) : IRequestHandler<AddNewTicketCommand, Ticket>
{
    public async Task<Ticket> Handle(AddNewTicketCommand request, CancellationToken cancellationToken)
    {
        Event? @event = null;

        var guest = await guestContext.Guests.FirstOrDefaultAsync(g => g.Id == request.GuestId, cancellationToken);
        if (guest is null)
        {
            throw new ArgumentException("GuestId seems being invalid.");
        }

        if (request.EventId is not null)
        {
            var foundEvent = await eventContext.Events.FirstOrDefaultAsync(e => e.Id == request.EventId, cancellationToken);

            @event = foundEvent ?? throw new Exception("Invalid EventId.");
        }

        var existingTicket = await ticketContext.Tickets.FirstOrDefaultAsync(t =>
            t.Event != null && t.Guest.Id == request.GuestId && t.Event.Id == request.EventId, cancellationToken);
        if (existingTicket is not null)
        {
            throw new ArgumentException("This ticket already exists.");
        }

        var ticketType = request.Type switch
        {
            CreateTicketRequest.CreationTicketType.Free => TicketType.Free,
            CreateTicketRequest.CreationTicketType.Season => TicketType.Season,
            _ => TicketType.Single
        };

        var ticketToCreate = new Ticket(@event, ticketType, request.IncludedVisits, guest, request.Price,
            request.IncludedVisits);

        ticketContext.Tickets.Add(ticketToCreate);
        await ticketContext.SaveChangesAsync(cancellationToken);

        return ticketToCreate;
    }
}
