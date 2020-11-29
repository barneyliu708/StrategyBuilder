using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StrategyBuilder.Repository.Entities;
using StrategyBuilder.Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StrategyBuilder.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventGroupController : ControllerBase
    {
        private IEventService _eventService;
        private IUserService _userService;

        public EventGroupController(IEventService eventService, IUserService userService)
        {
            _eventService = eventService;
            _userService = userService;
        }

        // GET: api/<EventGroupController>
        [HttpGet]
        public IEnumerable<EventGroup> GetAllEventGroupsByUserId()
        {
            int userId = _userService.GetValidUserId();
            var eventList = _eventService.GetAllEventGroupsByUserId(userId);
            return eventList;
        }

        // GET api/<EventGroupController>/5
        [HttpGet("{eventgoupid}")]
        public string GetEventGroupDetails(int eventgoupid)
        {
            return "value";
        }

        // POST api/<EventGroupController>
        [HttpPost]
        public IActionResult Post([FromBody] EventGroup eventGroup)
        {
            _eventService.CreateEventGroup(eventGroup);
            return Ok();
        }

        // PUT api/<EventGroupController>/5
        [HttpPut("{eventgoupid}")]
        public void Put(int eventgoupid, [FromBody] string name)
        {
        }

        // DELETE api/<EventGroupController>/5
        [HttpDelete("{eventgoupid}")]
        public void Delete(int eventgoupid)
        {
        }
    }
}
