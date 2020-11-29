using Microsoft.EntityFrameworkCore;
using StrategyBuilder.Repository.Entities;
using System;

namespace StrategyBuilder.Repository
{
    public class StrategyBuilderContext : DbContext
    {
        //public StrategyBuilderContext() { }
        public StrategyBuilderContext(DbContextOptions<StrategyBuilderContext> options) : base (options)
        {
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventGroup> EventGroups { get; set; }
        public DbSet<Strategy> Strategies { get; set; }
        public DbSet<BackTestingResult> BackTestingResults { get; set; }
        public DbSet<User> Users { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=localhost\;Database=StrategyBuilderTest1;Trusted_Connection=True;MultipleActiveResultSets=true");
        //}
    }
}
