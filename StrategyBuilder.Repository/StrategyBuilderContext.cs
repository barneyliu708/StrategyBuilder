using Microsoft.EntityFrameworkCore;
using StrategyBuilder.Repository.Entities;
using System;

namespace StrategyBuilder.Repository
{
    public class StrategyBuilderContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<EventGroup> EventGroups { get; set; }
        public DbSet<Strategy> Strategies { get; set; }
        public DbSet<BackTestingResult> BackTestingResults { get; set; }
        public DbSet<User> Users { get; set; }

        public int InsertIntoBackTestingResult(DateTime executeOn, DateTime startFrom, DateTime endTo, string reportUri, int executedBy, int strategyId)
        {
            string query = $"INSERT INTO BackTestingResults (ExecutedOn, StartFrom, EndTo, ReportFileUri, ExecutedById, StrategyId) " +
                           $"VALUES('{executeOn:yyyy-MM-dd HH:mm:ss.fff}', '{startFrom:yyyy-MM-dd HH:mm:ss.fff}', '{endTo:yyyy-MM-dd HH:mm:ss.fff}', '{reportUri}', {executedBy}, {strategyId}); ";
            var result = Database.ExecuteSqlCommand(query);
            return result;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost\;Database=StrategyBuilder;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
