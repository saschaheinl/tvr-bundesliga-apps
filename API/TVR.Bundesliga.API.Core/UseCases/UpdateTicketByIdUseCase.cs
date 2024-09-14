using MediatR;
using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Core.Commands;
using TVR.Bundesliga.API.Core.Context;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.UseCases;

public class UpdateTicketByIdUseCase(TicketDb ticketContext, GuestDb guestContext, EventDb eventContext) : IRequestHandler<UpdateTicketByIdCommand,Ticket>
{
    public async Task<Ticket> Handle(UpdateTicketByIdCommand request, CancellationToken cancellationToken)
    {
        var foundTicket = await ticketContext.Tickets.FindAsync(request.TicketId);
        if (foundTicket is null)
        {
            throw new NullReferenceException($"No ticket found with id {request.TicketId}");
        }
        
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
        
        foundTicket.EventId = foundTicket.EventId != request.EventId ? request.EventId : foundTicket.EventId;
        foundTicket.GuestId = foundTicket.GuestId != request.GuestId ? request.GuestId : foundTicket.GuestId;
        foundTicket.RemainingVisits = foundTicket.RemainingVisits != request.RemainingVisits ? request.RemainingVisits : foundTicket.RemainingVisits;
        foundTicket.Price = foundTicket.Price != request.Price ? request.Price : foundTicket.Price;
        
        ticketContext.Tickets.Update(foundTicket);
        await ticketContext.SaveChangesAsync(cancellationToken);

        return foundTicket;
    }
}
