using Microsoft.EntityFrameworkCore;
using StrategyBuilder.Repository.Entities;
using StrategyBuilder.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrategyBuilder.Service
{
    public class StrategyService : BaseService, IStrategyService
    {
        public StrategyService(DbContext dbContext)
            : base(dbContext)
        {

        }

        public void AddNewStrategy(Strategy strategy)
        {
            var user = _dbContext.Set<User>().First(u => u.Id == strategy.CreatedBy.Id);
            strategy.CreatedBy = user;
            _dbContext.Set<Strategy>().Add(strategy);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Strategy> GetAllStrategiesByUserId(int userId)
        {
            return _dbContext.Set<Strategy>()
                             .Include(s => s.CreatedBy)
                             .Include(s => s.JoinStrategyEventGroups).ThenInclude(j => j.EventGroup)
                             .Include(s => s.BackTestingResults)
                             .Where(s => s.CreatedBy.Id == userId)
                             .OrderBy(s => s.Id);
        }

        public Strategy GetStrategiesByStrategyId(int strategyId)
        {
            return _dbContext.Set<Strategy>()
                             .Include(s => s.CreatedBy)
                             .Include(s => s.JoinStrategyEventGroups).ThenInclude(j => j.EventGroup).ThenInclude(eg => eg.Events)
                             .Include(s => s.BackTestingResults)
                             .FirstOrDefault(s => s.Id == strategyId);
        }

        public void UpdateEventGroupsInStrategy(int strategyId, IEnumerable<JoinStrategyEventGroup> strategyEventgroups)
        {
            var strategy = _dbContext.Set<Strategy>()
                                     .Include(s => s.JoinStrategyEventGroups)
                                     .Single(s => s.Id == strategyId);

            strategy.JoinStrategyEventGroups.Clear();

            foreach (var join in strategyEventgroups)
            {
                var eventGroup = _dbContext.Set<EventGroup>().Single(e => e.Id == join.EventGroupId);
                strategy.JoinStrategyEventGroups.Add(new JoinStrategyEventGroup 
                { 
                    Action = join.Action,
                    Strategy = strategy, 
                    EventGroup = eventGroup
                });
            }

            _dbContext.SaveChanges();
        }

        public int InsertIntoBackTestingResult(DateTime executeOn, DateTime startFrom, DateTime endTo, string reportUri, int executedBy, int strategyId)
        {
            var strategy = _dbContext.Set<Strategy>().First(s => s.Id == strategyId);
            strategy.BackTestingResults.Add(new BackTestingResult() { ExecutedOn = executeOn, StartFrom = startFrom, EndTo = endTo, ReportFileUri = reportUri});
            return _dbContext.SaveChanges();
            //string query = $"INSERT INTO BackTestingResults (ExecutedOn, StartFrom, EndTo, ReportFileUri, ExecutedById, StrategyId) " +
            //               $"VALUES('{executeOn:yyyy-MM-dd HH:mm:ss.fff}', '{startFrom:yyyy-MM-dd HH:mm:ss.fff}', '{endTo:yyyy-MM-dd HH:mm:ss.fff}', '{reportUri}', {executedBy}, {strategyId}); ";
            //var result = _dbContext.Database.ExecuteSqlCommand(query);
        }
    }
}
