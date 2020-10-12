using System;
using System.Collections.Generic;
using StrategyBuilder.Repository.Entities;

namespace StrategyBuilder.Service.Interfaces
{
    public interface IEventService
    {
        void CreateEventGroup(string name, string description = null);
        void AddEvents(int eventGroupId, IEnumerable<DateTime> occurrances);
        void RemoveEvents(int eventGroupid, IEnumerable<int> eventIds);
        EventGroup GetEventGroupDetailsById(int eventGroupId);
        IEnumerable<EventGroup> GetAllEventGroupsByUserId(int userId);
    }
}
