using Microsoft.EntityFrameworkCore;
using TVR.Bundesliga.API.Models;

namespace TVR.Bundesliga.API.Context
{
    public class EventDb : DbContext
    {
        public EventDb(DbContextOptions<EventDb> options) : base(options)
        {
        }

        public DbSet<Event> Events => Set<Event>();
    }
}
