using MediatR;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.Commands;

public record AddNewGuestCommand(string Name, string EmailAddress) : IRequest<Guest>;
