using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using StrategyBuilder.Repository.Entities;

namespace StrategyBuilder.Service.Test
{
    public class StrategyServiceTests
    {
        private Mock<DbContext> dbcontext;
        private StrategyService strategyService;

        [SetUp]
        public void Init()
        {
            dbcontext = new Mock<DbContext>();
            dbcontext.Setup(p => p.SaveChanges()).Returns(1);
            strategyService = new StrategyService(dbcontext.Object);
        }

        [Test, Description("Test AddNewStrategy: A new strategy is added to the user successfully")]
        public void TestAddNewStrategy_NewStrategyAddedSuccessfully()
        {
            // arrange
            var user123 = new User() { Id = 123, Username = "testusername123" };
            var user124 = new User() { Id = 124, Username = "testusername124" };
            var strategyList = new List<Strategy>()
            {
                new Strategy() {Id = 1, CreatedBy = user123, Name = "name1", Description = "description1"},
                new Strategy() {Id = 2, CreatedBy = user123, Name = "name2", Description = "description2"},
                new Strategy() {Id = 3, CreatedBy = user124, Name = "name3", Description = "description3"}
            };
            dbcontext.Setup(p => p.Set<User>()).Returns(DbContextMock.GetQueryableMockDbSet<User>(new List<User>() { user123, user124 }));
            dbcontext.Setup(p => p.Set<Strategy>()).Returns(DbContextMock.GetQueryableMockDbSet<Strategy>(strategyList));

            // act
            var newStrategy = new Strategy() { Id = 4, CreatedBy = user123, Name = "new name", Description = "new description" };
            strategyService.AddNewStrategy(newStrategy);

            // assert
            Assert.AreEqual(3, dbcontext.Object.Set<Strategy>().Where(x => x.CreatedBy.Id == 123).ToList().Count,
                "User 123 should have 3 strategies after the update");
            Assert.AreEqual(1, dbcontext.Object.Set<Strategy>().Where(x => x.CreatedBy.Id == 124).ToList().Count,
                "User 124 should have 1 strategy after the update");
        }

        [Test, Description("Test AddNewStrategy: The new strategy failed be added when the user does not exist")]
        public void TestAddNewStrategy_FaildToAddStrategyWhenUserIsNotValid()
        {
            // arrange
            var user123 = new User() { Id = 123, Username = "testusername123" };
            var user124 = new User() { Id = 124, Username = "testusername124" };
            var strategyList = new List<Strategy>()
            {
                new Strategy() {Id = 1, CreatedBy = user123, Name = "name1", Description = "description1"},
                new Strategy() {Id = 2, CreatedBy = user123, Name = "name2", Description = "description2"},
                new Strategy() {Id = 3, CreatedBy = user124, Name = "name3", Description = "description3"}
            };
            dbcontext.Setup(p => p.Set<User>()).Returns(DbContextMock.GetQueryableMockDbSet<User>(new List<User>() { user123, user124 }));
            dbcontext.Setup(p => p.Set<Strategy>()).Returns(DbContextMock.GetQueryableMockDbSet<Strategy>(strategyList));

            // act + assert
            var newStrategy = new Strategy() { Id = 4, CreatedBy = new User() { Id = 215, Username = "new user" }, Name = "new name", Description = "new description" };
            Assert.Throws<InvalidOperationException>(() => strategyService.AddNewStrategy(newStrategy), "InvalidOperationException should be thrown when the user does not exist.");
        }

        [Test, Description("Test GetAllStrategiesByUserId: All strategies should be returned successfully for the user")]
        public void TestGetAllStrategiesByUserId_AllStrategiesReturnedSuccessfully()
        {
            // arrange
            var user123 = new User() { Id = 123, Username = "testusername123" };
            var user124 = new User() { Id = 124, Username = "testusername124" };
            var strategyList = new List<Strategy>()
            {
                new Strategy() {Id = 1, CreatedBy = user123, Name = "name1", Description = "description1"},
                new Strategy() {Id = 2, CreatedBy = user123, Name = "name2", Description = "description2"},
                new Strategy() {Id = 3, CreatedBy = user124, Name = "name3", Description = "description3"}
            };
            dbcontext.Setup(p => p.Set<User>()).Returns(DbContextMock.GetQueryableMockDbSet<User>(new List<User>() { user123, user124 }));
            dbcontext.Setup(p => p.Set<Strategy>()).Returns(DbContextMock.GetQueryableMockDbSet<Strategy>(strategyList));

            // act
            var result = strategyService.GetAllStrategiesByUserId(123).ToList();

            // assert
            Assert.AreEqual(2, result.Count, "User 123 should have 2 strategies");
            Assert.AreEqual("name1", result[0].Name, "Strategy name1 should be returned for user 123");
            Assert.AreEqual("name2", result[1].Name, "Strategy name2 should be returned for user 123");
        }

        [Test, Description("Test GetAllStrategiesByUserId: Empty strategy list should be returned when the user does not have any strategy created")]
        public void TestGetAllStrategiesByUserId_EmptyStrategyShouldBeReturnWhenUserDoseNotHaveAnyStrategy()
        {
            // arrange
            var user123 = new User() { Id = 123, Username = "testusername123" };
            var user124 = new User() { Id = 124, Username = "testusername124" };
            var strategyList = new List<Strategy>()
            {
                new Strategy() {Id = 1, CreatedBy = user123, Name = "name1", Description = "description1"},
                new Strategy() {Id = 2, CreatedBy = user123, Name = "name2", Description = "description2"},
                new Strategy() {Id = 3, CreatedBy = user124, Name = "name3", Description = "description3"}
            };
            dbcontext.Setup(p => p.Set<User>()).Returns(DbContextMock.GetQueryableMockDbSet<User>(new List<User>() { user123, user124 }));
            dbcontext.Setup(p => p.Set<Strategy>()).Returns(DbContextMock.GetQueryableMockDbSet<Strategy>(strategyList));

            // act
            var result = strategyService.GetAllStrategiesByUserId(215).ToList();

            // assert
            Assert.AreEqual(0, result.Count, "Empty list of strategy should be returned as user 215 does not have any strategy created");
        }

        [Test, Description("Test GetStrategiesByStrategyId: Get strategy successfully by strategy Id")]
        public void TestGetStrategiesByStrategyId_StrategyReturnedSuccessfully()
        {
            // arrange
            var user123 = new User() { Id = 123, Username = "testusername123" };
            var user124 = new User() { Id = 124, Username = "testusername124" };
            var strategyList = new List<Strategy>()
            {
                new Strategy() {Id = 1, CreatedBy = user123, Name = "name1", Description = "description1"},
                new Strategy() {Id = 2, CreatedBy = user123, Name = "name2", Description = "description2"},
                new Strategy() {Id = 3, CreatedBy = user124, Name = "name3", Description = "description3"}
            };
            dbcontext.Setup(p => p.Set<User>()).Returns(DbContextMock.GetQueryableMockDbSet<User>(new List<User>() { user123, user124 }));
            dbcontext.Setup(p => p.Set<Strategy>()).Returns(DbContextMock.GetQueryableMockDbSet<Strategy>(strategyList));

            // act
            var result = strategyService.GetStrategiesByStrategyId(1);

            // assert
            Assert.AreEqual("name1", result.Name, "Strategy 1 is returned with name1");
            Assert.AreEqual("description1", result.Description, "Strategy 1 is returned with description1");
        }

        [Test, Description("Test GetStrategiesByStrategyId: Null should be returned when strategy Id is not valid")]
        public void TestGetStrategiesByStrategyId_NullReturnedWhenInvalidStrategyId()
        {
            // arrange
            var user123 = new User() { Id = 123, Username = "testusername123" };
            var user124 = new User() { Id = 124, Username = "testusername124" };
            var strategyList = new List<Strategy>()
            {
                new Strategy() {Id = 1, CreatedBy = user123, Name = "name1", Description = "description1"},
                new Strategy() {Id = 2, CreatedBy = user123, Name = "name2", Description = "description2"},
                new Strategy() {Id = 3, CreatedBy = user124, Name = "name3", Description = "description3"}
            };
            dbcontext.Setup(p => p.Set<User>()).Returns(DbContextMock.GetQueryableMockDbSet<User>(new List<User>() { user123, user124 }));
            dbcontext.Setup(p => p.Set<Strategy>()).Returns(DbContextMock.GetQueryableMockDbSet<Strategy>(strategyList));

            // act
            var result = strategyService.GetStrategiesByStrategyId(10);

            // assert
            Assert.IsNull(result, "NULL result is returned as strategy id 10 does not exist in the database");
        }

        [Test, Description("Test UpdateEventGroupsInStrategy: The relationships are udpated successfully")]
        public void TestUpdateEventGroupsInStrategy_RelationshipUpdatedSuccessfully()
        {
            // arrange
            var user123 = new User() { Id = 123, Username = "testusername123" };
            var strategyList = new List<Strategy>()
            {
                new Strategy() {Id = 1, CreatedBy = user123, Name = "name1", Description = "description1", JoinStrategyEventGroups = new List<JoinStrategyEventGroup>()
                {
                    new JoinStrategyEventGroup() { StrategyId = 1, EventGroupId = 11 },
                    new JoinStrategyEventGroup() { StrategyId = 1, EventGroupId = 12 }
                }},
            };
            var eventGroups = new List<EventGroup>()
            {
                new EventGroup() {Id = 11, CreatedBy = user123, Name = "name11", Description = "description11"},
                new EventGroup() {Id = 12, CreatedBy = user123, Name = "name12", Description = "description12"},
                new EventGroup() {Id = 13, CreatedBy = user123, Name = "name13", Description = "description13"},
                new EventGroup() {Id = 14, CreatedBy = user123, Name = "name14", Description = "description14"}
            };
            dbcontext.Setup(p => p.Set<User>()).Returns(DbContextMock.GetQueryableMockDbSet<User>(new List<User>() { user123 }));
            dbcontext.Setup(p => p.Set<Strategy>()).Returns(DbContextMock.GetQueryableMockDbSet<Strategy>(strategyList));
            dbcontext.Setup(p => p.Set<EventGroup>()).Returns(DbContextMock.GetQueryableMockDbSet<EventGroup>(eventGroups));

            // act
            var newRelationship = new List<JoinStrategyEventGroup>()
            {
                new JoinStrategyEventGroup() {StrategyId = 1, EventGroupId = 11},
                new JoinStrategyEventGroup() {StrategyId = 1, EventGroupId = 13},
                new JoinStrategyEventGroup() {StrategyId = 1, EventGroupId = 14}
            };
            strategyService.UpdateEventGroupsInStrategy(1, newRelationship);

            // assert
            Assert.AreEqual(3, dbcontext.Object.Set<Strategy>().FirstOrDefault(x => x.Id == 1)?.JoinStrategyEventGroups.Count, 
                "Strategy should link to 3 event groups after the update");
            Assert.AreEqual(11, dbcontext.Object.Set<Strategy>().FirstOrDefault(x => x.Id == 1)?.JoinStrategyEventGroups.ToList()[0].EventGroup.Id,
                "Strategy should link to the event group 11 after the update");
            Assert.AreEqual(13, dbcontext.Object.Set<Strategy>().FirstOrDefault(x => x.Id == 1)?.JoinStrategyEventGroups.ToList()[1].EventGroup.Id,
                "Strategy should link to the event group 13 after the update");
            Assert.AreEqual(14, dbcontext.Object.Set<Strategy>().FirstOrDefault(x => x.Id == 1)?.JoinStrategyEventGroups.ToList()[2].EventGroup.Id,
                "Strategy should link to the event group 14 after the update");
        }

        [Test, Description("Test InsertIntoBackTestingResult: New back-testing result is saved to database successfully")]
        public void TestInsertIntoBackTestingResult_NewTestingResultSavedSuccessfully()
        {
            // arrange
            var user123 = new User() { Id = 123, Username = "testusername123" };
            var strategyList = new List<Strategy>()
            {
                new Strategy() {Id = 1, CreatedBy = user123, Name = "name1", Description = "description1", BackTestingResults = new List<BackTestingResult>()
                {
                    new BackTestingResult() { Id = 1, ReportFileUri = "reporturl1"}
                }},
            };

            dbcontext.Setup(p => p.Set<User>()).Returns(DbContextMock.GetQueryableMockDbSet<User>(new List<User>() { user123 }));
            dbcontext.Setup(p => p.Set<Strategy>()).Returns(DbContextMock.GetQueryableMockDbSet<Strategy>(strategyList));

            // act
            var result = strategyService.InsertIntoBackTestingResult(DateTime.Parse("2021-04-01"),
                DateTime.Parse("2020-03-01"), DateTime.Parse("2020-03-01"), "newreporturl2", 123, 1);

            // assert
            Assert.AreEqual(1, result);
            Assert.AreEqual(2, dbcontext.Object.Set<Strategy>().FirstOrDefault(x => x.Id == 1)?.BackTestingResults.Count, 
                "There should be 2 test results of strategy 1 after the update");
            Assert.AreEqual("reporturl1", dbcontext.Object.Set<Strategy>().FirstOrDefault(x => x.Id == 1)?.BackTestingResults.ToList()[0].ReportFileUri,
                "Strategy 1 should have test report reporturl1");
            Assert.AreEqual("newreporturl2", dbcontext.Object.Set<Strategy>().FirstOrDefault(x => x.Id == 1)?.BackTestingResults.ToList()[1].ReportFileUri,
                "Strategy 1 should have test report newreporturl2");
        }

        [Test, Description("Test InsertIntoBackTestingResult: Failed to insert new report when strategy Id is invalid")]
        public void TestInsertIntoBackTestingResult_FailedToAddNewReportDueToInvalidStrategyId()
        {
            // arrange
            var user123 = new User() { Id = 123, Username = "testusername123" };
            var strategyList = new List<Strategy>()
            {
                new Strategy() {Id = 1, CreatedBy = user123, Name = "name1", Description = "description1", BackTestingResults = new List<BackTestingResult>()
                {
                    new BackTestingResult() { Id = 1, ReportFileUri = "reporturl1"}
                }},
            };

            dbcontext.Setup(p => p.Set<User>()).Returns(DbContextMock.GetQueryableMockDbSet<User>(new List<User>() { user123 }));
            dbcontext.Setup(p => p.Set<Strategy>()).Returns(DbContextMock.GetQueryableMockDbSet<Strategy>(strategyList));

            // act
            Assert.Throws<InvalidOperationException>(() => 
                strategyService.InsertIntoBackTestingResult(DateTime.Parse("2021-04-01"), DateTime.Parse("2020-03-01"), DateTime.Parse("2020-03-01"), "newreporturl2", 123, 10), 
                "InvalidOperationException should be thrown when strategy Id is invalid");
        }

    }
}
