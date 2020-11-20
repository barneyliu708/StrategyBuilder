using System;
using System.Collections.Generic;
using StrategyBuilder.Repository.Entities;

namespace StrategyBuilder.Service.Interfaces
{
    public interface IEventService
    {
        void CreateEventGroup(EventGroup eventGroup);
        void AddEvents(int eventGroupId, IEnumerable<DateTime> occurrances);
        void RemoveEvents(int eventGroupid, IEnumerable<int> eventIds);
        EventGroup GetEventGroupDetailsById(int eventGroupId);
        IEnumerable<EventGroup> GetAllEventGroupsByUserId(int userId);
    }
}
