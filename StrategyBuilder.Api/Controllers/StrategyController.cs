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

        public StrategyController(IStrategyService strategyService)
        {
            _strategyService = strategyService;
        }

        // GET: api/<StrategyController>
        [HttpGet]
        public IEnumerable<Strategy> GetAllStrategiesByUserId()
        {
            int userId = 2;
            var eventList = _strategyService.GetAllStrategiesByUserId(userId);
            return eventList;
        }

        // GET api/<StrategyController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<StrategyController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<StrategyController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StrategyController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
