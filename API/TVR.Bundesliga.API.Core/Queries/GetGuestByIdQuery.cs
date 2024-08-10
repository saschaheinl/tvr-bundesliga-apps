using MediatR;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.Queries;

public record GetGuestByIdQuery(int GuestId) : IRequest<Guest?>;
