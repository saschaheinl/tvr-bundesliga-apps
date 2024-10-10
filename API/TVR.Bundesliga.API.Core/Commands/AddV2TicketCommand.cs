using MediatR;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.Commands;

public record AddV2TicketCommand(
    string GuestName,
    bool ShouldSendEmail,
    string? GuestEmail,
    int IncludedVisits,
    string CreatedBy) : IRequest<V2Ticket>;
