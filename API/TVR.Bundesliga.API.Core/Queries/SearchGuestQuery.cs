using MediatR;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.Queries;

public record SearchGuestQuery(int? GuestId, string? Name, string? MailAddress) : IRequest<List<Guest>>;
