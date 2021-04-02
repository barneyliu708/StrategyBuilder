using StrategyBuilder.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyBuilder.Service.Interfaces
{
    public interface IIndicatorService
    {
        IEnumerable<Indicator> GetAllIndicators();
    }
}
