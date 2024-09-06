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
        // Configuring Event as a required relationship
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Event)
            .WithMany() // No inverse navigation from Event to Ticket
            .HasForeignKey("EventId") // Assume there's an EventId FK in Ticket
            .IsRequired(false); // Optional relationship

        // Configuring Guest as a required relationship
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Guest)
            .WithMany() // No inverse navigation from Guest to Ticket
            .HasForeignKey("GuestId") // Assume there's a GuestId FK in Ticket
            .IsRequired(true); // This is required in your logic
    }
}
