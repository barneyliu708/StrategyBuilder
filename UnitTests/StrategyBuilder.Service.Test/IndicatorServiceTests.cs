using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using StrategyBuilder.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyBuilder.Service.Test
{
    public class IndicatorServiceTests
    {
        private Mock<DbContext> dbcontext;
        private IIndicatorService indicatorService;
        [SetUp]
        public void Init()
        {
            dbcontext = new Mock<DbContext>();
            // indicatorService = new IndicatorService(dbcontext.Object);
        }

        [Test]
        public void TestGetEventsFromExpression()
        {
            // var result = indicatorService.GetEventsFromExpression("");
        }
    }
}
