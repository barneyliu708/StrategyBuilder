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

        public IEnumerable<Strategy> GetAllStrategiesByUserId(int userId)
        {
            return _dbContext.Set<Strategy>()
                             .Where(s => s.CreatedBy.Id == userId)
                             .Include(s => s.EventGroups)
                             .Include(s => s.BackTestingResults);
        }

        public Strategy GetStrategiesByStrategyId(int strategyId)
        {
            return _dbContext.Set<Strategy>()
                             .Where(s => s.Id == strategyId)
                             .Include(s => s.EventGroups).ThenInclude(s => s.Events)
                             .Include(s => s.BackTestingResults)
                             .FirstOrDefault();
        }
    }
}
