using System;
using System.Collections.Generic;
using StrategyBuilder.Repository.Entities;

namespace StrategyBuilder.Service.Interfaces
{
    public interface IEventService
    {
        void AddEventGroup(EventGroup eventGroup);
        void UpdateEvents(int eventGroupId, IEnumerable<DateTime> occurrances);
        void UpdateExpressions(int eventGroupId, IEnumerable<string> expressions);
        void RemoveEvents(int eventGroupid, IEnumerable<int> eventIds);
        EventGroup GetEventGroupDetailsById(int eventGroupId);
        IEnumerable<EventGroup> GetAllEventGroupsByUserId(int userId);
    }
}
