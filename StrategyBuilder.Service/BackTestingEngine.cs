using Microsoft.EntityFrameworkCore;
using StrategyBuilder.Repository.Entities;
using StrategyBuilder.Repository.Interfaces;
using StrategyBuilder.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StrategyBuilder.Service
{
    public class BackTestingEngine : BaseService, IBackTestingEngine
    {
        private IStockDataRepo _stockDataRepo;
        private IStrategyService _strategyService;

        public BackTestingEngine(DbContext dbContext, IStockDataRepo stockDataRepo, IStrategyService strategyService) : base(dbContext) 
        {
            _stockDataRepo = stockDataRepo;
            _strategyService = strategyService;
        } 

        public async Task Execute(DateTime from, DateTime to, string symbol, int strategyId)
        {
            var stockprices = await _stockDataRepo.GetStockPriceAdjustDaily(from, to, symbol);
            var strategy = _strategyService.GetAllStrategiesByUserId(strategyId);
        }
    }
}
