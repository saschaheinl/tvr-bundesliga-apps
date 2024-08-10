using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.Context;

public class TicketDb(DbContextOptions<TicketDb> options) : DbContext(options)
{
    public DbSet<Ticket> Tickets => Set<Ticket>();
}
