using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
