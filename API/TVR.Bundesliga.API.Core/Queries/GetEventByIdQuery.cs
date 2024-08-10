using MediatR;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.Requests;

public record GetEventByIdQuery(int EventId) : IRequest<Event?>;
