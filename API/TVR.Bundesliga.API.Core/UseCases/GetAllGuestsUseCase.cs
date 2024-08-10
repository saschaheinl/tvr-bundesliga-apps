using MediatR;
using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Core.Context;
using TVR.Bundesliga.API.Core.Queries;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.UseCases;

public class GetAllGuestsUseCase (GuestDb context) : IRequestHandler<GetAllGuestsQuery, List<Guest>>
{
    public Task<List<Guest>> Handle(GetAllGuestsQuery request, CancellationToken cancellationToken)
    {
        return context.Guests.ToListAsync(cancellationToken);
    }
}
