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
    public class StrategyController : ControllerBase
    {
        private IStrategyService _strategyService;
        private IUserService _userService;

        public StrategyController(IStrategyService strategyService, IUserService userService)
        {
            _strategyService = strategyService;
            _userService = userService;
        }

        // GET: api/<StrategyController>
        [HttpGet("{userid}")]
        public IEnumerable<Strategy> GetAllStrategiesByUserId(int userid)
        {
            var eventList = _strategyService.GetAllStrategiesByUserId(userid);
            return eventList;
        }

        //// GET api/<StrategyController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<StrategyController>
        [HttpPost]
        public IActionResult Post([FromBody] Strategy strategy)
        {
            _strategyService.AddNewStrategy(strategy);
            return Ok();
        }

        // PUT api/<StrategyController>/5
        [HttpPut("{strategyId}/eventgroups")]
        public IActionResult UpdateEventGroupsInStrategy(int strategyId, [FromBody]IEnumerable<JoinStrategyEventGroup> strategyEventGroups)
        {
            _strategyService.UpdateEventGroupsInStrategy(strategyId, strategyEventGroups);
            return Ok();
        }

        // DELETE api/<StrategyController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
