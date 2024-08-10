using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Domain.Models;

namespace TVR.Bundesliga.API.Core.Context
{
    public class EventDb : DbContext
    {
        public EventDb(DbContextOptions<EventDb> options) : base(options)
        {
        }

        public DbSet<Event> Events => Set<Event>();
    }
}
