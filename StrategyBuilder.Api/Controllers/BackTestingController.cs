using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StrategyBuilder.Service.Interfaces;

namespace StrategyBuilder.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackTestingController : ControllerBase
    {
        private IBackTestingEngine _backTestingEngine;

        public BackTestingController(IBackTestingEngine backTestingEngine)
        {
            _backTestingEngine = backTestingEngine;
        }

        [HttpGet]
        [Route("Execute")]
        public async Task Execute(string from, string to, string symbol, string strategyId)
        {
            DateTime fromDate = DateTime.Parse(from);
            DateTime toDate = DateTime.Parse(to);
            int id = int.Parse(strategyId);
            await _backTestingEngine.Execute(fromDate, toDate, symbol, id);
        } 
    }
}
