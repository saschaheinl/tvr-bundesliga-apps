using MediatR;
using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Core.Context;
using TVR.Bundesliga.API.Core.Queries;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.UseCases;

public class GetTicketByIdUseCase (TicketDb context) : IRequestHandler<GetTicketByIdQuery, Ticket?>
{
    public Task<Ticket?> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
    {
        return context.Tickets.FirstOrDefaultAsync(t => t.Id == request.TicketId, cancellationToken);
    }
}
