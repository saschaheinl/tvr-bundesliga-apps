using MediatR;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.Commands;

public record UpdateEventByIdCommand(int EventId, string Name, string? League, DateTimeOffset? Timepoint): IRequest;
