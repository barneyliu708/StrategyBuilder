using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using StrategyBuilder.Repository.Interfaces;

namespace StrategyBuilder.Service.Test
{
    public class EventServiceTest
    {
        private Mock<DbContext> dbcontext;
        private EventService eventService;
        
        [SetUp]
        public void Init()
        {
            dbcontext = new Mock<DbContext>();
            eventService = new EventService(dbcontext.Object);
        }

        [Test]
        public void TestUpdateEvents()
        {

        }
    }
}
