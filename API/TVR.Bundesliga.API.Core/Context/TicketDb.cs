using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.Context;

public class TicketDb : DbContext
{
    public DbSet<Ticket> Tickets => Set<Ticket>();
    
    public TicketDb(DbContextOptions<TicketDb> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
  
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Event)
            .WithMany() 
            .HasForeignKey("EventId") 
            .IsRequired(false);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Guest)
            .WithMany()
            .HasForeignKey(t => t.GuestId);

        modelBuilder.Entity<Ticket>()
            .Property(t => t.Id)
            .ValueGeneratedOnAdd();
    }
}
