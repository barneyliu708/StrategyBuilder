using Microsoft.EntityFrameworkCore;
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

        public void AddEvents(int eventGroupId, IEnumerable<DateTime> occurrances)
        {
            throw new NotImplementedException();
        }

        public void CreateEventGroup(string name, string description = null)
        {

            throw new NotImplementedException();
        }

        public IEnumerable<EventGroup> GetAllEventGroupsByUserId(int userId)
        {
            return _dbContext.Set<EventGroup>()
                             .Where(e => e.CreatedBy.Id == userId)
                             .Include(e => e.Events);
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
