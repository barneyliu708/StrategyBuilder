﻿using Microsoft.EntityFrameworkCore;
using StrategyBuilder.Service.Interfaces;
using StrategyBuilder.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StrategyBuilder.Service
{
    public class EventService : BaseService, IEventService
    {
        public EventService(DbContext dbContext) 
            : base(dbContext)
        {

        }

        public void UpdateEvents(int eventGroupId, IEnumerable<DateTime> occurrances)
        {
            var eventGroup = _dbContext.Set<EventGroup>().Include(eg => eg.Events).First(s => s.Id == eventGroupId);
            eventGroup.Events.Clear();
            foreach (var dt in occurrances)
            {
                eventGroup.Events.Add(new Event() { Occurrence = dt });
            }
            _dbContext.SaveChanges();
        }

        public void AddEventGroup(EventGroup eventGroup)
        {
            var user = _dbContext.Set<User>().First(u => u.Id == eventGroup.CreatedBy.Id);
            eventGroup.CreatedBy = user;
            _dbContext.Set<EventGroup>().Add(eventGroup);
            _dbContext.SaveChanges();
        }

        public IEnumerable<EventGroup> GetAllEventGroupsByUserId(int userId)
        {
            return _dbContext.Set<EventGroup>()
                .Include(u => u.Events)
                .Where(eg => eg.CreatedBy.Id == userId);
        }

        public EventGroup GetEventGroupDetailsById(int eventGroupId)
        {
            throw new NotImplementedException();
        }

        public void RemoveEvents(int eventGroupid, IEnumerable<int> eventIds)
        {
            throw new NotImplementedException();
        }
    }
}
