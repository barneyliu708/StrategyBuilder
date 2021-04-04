using StrategyBuilder.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyBuilder.Service.Interfaces
{
    public interface IStrategyService
    {
        IEnumerable<Strategy> GetAllStrategiesByUserId(int userId);
        Strategy GetStrategiesByStrategyId(int strategyId);
        void AddNewStrategy(Strategy strategy);
        void UpdateEventGroupsInStrategy(int strategyId, IEnumerable<JoinStrategyEventGroup> eventGroupIds);
        int InsertIntoBackTestingResult(DateTime executeOn, DateTime startFrom, DateTime endTo, string reportUri, int executedBy, int strategyId);
    }
}
