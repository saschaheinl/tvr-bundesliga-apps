using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Models;

namespace TVR.Bundesliga.API.Context;

public class GuestDb : DbContext
{
    public GuestDb(DbContextOptions<GuestDb> options) : base(options)
    {
    }

    public DbSet<Guest> Guests { get; set; }
}
