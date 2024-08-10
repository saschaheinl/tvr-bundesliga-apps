using MediatR;
using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Core.Context;
using TVR.Bundesliga.API.Core.Queries;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.UseCases;

public class GetGuestByIdUseCase (GuestDb context) : IRequestHandler<GetGuestByIdQuery, Guest?>
{
    public Task<Guest?> Handle(GetGuestByIdQuery request, CancellationToken cancellationToken)
    {
        return context.Guests.FirstOrDefaultAsync(g => g.Id == request.GuestId, cancellationToken);
    }
}
