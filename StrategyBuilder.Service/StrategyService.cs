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
            string query = $"  Insert Into [Strategies] ([Name], [Description], [CreatedById]) Values('{strategy.Name}', '{strategy.Description}', {strategy.CreatedBy.Id})";
            _dbContext.Database.ExecuteSqlCommand(query);
        }

        public IEnumerable<Strategy> GetAllStrategiesByUserId(int userId)
        {
            return _dbContext.Set<Strategy>()
                             .Where(s => s.CreatedBy.Id == userId)
                             .Include(s => s.EventGroups)
                             .Include(s => s.BackTestingResults)
                             .OrderByDescending(s => s.Id);
        }

        public Strategy GetStrategiesByStrategyId(int strategyId)
        {
            return _dbContext.Set<Strategy>()
                             .Where(s => s.Id == strategyId)
                             .Include(s => s.CreatedBy)
                             .Include(s => s.EventGroups).ThenInclude(s => s.Events)
                             .Include(s => s.BackTestingResults)
                             .FirstOrDefault();
        }

        public void UpdateEventGroupsInStrategy(int strategyId, IEnumerable<int> eventGroupIds)
        {
            string queryold = $"Update [EventGroups] Set StrategyId = NULL Where StrategyId = {strategyId}";
            string querynew = $"Update [EventGroups] Set StrategyId = {strategyId} Where [Id] in ({string.Join(",", eventGroupIds)})";
            _dbContext.Database.ExecuteSqlCommand(queryold);
            _dbContext.Database.ExecuteSqlCommand(querynew);
        }

        public int InsertIntoBackTestingResult(DateTime executeOn, DateTime startFrom, DateTime endTo, string reportUri, int executedBy, int strategyId)
        {
            string query = $"INSERT INTO BackTestingResults (ExecutedOn, StartFrom, EndTo, ReportFileUri, ExecutedById, StrategyId) " +
                           $"VALUES('{executeOn:yyyy-MM-dd HH:mm:ss.fff}', '{startFrom:yyyy-MM-dd HH:mm:ss.fff}', '{endTo:yyyy-MM-dd HH:mm:ss.fff}', '{reportUri}', {executedBy}, {strategyId}); ";
            var result = _dbContext.Database.ExecuteSqlCommand(query);
            return result;
        }
    }
}
