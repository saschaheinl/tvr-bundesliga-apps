using MediatR;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.Queries;

public record ScanTicketCommand(
    string TicketId,
    int NumberOfGuests,
    string Username) : IRequest<V2Ticket>;
