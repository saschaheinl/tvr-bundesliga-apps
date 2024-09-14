using MediatR;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.Commands;

public record UpdateTicketByIdCommand(int TicketId, int? EventId, int GuestId, int RemainingVisits, decimal? Price):IRequest<Ticket>;
