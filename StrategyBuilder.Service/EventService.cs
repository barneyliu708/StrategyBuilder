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
            //var events = _dbContext.Set<Event>().Where(e => e.EventGroup.Id == eventGroupId);
            //_dbContext.RemoveRange(events);
            //_dbContext.SaveChanges();
            //var newEvents = occurrances.Select(o => new Event() { Occurrence = o, EventGroup = })
            //_dbContext.AddRange();
        }

        public void CreateEventGroup(EventGroup eventGroup)
        {
            string query = $"Insert Into [EventGroups] ([Name], [Description], [CreatedById]) Values('{eventGroup.Name}', '{eventGroup.Description}', {eventGroup.CreatedBy.Id})";
            _dbContext.Database.ExecuteSqlCommand(query);
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
