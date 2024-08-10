using MediatR;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.Queries;

public record GetAllEventsQuery : IRequest<List<Event>>;
