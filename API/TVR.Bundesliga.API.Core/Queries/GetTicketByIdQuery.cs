using MediatR;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.Queries;

public record GetTicketByIdQuery(int TicketId) : IRequest<Ticket?>;
