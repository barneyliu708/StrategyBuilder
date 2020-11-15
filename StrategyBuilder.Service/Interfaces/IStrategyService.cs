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
    }
}
