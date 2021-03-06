﻿using StrategyBuilder.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyBuilder.Service.Interfaces
{
    public interface IUserService
    {
        int GetValidUserId();
        void AddUser(User newUser);
    }
}
