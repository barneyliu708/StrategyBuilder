using Microsoft.AspNetCore.Mvc;
using StrategyBuilder.Repository.Entities;
using StrategyBuilder.Service.Interfaces;
using System.Collections.Generic;

namespace StrategyBuilder.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        [Route("{userId}")]
        public IEnumerable<EventGroup> GetAllEventGroupsByUserId(int userId)
        {
            var eventList = _eventService.GetAllEventGroupsByUserId(userId);
            return eventList;
        }
    }
}
