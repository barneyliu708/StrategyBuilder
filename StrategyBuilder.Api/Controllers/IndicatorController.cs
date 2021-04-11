using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StrategyBuilder.Model;
using StrategyBuilder.Repository.Entities;
using StrategyBuilder.Service.Interfaces;

namespace StrategyBuilder.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndicatorController : ControllerBase
    {
        private IIndicatorService _indicatorService;

        public IndicatorController(IIndicatorService indicatorService)
        {
            _indicatorService = indicatorService;
        }

        [HttpGet]
        public IEnumerable<Indicator> Get()
        {
            return _indicatorService.GetAllIndicators();
        }

        [HttpPost]
        [Route("ExecuteExpression")]
        public async Task<IEnumerable<Event>> GetEventsFromExpression([FromBody]ExpressionRequest request)
        {
            DateTime fromDate = DateTime.Parse(request.from).Date;
            DateTime toDate = DateTime.Parse(request.to).Date;
            return await _indicatorService.GetEventsFromExpression(fromDate, toDate, request.Expression);
        }
    }
}
