using Microsoft.EntityFrameworkCore;
using StrategyBuilder.Repository.Entities;
using System;

namespace StrategyBuilder.Repository
{
    public class StrategyBuilderContext : DbContext
    {
        // public StrategyBuilderContext() { }
        public StrategyBuilderContext(DbContextOptions<StrategyBuilderContext> options) : base(options)
        {
        }
        public DbSet<Indicator> Indicators { get; set; }
        public DbSet<Expression> Expressions { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventGroup> EventGroups { get; set; }
        public DbSet<Strategy> Strategies { get; set; }
        public DbSet<BackTestingResult> BackTestingResults { get; set; }
        public DbSet<User> Users { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=localhost\;Database=StrategyBuilderTest7;Trusted_Connection=True;MultipleActiveResultSets=true");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JoinStrategyEventGroup>()
                .HasKey(j => new { j.StrategyId, j.EventGroupId });
            modelBuilder.Entity<JoinStrategyEventGroup>()
                .HasOne(j => j.Strategy)
                .WithMany(s => s.JoinStrategyEventGroups)
                .HasForeignKey(j => j.StrategyId);
            modelBuilder.Entity<JoinStrategyEventGroup>()
                .HasOne(j => j.EventGroup)
                .WithMany(e => e.JoinStrategyEventGroups)
                .HasForeignKey(j => j.EventGroupId);


        }
    }
}
