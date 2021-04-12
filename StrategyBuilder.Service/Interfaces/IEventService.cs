using System;
using System.Collections.Generic;
using StrategyBuilder.Repository.Entities;

namespace StrategyBuilder.Service.Interfaces
{
    public interface IEventService
    {
        void AddEventGroup(EventGroup eventGroup);
        void UpdateEvents(int eventGroupId, IEnumerable<DateTime> occurrances);
        void UpdateEventGroup(int eventGroupid, EventGroup eventGroup);
        IEnumerable<EventGroup> GetAllEventGroupsByUserId(int userId);
    }
}
