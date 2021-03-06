﻿using Microsoft.EntityFrameworkCore;
using StrategyBuilder.Repository.Entities;
using StrategyBuilder.Service.Interfaces;
using System.Linq;

namespace StrategyBuilder.Service
{
    public class UserService : BaseService, IUserService
    {
        public UserService(DbContext dbContext)
            : base(dbContext)
        {

        }

        public int GetValidUserId()
        {
            return 1;
            //return _dbContext.Set<Strategy>()
            //                 .Where(s => s.CreatedBy != null)
            //                 .Include(s => s.CreatedBy)
            //                 .FirstOrDefault()
            //                 .CreatedBy.Id;
        }

        public void  AddUser(User newUser)
        {
            _dbContext.Set<User>().Add(newUser);
            _dbContext.SaveChanges();
        }
    }
}
