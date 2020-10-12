using Microsoft.EntityFrameworkCore;

namespace StrategyBuilder.Service
{
    public abstract class BaseService
    {
        protected DbContext _dbContext;

        public BaseService(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
