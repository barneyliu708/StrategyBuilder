using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using StrategyBuilder.Repository.Entities;
using StrategyBuilder.Repository.Interfaces;

namespace StrategyBuilder.Service.Test
{
    public class EventServiceTests
    {
        private Mock<DbContext> dbcontext;
        private EventService eventService;
        
        [SetUp]
        public void Init()
        {
            dbcontext = new Mock<DbContext>();
            dbcontext.Setup(p => p.SaveChanges()).Returns(1);
            eventService = new EventService(dbcontext.Object);
        }

        [Test, Description("Test GetAllEventGroupsByUserId: All event groups are returned by user id successfully from the database")]
        public void TestGetAllEventGroupsByUserId_GetAllDataSuccessfully()
        {
            // arrange
            var user123 = new User() { Id = 123, Username = "testusername123"};
            var user124 = new User() { Id = 124, Username = "testusername124" };
            var eventGroups = new List<EventGroup>()
            {
                new EventGroup() {Id = 1, CreatedBy = user123, Name = "name1", Description = "description1"},
                new EventGroup() {Id = 2, CreatedBy = user123, Name = "name2", Description = "description2"},
                new EventGroup() {Id = 3, CreatedBy = user124, Name = "name3", Description = "description3"}
            };
            dbcontext.Setup(p => p.Set<EventGroup>()).Returns(DbContextMock.GetQueryableMockDbSet<EventGroup>(eventGroups));

            // act
            var result123 = eventService.GetAllEventGroupsByUserId(123).ToList();
            var result124 = eventService.GetAllEventGroupsByUserId(124).ToList();

            // assert
            Assert.AreEqual(2, result123.Count, "User 123 should have 2 event groups");
            Assert.AreEqual(1, result124.Count, "User 124 should have 1 event groups");
        }

        [Test, Description("Test GetAllEventGroupsByUserId: User does not have any event groups created")]
        public void TestGetAllEventGroupsByUserId_EmtpyDataReturnWhenUserDoesNotHaveAnyEventGroups()
        {
            // arrange
            var user123 = new User() { Id = 123, Username = "testusername123" };
            var user124 = new User() { Id = 124, Username = "testusername124" };
            var eventGroups = new List<EventGroup>()
            {
                new EventGroup() {Id = 1, CreatedBy = user123, Name = "name1", Description = "description1"},
                new EventGroup() {Id = 2, CreatedBy = user123, Name = "name2", Description = "description2"},
                new EventGroup() {Id = 3, CreatedBy = user124, Name = "name3", Description = "description3"}
            };
            dbcontext.Setup(p => p.Set<EventGroup>()).Returns(DbContextMock.GetQueryableMockDbSet<EventGroup>(eventGroups));

            // act
            var result215 = eventService.GetAllEventGroupsByUserId(215).ToList();

            // assert
            Assert.AreEqual(0, result215.Count, "User 215 does not have any event group created");
        }

        [Test, Description("Test AddEventGroup: Add new event group to user 123 sccessfully")]
        public void TestAddEventGroup_AddNewEventGroupSuccessfully()
        {
            // arrange
            var user123 = new User() { Id = 123, Username = "testusername123" };
            var user124 = new User() { Id = 124, Username = "testusername124" };
            var eventGroups = new List<EventGroup>()
            {
                new EventGroup() {Id = 1, CreatedBy = user123, Name = "name1", Description = "description1"},
                new EventGroup() {Id = 2, CreatedBy = user123, Name = "name2", Description = "description2"},
                new EventGroup() {Id = 3, CreatedBy = user124, Name = "name3", Description = "description3"}
            };
            dbcontext.Setup(p => p.Set<User>()).Returns(DbContextMock.GetQueryableMockDbSet<User>(new List<User>() {user123, user124}));
            dbcontext.Setup(p => p.Set<EventGroup>()).Returns(DbContextMock.GetQueryableMockDbSet<EventGroup>(eventGroups));

            // act
            var newEventGroup = new EventGroup() { Id = 4, CreatedBy = user123, Name = "new name", Description = "new description"};
            eventService.AddEventGroup(newEventGroup);

            // assert
            Assert.AreEqual(3, dbcontext.Object.Set<EventGroup>().Where(x => x.CreatedBy.Id == 123).ToList().Count, 
                "User 123 should have 3 event groups after the update");
            Assert.AreEqual(1, dbcontext.Object.Set<EventGroup>().Where(x => x.CreatedBy.Id == 124).ToList().Count,
                "User 124 should have 1 event groups after the update");
        }

        [Test, Description("Test AddEventGroup: The new event group failed be added when the user does not exist")]
        public void TestAddEventGroup_FaildToAddNewEventGroupWhenUserIsNotValid()
        {
            // arrange
            var user123 = new User() { Id = 123, Username = "testusername123" };
            var user124 = new User() { Id = 124, Username = "testusername124" };
            var eventGroups = new List<EventGroup>()
            {
                new EventGroup() {Id = 1, CreatedBy = user123, Name = "name1", Description = "description1"},
                new EventGroup() {Id = 2, CreatedBy = user123, Name = "name2", Description = "description2"},
                new EventGroup() {Id = 3, CreatedBy = user124, Name = "name3", Description = "description3"}
            };
            dbcontext.Setup(p => p.Set<User>()).Returns(DbContextMock.GetQueryableMockDbSet<User>(new List<User>() { user123, user124 }));
            dbcontext.Setup(p => p.Set<EventGroup>()).Returns(DbContextMock.GetQueryableMockDbSet<EventGroup>(eventGroups));

            // act + assert
            var newEventGroup = new EventGroup() { Id = 4, CreatedBy = new User() { Id = 215, Username = "new user"}, Name = "new name", Description = "new description" };
            Assert.Throws<InvalidOperationException>(() => eventService.AddEventGroup(newEventGroup), "InvalidOperationException should be thrown when the user does not exist.");
        }
        
        [Test, Description("Test UpdateEventGroup: The event group is updated successfully")]
        public void TestUpdateEventGroup_UpdateTheEventGroupSuccessfully()
        {
            // arrange
            var user123 = new User() { Id = 123, Username = "testusername123" };
            var user124 = new User() { Id = 124, Username = "testusername124" };
            var eventGroups = new List<EventGroup>()
            {
                new EventGroup() {Id = 1, CreatedBy = user123, Name = "name1", Description = "description1", Expression = "expression1" },
                new EventGroup() {Id = 2, CreatedBy = user123, Name = "name2", Description = "description2", Expression = "expression2" },
                new EventGroup() {Id = 3, CreatedBy = user124, Name = "name3", Description = "description3", Expression = "expression3" }
            };
            dbcontext.Setup(p => p.Set<User>()).Returns(DbContextMock.GetQueryableMockDbSet<User>(new List<User>() { user123, user124 }));
            dbcontext.Setup(p => p.Set<EventGroup>()).Returns(DbContextMock.GetQueryableMockDbSet<EventGroup>(eventGroups));

            // act
            var updatedEventGroup = new EventGroup() { Id = 1, CreatedBy = user123, Name = "updated name1", Description = "updated description1", Expression = "updated expression1" };
            eventService.UpdateEventGroup(1, updatedEventGroup);

            // assert
            Assert.AreEqual("updated name1", dbcontext.Object.Set<EventGroup>().FirstOrDefault(x => x.Id == 1)?.Name, "The name of event group 1 should be updated");
            Assert.AreEqual("updated description1", dbcontext.Object.Set<EventGroup>().FirstOrDefault(x => x.Id == 1)?.Description, "The description of event group 1 should be updated");
            Assert.AreEqual("updated expression1", dbcontext.Object.Set<EventGroup>().FirstOrDefault(x => x.Id == 1)?.Expression, "The expression of event group 1 should be updated");
        }

        [Test, Description("Test UpdateEventGroup: The event group failed to be updated due to invalid event group id")]
        public void TestUpdateEventGroup_FailedToUpdateEventGroupDueToInvalidID()
        {
            // arrange
            var user123 = new User() { Id = 123, Username = "testusername123" };
            var user124 = new User() { Id = 124, Username = "testusername124" };
            var eventGroups = new List<EventGroup>()
            {
                new EventGroup() {Id = 1, CreatedBy = user123, Name = "name1", Description = "description1", Expression = "expression1" },
                new EventGroup() {Id = 2, CreatedBy = user123, Name = "name2", Description = "description2", Expression = "expression2" },
                new EventGroup() {Id = 3, CreatedBy = user124, Name = "name3", Description = "description3", Expression = "expression3" }
            };
            dbcontext.Setup(p => p.Set<User>()).Returns(DbContextMock.GetQueryableMockDbSet<User>(new List<User>() { user123, user124 }));
            dbcontext.Setup(p => p.Set<EventGroup>()).Returns(DbContextMock.GetQueryableMockDbSet<EventGroup>(eventGroups));

            // act + assert
            var updatedEventGroup = new EventGroup() { Id = 10, CreatedBy = user123, Name = "updated name1", Description = "updated description1", Expression = "updated expression1" };
            Assert.Throws<InvalidOperationException>(() => eventService.UpdateEventGroup(10, updatedEventGroup),
                "InvalidOperationException should be thrown when invalid group id is provided");
        }

        [Test, Description("Test UpdateEvents: The list of events is updated in the event group")]
        public void TestUpdateEvents_UpdateEventsSuccessfully()
        {
            // arrange
            var user123 = new User() { Id = 123, Username = "testusername123" };
            var eventGroups = new List<EventGroup>()
            {
                new EventGroup() {Id = 1, CreatedBy = user123, Name = "name1", Description = "description1", Expression = "expression1", Events = new List<Event>()
                {
                    new Event() { Id = 1001, Occurrence = DateTime.Parse("2020-03-01")},
                    new Event() { Id = 1001, Occurrence = DateTime.Parse("2020-04-01")},
                    new Event() { Id = 1001, Occurrence = DateTime.Parse("2020-05-01")}
                }}
            };
            dbcontext.Setup(p => p.Set<User>()).Returns(DbContextMock.GetQueryableMockDbSet<User>(new List<User>() { user123 }));
            dbcontext.Setup(p => p.Set<EventGroup>()).Returns(DbContextMock.GetQueryableMockDbSet<EventGroup>(eventGroups));

            // act
            var newEvents = new List<DateTime>()
            {
                DateTime.Parse("2020-06-01"),
                DateTime.Parse("2020-07-01")
            };
            eventService.UpdateEvents(1, newEvents);

            // assert
            Assert.AreEqual(2, dbcontext.Object.Set<EventGroup>().FirstOrDefault(x => x.Id == 1)?.Events.Count,
                "There should be two events after the update");
            Assert.AreEqual(DateTime.Parse("2020-06-01"), dbcontext.Object.Set<EventGroup>().FirstOrDefault(x => x.Id == 1)?.Events.ToList()[0].Occurrence, 
                "The first event should be updated to 2020-06-01");
            Assert.AreEqual(DateTime.Parse("2020-07-01"), dbcontext.Object.Set<EventGroup>().FirstOrDefault(x => x.Id == 1)?.Events.ToList()[1].Occurrence,
                "The second event should be updated to 2020-07-01");
        }

        [Test, Description("Test UpdateEvents: Failed to update the events of group due to invalid event group id")]
        public void TestUpdateEvents_UpdateEventsFailedDueToInvalidEventGroupID()
        {
            // arrange
            var user123 = new User() { Id = 123, Username = "testusername123" };
            var eventGroups = new List<EventGroup>()
            {
                new EventGroup() {Id = 1, CreatedBy = user123, Name = "name1", Description = "description1", Expression = "expression1", Events = new List<Event>()
                {
                    new Event() { Id = 1001, Occurrence = DateTime.Parse("2020-03-01")},
                    new Event() { Id = 1001, Occurrence = DateTime.Parse("2020-04-01")},
                    new Event() { Id = 1001, Occurrence = DateTime.Parse("2020-05-01")}
                }}
            };
            dbcontext.Setup(p => p.Set<User>()).Returns(DbContextMock.GetQueryableMockDbSet<User>(new List<User>() { user123 }));
            dbcontext.Setup(p => p.Set<EventGroup>()).Returns(DbContextMock.GetQueryableMockDbSet<EventGroup>(eventGroups));

            // act + assert
            var newEvents = new List<DateTime>()
            {
                DateTime.Parse("2020-06-01"),
                DateTime.Parse("2020-07-01")
            };
            Assert.Throws<InvalidOperationException>(() => eventService.UpdateEvents(10, newEvents),
                "InvalidOperationException should be thrown due to invalid eventgroup Id");
        }
    }
}
