using Microsoft.EntityFrameworkCore;
using StrategyBuilder.Repository.Entities;
using StrategyBuilder.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StrategyBuilder.Service
{
    public class IndicatorService : BaseService, IIndicatorService
    {
        public IndicatorService(DbContext dbContext) : base(dbContext)
        {

        }

        public IEnumerable<Indicator> GetAllIndicators()
        {
            return _dbContext.Set<Indicator>().ToList();
        }
    }
}
