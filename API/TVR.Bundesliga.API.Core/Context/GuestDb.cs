using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.Context;

public class GuestDb : DbContext
{
    public GuestDb(DbContextOptions<GuestDb> options) : base(options)
    {
    }

    public DbSet<Guest> Guests { get; set; }
}
