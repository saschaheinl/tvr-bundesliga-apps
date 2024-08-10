using MediatR;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.Commands;

public record AddNewEventCommand(string Name, string? League, DateTimeOffset? Timepoint) : IRequest<Event>;
