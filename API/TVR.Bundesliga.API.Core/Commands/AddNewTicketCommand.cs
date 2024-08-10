using MediatR;
using TVR.Bundesliga.API.Contracts.Requests;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.Commands;

public record AddNewTicketCommand(
    int? EventId,
    CreateTicketRequest.TicketType Type,
    int GuestId,
    int IncludedVisits,
    decimal? Price) : IRequest<Ticket>;
