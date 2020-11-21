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
        void UpdateEventGroupsInStrategy(int strategyId, IEnumerable<int> eventGroupIds);
    }
}
