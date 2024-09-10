using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.Context;

public class TicketDb : DbContext
{
    public DbSet<Ticket> Tickets => Set<Ticket>();
    
    public TicketDb(DbContextOptions<TicketDb> options) : base(options)
    {
    }
}
