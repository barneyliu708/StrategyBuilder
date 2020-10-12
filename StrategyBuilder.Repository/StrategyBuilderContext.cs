using Microsoft.EntityFrameworkCore;
using StrategyBuilder.Repository.Entities;

namespace StrategyBuilder.Repository
{
    public class StrategyBuilderContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<EventGroup> EventGroups { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\;Database=StrategyBuilder;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
