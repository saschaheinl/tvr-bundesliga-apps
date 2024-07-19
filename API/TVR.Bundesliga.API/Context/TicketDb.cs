using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Models;

namespace TVR.Bundesliga.API.Context;

public class TicketDb(DbContextOptions<TicketDb> options) : DbContext(options)
{
    public DbSet<Ticket> Tickets => Set<Ticket>();
}
