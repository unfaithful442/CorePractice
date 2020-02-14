using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using CorePractice.Controllers;
using CorePractice.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CorePracticeTestProject
{
    public class GroupControllerTest
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void GetReturnsGroups()
        {
            var mockCache = new Mock<IMemoryCache>();

            var mockMapper = new Mock<IMapper>();

            var options = new DbContextOptionsBuilder<CorePracticeDbContext>()
            .UseInMemoryDatabase(databaseName: "CorePracticeDb")
            .Options;

            //create in memory db context for test
            using (var context = new CorePracticeDbContext(options))
            {
                context.Groups.Add(new Group() { GroupId = 1, GroupName = "Dentist", Description = "Group 1" });

                context.Groups.Add(new Group() { GroupId = 2, GroupName = "Patient", Description = "Group 2" });

                GroupController controller = new GroupController(mockCache.Object, context, mockMapper.Object);

                IActionResult actionResult = controller.GetGroups();

                Assert.That(actionResult, Is.InstanceOf<OkObjectResult>());
            }

        }

    }
}
