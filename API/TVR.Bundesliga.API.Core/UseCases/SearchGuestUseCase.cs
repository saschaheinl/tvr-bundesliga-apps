using MediatR;
using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Core.Context;
using TVR.Bundesliga.API.Core.Queries;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.UseCases;

public class SearchGuestUseCase (GuestDb context) : IRequestHandler<SearchGuestQuery, List<Guest>>
{
    public async Task<List<Guest>> Handle(SearchGuestQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Guest> query = context.Guests;
        
        if (request.GuestId is null && request.Name is null && request.MailAddress is null)
        {
            return [];
        }

        if (request.GuestId is not null)
        {
            query = query.Where(g => g.Id == request.GuestId);
        }
        else
        {
            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(g => g.Name.Contains(request.Name));
            }

            if (!string.IsNullOrEmpty(request.MailAddress))
            {
                query = query.Where(g => g.EmailAddress.Contains(request.MailAddress));
            }
        }

        return await query.ToListAsync(cancellationToken);
    }
}
