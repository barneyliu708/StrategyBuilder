using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.Service.Interfaces
{
    public interface IBackTestingEngine
    {
        Task Execute(DateTime from, DateTime to, string[] symbolList, int strategyIds);
        void DeleteBackTestingResult(int resultId);
    }
}
