using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using StrategyBuilder.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StrategyBuilder.Repository;
using StrategyBuilder.Repository.Entities;
using StrategyBuilder.Repository.Interfaces;

namespace StrategyBuilder.Service.Test
{
    public class IndicatorServiceTests
    {
        private Mock<DbContext> dbcontext;
        private Mock<IStockDataRepo> stockDataRepo;
        private IIndicatorService indicatorService;
        
        [SetUp]
        public void Init()
        {
            dbcontext = new Mock<DbContext>();
            dbcontext.Setup(p => p.SaveChanges()).Returns(1);
            stockDataRepo = new Mock<IStockDataRepo>();
            indicatorService = new IndicatorService(dbcontext.Object, stockDataRepo.Object);
        }

        [Test, Description("Test GetEventsFromExpression: Get events successfully from the expression that Google stock price become less than 30-days moving average price in 2020")]
        public async Task TestGetEventsFromExpression_WhenGOOGIsLessThanMovingAverage()
        {
            // arrange
            DateTime from = DateTime.Parse("2020-01-01");
            DateTime to = DateTime.Parse("2020-12-31");
            stockDataRepo.Setup(x => x.GetStockPriceAdjustDaily(from, to, "GOOG")).ReturnsAsync(new Dictionary<DateTime, StockPriceAdjustDaily>()
            {
                { DateTime.Parse("2020-03-01"), new StockPriceAdjustDaily() { Closed = 150 } },
                { DateTime.Parse("2020-03-02"), new StockPriceAdjustDaily() { Closed = 120 } },
                { DateTime.Parse("2020-03-03"), new StockPriceAdjustDaily() { Closed = 90 } },
                { DateTime.Parse("2020-03-04"), new StockPriceAdjustDaily() { Closed = 70 } }
            });
            stockDataRepo.Setup(x => x.GetStockTechIndicator(from, to, "GOOG", "SMA")).ReturnsAsync(new Dictionary<DateTime, StockPriceAdjustDaily>()
            {
                { DateTime.Parse("2020-03-01"), new StockPriceAdjustDaily() { Closed = 120 } },
                { DateTime.Parse("2020-03-02"), new StockPriceAdjustDaily() { Closed = 110 } },
                { DateTime.Parse("2020-03-03"), new StockPriceAdjustDaily() { Closed = 100 } },
                { DateTime.Parse("2020-03-04"), new StockPriceAdjustDaily() { Closed = 90 } }
            });

            // act
            var result = await indicatorService.GetEventsFromExpression(from, to, "{Symbol:GOOG};{Comparator:Less};{Indicator:SMA}");

            // assert
            Assert.AreEqual(1, result.Count(), "One event should be returned");
            Assert.AreEqual(DateTime.Parse("2020-03-03"), result.ToArray()[0].Occurrence, "On 2020-03-03, Google stock price become less that moving average price");
        }

        [Test, Description("Test GetEventsFromExpression: Get events successfully from the expression that Apple stock price with 0.9 multiplier become less than 30-days moving average price in 2020")]
        public async Task TestGetEventsFromExpression_WhenAAPLWithMultiplierIsLessThanMovingAverage()
        {
            // arrange
            DateTime from = DateTime.Parse("2020-01-01");
            DateTime to = DateTime.Parse("2020-12-31");
            stockDataRepo.Setup(x => x.GetStockPriceAdjustDaily(from, to, "AAPL")).ReturnsAsync(new Dictionary<DateTime, StockPriceAdjustDaily>()
            {
                { DateTime.Parse("2020-03-01"), new StockPriceAdjustDaily() { Closed = 150 } },
                { DateTime.Parse("2020-03-02"), new StockPriceAdjustDaily() { Closed = 120 } },
                { DateTime.Parse("2020-03-03"), new StockPriceAdjustDaily() { Closed = 90 } },
                { DateTime.Parse("2020-03-04"), new StockPriceAdjustDaily() { Closed = 70 } }
            });
            stockDataRepo.Setup(x => x.GetStockTechIndicator(from, to, "AAPL", "SMA")).ReturnsAsync(new Dictionary<DateTime, StockPriceAdjustDaily>()
            {
                { DateTime.Parse("2020-03-01"), new StockPriceAdjustDaily() { Closed = 120 } },
                { DateTime.Parse("2020-03-02"), new StockPriceAdjustDaily() { Closed = 110 } },
                { DateTime.Parse("2020-03-03"), new StockPriceAdjustDaily() { Closed = 100 } },
                { DateTime.Parse("2020-03-04"), new StockPriceAdjustDaily() { Closed = 90 } }
            });

            // act
            var result = await indicatorService.GetEventsFromExpression(from, to, "{Symbol:AAPL};{Operator:Multiplication};{Number:0.9};{Comparator:Less};{Indicator:SMA}");

            // assert
            Assert.AreEqual(1, result.Count(), "One event should be returned");
            Assert.AreEqual(DateTime.Parse("2020-03-02"), result.ToArray()[0].Occurrence, "On 2020-03-02, Google stock price become less that moving average price");
        }

        [Test, Description("Test GetEventsFromExpression: Get empty events from the expression when the date range is invalid")]
        public async Task TestGetEventsFromExpression_WhenDateRangeIsInvalid()
        {
            // arrange
            DateTime from = DateTime.Parse("2021-01-01");
            DateTime to = DateTime.Parse("2020-01-01");
            stockDataRepo.Setup(x => x.GetStockPriceAdjustDaily(from, to, "AAPL")).ReturnsAsync(new Dictionary<DateTime, StockPriceAdjustDaily>()
            {
                { DateTime.Parse("2020-03-01"), new StockPriceAdjustDaily() { Closed = 150 } },
                { DateTime.Parse("2020-03-02"), new StockPriceAdjustDaily() { Closed = 120 } },
                { DateTime.Parse("2020-03-03"), new StockPriceAdjustDaily() { Closed = 90 } },
                { DateTime.Parse("2020-03-04"), new StockPriceAdjustDaily() { Closed = 70 } }
            });
            stockDataRepo.Setup(x => x.GetStockTechIndicator(from, to, "AAPL", "SMA")).ReturnsAsync(new Dictionary<DateTime, StockPriceAdjustDaily>()
            {
                { DateTime.Parse("2020-03-01"), new StockPriceAdjustDaily() { Closed = 120 } },
                { DateTime.Parse("2020-03-02"), new StockPriceAdjustDaily() { Closed = 110 } },
                { DateTime.Parse("2020-03-03"), new StockPriceAdjustDaily() { Closed = 100 } },
                { DateTime.Parse("2020-03-04"), new StockPriceAdjustDaily() { Closed = 90 } }
            });

            // act
            var result = await indicatorService.GetEventsFromExpression(from, to, "{Symbol:AAPL};{Operator:Multiplication};{Number:0.9};{Comparator:Less};{Indicator:SMA}");

            // assert
            Assert.AreEqual(0, result.Count(), "Empty data should be return when the date range is invalid");
        }

        [Test, Description("Test GetAllIndicators: Get all Indicator successfully")]
        public void TestGetAllIndicators_GetDataSuccessfully()
        {
            // arrange
            var indicators = new List<Indicator>() 
            {
                new Indicator() { Id = 1, Key = "key1"},
                new Indicator() { Id = 2, Key = "key2"}
            };
            dbcontext.Setup(p => p.Set<Indicator>()).Returns(DbContextMock.GetQueryableMockDbSet<Indicator>(indicators));

            // act
            var result = indicatorService.GetAllIndicators();

            // assert
            Assert.AreEqual(2, result.Count(), "All the 2 indicators in the database should be returned");
        }

        [Test, Description("Test GetAllIndicators: Failed to connect database and an exception is thrown")]
        public void TestGetAllIndicators_FailedToConnectDatabase()
        {
            // arrange
            var indicators = new List<Indicator>()
            {
                new Indicator() { Id = 1, Key = "key1"},
                new Indicator() { Id = 2, Key = "key2"}
            };
            dbcontext.Setup(p => p.Set<Indicator>()).Throws(new TimeoutException("Timeout exception test."));

            // act + assert
            Assert.Throws<TimeoutException>(() => indicatorService.GetAllIndicators(), "Failed to connect database and the timeout exception is thrown");
        }
    }
}
