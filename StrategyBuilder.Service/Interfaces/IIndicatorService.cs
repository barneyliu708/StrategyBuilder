using StrategyBuilder.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.Service.Interfaces
{
    public interface IIndicatorService
    {
        IEnumerable<Indicator> GetAllIndicators();
        Task<IEnumerable<Event>> GetEventsFromExpression(DateTime from, DateTime to, string expression);
    }
}
