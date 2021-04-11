using StrategyBuilder.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.Repository.Interfaces
{
    // API key 2UE2F3PIEK3EAKYP
    public interface IStockDataRepo
    {
        Task<Dictionary<DateTime, StockPriceAdjustDaily>> GetStockPriceAdjustDaily(DateTime from, DateTime to, string symbol);
        Task<Dictionary<DateTime, StockPriceAdjustDaily>> GetStockTechIndicator(DateTime from, DateTime to, string symbol, string function);
    }
}
