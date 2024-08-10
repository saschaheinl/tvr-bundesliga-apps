using MediatR;
using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Core.Commands;
using TVR.Bundesliga.API.Core.Context;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.UseCases;

public class AddNewGuestUseCase (GuestDb context) : IRequestHandler<AddNewGuestCommand, Guest>
{
    public async Task<Guest> Handle(AddNewGuestCommand request, CancellationToken cancellationToken)
    {
        var existingGuest =
            await context.Guests.FirstOrDefaultAsync(
                g => g.Name == request.Name && g.EmailAddress == request.EmailAddress, cancellationToken);
        if (existingGuest is not null)
        {
            throw new ArgumentException("Guest to create already exists.");
        }

        var guestToCreate = new Guest(request.Name, request.EmailAddress);
        context.Guests.Add(guestToCreate);
        await context.SaveChangesAsync(cancellationToken);

        return guestToCreate;
    }
}
