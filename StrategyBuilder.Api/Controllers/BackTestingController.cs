using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StrategyBuilder.Model;
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

        [HttpPost]
        [Route("Execute")]
        public async Task Execute([FromBody]BackTestingRequest request)
        {
            DateTime fromDate = DateTime.Parse(request.from).Date;
            DateTime toDate = DateTime.Parse(request.to).Date;
            await _backTestingEngine.Execute(fromDate, toDate, request.SimbolList, request.StrategyId);
        }

        [HttpDelete]
        public IActionResult DeleteBackTestingResult(int resultID)
        {
            _backTestingEngine.DeleteBackTestingResult(resultID);
            return Ok();
        }
    }
}
