using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using StrategyBuilder.Model;
using StrategyBuilder.Repository.Entities;
using StrategyBuilder.Repository.Interfaces;
using StrategyBuilder.Service.Interfaces;

namespace StrategyBuilder.Service.Test
{
    public class BackTestingEngineTests
    {
        private Mock<DbContext> dbcontext;
        private Mock<IStockDataRepo> stockDataRepo;
        private Mock<IStrategyService> strategyService;
        private Mock<IReportGenerator> reportGenerator;
        private BackTestingEngine backTestingEngine;

        [SetUp]
        public void Init()
        {
            dbcontext = new Mock<DbContext>();
            dbcontext.Setup(p => p.SaveChanges()).Returns(1);

            stockDataRepo = new Mock<IStockDataRepo>();
            strategyService = new Mock<IStrategyService>();
            reportGenerator = new Mock<IReportGenerator>();

            backTestingEngine = new BackTestingEngine(dbcontext.Object, stockDataRepo.Object, strategyService.Object, reportGenerator.Object);
        }

        [Test, Description("Test Execute: Back-testing execution completed successfully")]
        public async Task TestExecute_ExecutionSuccess()
        {
            Dictionary<string, NegativeIndexArray<decimal>> _meanResult = new Dictionary<string, NegativeIndexArray<decimal>>();
            List<Account> _accountPerformance = new List<Account>();
            // arrange
            var user123 = new User() { Id = 123, Username = "testusername123" };
            var strategy = new Strategy()
            {
                Id = 1,
                CreatedBy = user123,
                Name = "name1",
                Description = "description1",
                JoinStrategyEventGroups = new List<JoinStrategyEventGroup>()
                {
                    new JoinStrategyEventGroup() { StrategyId = 1, EventGroupId = 11, Action = "Buy", AfterDays = 1, Percentage = 40, EventGroup = new EventGroup() { Id = 11, Events = new List<Event>()
                    {
                        new Event() { Id = 1001, Occurrence = DateTime.Parse("2020-03-10") }
                    }}},
                    new JoinStrategyEventGroup() { StrategyId = 1, EventGroupId = 12, Action = "Buy", AfterDays = 1, Percentage = 40, EventGroup = new EventGroup() { Id = 11, Events = new List<Event>()
                    {
                        new Event() { Id = 1001, Occurrence = DateTime.Parse("2020-03-20") }
                    }}}
                }
            };
            dbcontext.Setup(p => p.Set<User>()).Returns(DbContextMock.GetQueryableMockDbSet<User>(new List<User>() { user123 }));
            dbcontext.Setup(p => p.Set<Strategy>()).Returns(DbContextMock.GetQueryableMockDbSet<Strategy>(new List<Strategy>() { strategy }));

            DateTime from = DateTime.Parse("2020-03-01");
            DateTime to = DateTime.Parse("2020-3-31");
            stockDataRepo.Setup(x => x.GetStockPriceAdjustDaily(from, to, "GOOG")).ReturnsAsync(new Dictionary<DateTime, StockPriceAdjustDaily>()
            {
                { DateTime.Parse("2020-03-01"), new StockPriceAdjustDaily() { Closed = 100 } },
                { DateTime.Parse("2020-03-02"), new StockPriceAdjustDaily() { Closed = 104 } },
                { DateTime.Parse("2020-03-03"), new StockPriceAdjustDaily() { Closed = 101 } },
                { DateTime.Parse("2020-03-05"), new StockPriceAdjustDaily() { Closed = 106 } },
                { DateTime.Parse("2020-03-06"), new StockPriceAdjustDaily() { Closed = 104 } },
                { DateTime.Parse("2020-03-07"), new StockPriceAdjustDaily() { Closed = 109 } },
                { DateTime.Parse("2020-03-08"), new StockPriceAdjustDaily() { Closed = 107 } },
                { DateTime.Parse("2020-03-09"), new StockPriceAdjustDaily() { Closed = 109 } },
                { DateTime.Parse("2020-03-10"), new StockPriceAdjustDaily() { Closed = 110 } },
                { DateTime.Parse("2020-03-11"), new StockPriceAdjustDaily() { Closed = 120 } },
                { DateTime.Parse("2020-03-12"), new StockPriceAdjustDaily() { Closed = 125 } },
                { DateTime.Parse("2020-03-13"), new StockPriceAdjustDaily() { Closed = 124 } },
                { DateTime.Parse("2020-03-14"), new StockPriceAdjustDaily() { Closed = 127 } },
                { DateTime.Parse("2020-03-15"), new StockPriceAdjustDaily() { Closed = 130 } },
                { DateTime.Parse("2020-03-16"), new StockPriceAdjustDaily() { Closed = 125 } },
                { DateTime.Parse("2020-03-17"), new StockPriceAdjustDaily() { Closed = 125 } },
                { DateTime.Parse("2020-03-18"), new StockPriceAdjustDaily() { Closed = 134 } },
                { DateTime.Parse("2020-03-19"), new StockPriceAdjustDaily() { Closed = 133 } },
                { DateTime.Parse("2020-03-20"), new StockPriceAdjustDaily() { Closed = 136 } },
                { DateTime.Parse("2020-03-21"), new StockPriceAdjustDaily() { Closed = 145 } },
                { DateTime.Parse("2020-03-22"), new StockPriceAdjustDaily() { Closed = 150 } },
                { DateTime.Parse("2020-03-23"), new StockPriceAdjustDaily() { Closed = 151 } },
                { DateTime.Parse("2020-03-24"), new StockPriceAdjustDaily() { Closed = 152 } },
                { DateTime.Parse("2020-03-25"), new StockPriceAdjustDaily() { Closed = 153 } },
                { DateTime.Parse("2020-03-26"), new StockPriceAdjustDaily() { Closed = 154 } },
                { DateTime.Parse("2020-03-27"), new StockPriceAdjustDaily() { Closed = 155 } },
                { DateTime.Parse("2020-03-28"), new StockPriceAdjustDaily() { Closed = 156 } },
                { DateTime.Parse("2020-03-30"), new StockPriceAdjustDaily() { Closed = 157 } },
            });
            stockDataRepo.Setup(x => x.GetStockPriceAdjustDaily(from, to, "AAPL")).ReturnsAsync(new Dictionary<DateTime, StockPriceAdjustDaily>()
            {
                { DateTime.Parse("2020-03-01"), new StockPriceAdjustDaily() { Closed = 100 } },
                { DateTime.Parse("2020-03-02"), new StockPriceAdjustDaily() { Closed = 104 } },
                { DateTime.Parse("2020-03-03"), new StockPriceAdjustDaily() { Closed = 101 } },
                { DateTime.Parse("2020-03-05"), new StockPriceAdjustDaily() { Closed = 106 } },
                { DateTime.Parse("2020-03-06"), new StockPriceAdjustDaily() { Closed = 104 } },
                { DateTime.Parse("2020-03-07"), new StockPriceAdjustDaily() { Closed = 109 } },
                { DateTime.Parse("2020-03-08"), new StockPriceAdjustDaily() { Closed = 107 } },
                { DateTime.Parse("2020-03-09"), new StockPriceAdjustDaily() { Closed = 109 } },
                { DateTime.Parse("2020-03-10"), new StockPriceAdjustDaily() { Closed = 110 } },
                { DateTime.Parse("2020-03-11"), new StockPriceAdjustDaily() { Closed = 120 } },
                { DateTime.Parse("2020-03-12"), new StockPriceAdjustDaily() { Closed = 125 } },
                { DateTime.Parse("2020-03-13"), new StockPriceAdjustDaily() { Closed = 124 } },
                { DateTime.Parse("2020-03-14"), new StockPriceAdjustDaily() { Closed = 127 } },
                { DateTime.Parse("2020-03-15"), new StockPriceAdjustDaily() { Closed = 130 } },
                { DateTime.Parse("2020-03-16"), new StockPriceAdjustDaily() { Closed = 125 } },
                { DateTime.Parse("2020-03-17"), new StockPriceAdjustDaily() { Closed = 125 } },
                { DateTime.Parse("2020-03-18"), new StockPriceAdjustDaily() { Closed = 134 } },
                { DateTime.Parse("2020-03-19"), new StockPriceAdjustDaily() { Closed = 133 } },
                { DateTime.Parse("2020-03-20"), new StockPriceAdjustDaily() { Closed = 136 } },
                { DateTime.Parse("2020-03-21"), new StockPriceAdjustDaily() { Closed = 145 } },
                { DateTime.Parse("2020-03-22"), new StockPriceAdjustDaily() { Closed = 150 } },
                { DateTime.Parse("2020-03-23"), new StockPriceAdjustDaily() { Closed = 151 } },
                { DateTime.Parse("2020-03-24"), new StockPriceAdjustDaily() { Closed = 152 } },
                { DateTime.Parse("2020-03-25"), new StockPriceAdjustDaily() { Closed = 153 } },
                { DateTime.Parse("2020-03-26"), new StockPriceAdjustDaily() { Closed = 154 } },
                { DateTime.Parse("2020-03-27"), new StockPriceAdjustDaily() { Closed = 155 } },
                { DateTime.Parse("2020-03-28"), new StockPriceAdjustDaily() { Closed = 156 } },
                { DateTime.Parse("2020-03-30"), new StockPriceAdjustDaily() { Closed = 157 } },
            });
            stockDataRepo.Setup(x => x.GetStockPriceAdjustDaily(from, to, "IVV")).ReturnsAsync(new Dictionary<DateTime, StockPriceAdjustDaily>()
            {
                { DateTime.Parse("2020-03-01"), new StockPriceAdjustDaily() { Closed = 100 } },
                { DateTime.Parse("2020-03-02"), new StockPriceAdjustDaily() { Closed = 104 } },
                { DateTime.Parse("2020-03-03"), new StockPriceAdjustDaily() { Closed = 101 } },
                { DateTime.Parse("2020-03-05"), new StockPriceAdjustDaily() { Closed = 106 } },
                { DateTime.Parse("2020-03-06"), new StockPriceAdjustDaily() { Closed = 104 } },
                { DateTime.Parse("2020-03-07"), new StockPriceAdjustDaily() { Closed = 109 } },
                { DateTime.Parse("2020-03-08"), new StockPriceAdjustDaily() { Closed = 107 } },
                { DateTime.Parse("2020-03-09"), new StockPriceAdjustDaily() { Closed = 109 } },
                { DateTime.Parse("2020-03-10"), new StockPriceAdjustDaily() { Closed = 110 } },
                { DateTime.Parse("2020-03-11"), new StockPriceAdjustDaily() { Closed = 110 } },
                { DateTime.Parse("2020-03-12"), new StockPriceAdjustDaily() { Closed = 115 } },
                { DateTime.Parse("2020-03-13"), new StockPriceAdjustDaily() { Closed = 114 } },
                { DateTime.Parse("2020-03-14"), new StockPriceAdjustDaily() { Closed = 117 } },
                { DateTime.Parse("2020-03-15"), new StockPriceAdjustDaily() { Closed = 110 } },
                { DateTime.Parse("2020-03-16"), new StockPriceAdjustDaily() { Closed = 125 } },
                { DateTime.Parse("2020-03-17"), new StockPriceAdjustDaily() { Closed = 115 } },
                { DateTime.Parse("2020-03-18"), new StockPriceAdjustDaily() { Closed = 124 } },
                { DateTime.Parse("2020-03-19"), new StockPriceAdjustDaily() { Closed = 123 } },
                { DateTime.Parse("2020-03-20"), new StockPriceAdjustDaily() { Closed = 126 } },
                { DateTime.Parse("2020-03-21"), new StockPriceAdjustDaily() { Closed = 135 } },
                { DateTime.Parse("2020-03-22"), new StockPriceAdjustDaily() { Closed = 140 } },
                { DateTime.Parse("2020-03-23"), new StockPriceAdjustDaily() { Closed = 131 } },
                { DateTime.Parse("2020-03-24"), new StockPriceAdjustDaily() { Closed = 132 } },
                { DateTime.Parse("2020-03-25"), new StockPriceAdjustDaily() { Closed = 133 } },
                { DateTime.Parse("2020-03-26"), new StockPriceAdjustDaily() { Closed = 134 } },
                { DateTime.Parse("2020-03-27"), new StockPriceAdjustDaily() { Closed = 135 } },
                { DateTime.Parse("2020-03-28"), new StockPriceAdjustDaily() { Closed = 136 } },
                { DateTime.Parse("2020-03-30"), new StockPriceAdjustDaily() { Closed = 137 } },
            });

            strategyService.Setup(x => x.GetStrategiesByStrategyId(1)).Returns(strategy);

            reportGenerator.Setup(x => x.GenerateReport(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string[]>(),
                    It.IsAny<string[]>(),
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<Dictionary<string, NegativeIndexArray<decimal>>>(),
                    It.IsAny<List<Account>>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>()))
                .Callback<string, string, string[], string[], int, int, Dictionary<string, NegativeIndexArray<decimal>>,
                    List<Account>, DateTime, DateTime, DateTime>(
                    (string strategyName,
                        string description,
                        string[] symbolList,
                        string[] eventNames,
                        int eventdatefrom,
                        int eventdateto,
                        Dictionary<string, NegativeIndexArray<decimal>> meanResult,
                        List<Account> accountPerformance,
                        DateTime executedOn,
                        DateTime executeFrom,
                        DateTime executeTo) =>
                    {
                        _meanResult = meanResult;
                        _accountPerformance = accountPerformance;
                    });

            // act
            await backTestingEngine.Execute(from, to, new[] {"GOOG", "AAPL"}, 1);

            // assert
            Assert.AreEqual(2, _meanResult.Keys.Count, "There should be 2 mean result as there are to symbols being tested");
            Assert.AreEqual(31, _accountPerformance.Count, "There should be 31 performance results as the back-testing range is from 2020-03-01 to 2020-03-31");
        }
    }
}
