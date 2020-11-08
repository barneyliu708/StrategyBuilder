﻿using Microsoft.EntityFrameworkCore;
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
                             .Where(e => e.CreatedBy.Id == userId)
                             .Include(e => e.EventGroups);
        }
    }
}
