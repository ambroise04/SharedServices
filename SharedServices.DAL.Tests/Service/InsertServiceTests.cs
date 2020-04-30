using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedServices.DAL.Entities;
using SharedServices.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SharedServices.DAL.Tests.ServiceGroup
{
    [TestClass]
    public partial class ServiceTests
    {
        [TestMethod]
        public void InsertService_NullServiceProvided_ThrowArgumentNullException()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: MethodBase.GetCurrentMethod().Name)
                .Options;
            var context = new ApplicationContext(options);
            var repository = new ServiceRepository(context);

            //ARRANGE
            //ACT
            //ASSERT
            Assert.ThrowsException<ArgumentNullException>(() => repository.Insert(null));
        }

        [TestMethod]
        public void InsertService_ServiceProvidedWithId_ThrowArgumentException()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: MethodBase.GetCurrentMethod().Name)
                .Options;
            var context = new ApplicationContext(options);
            var repository = new ServiceRepository(context);

            //ARRANGE
            //ACT
            var service = new Service
            {
                Id = 1,
                Title = "Babysitting",
                Description = "Garde des enfants.",
                Group = null,
                UserServices = null,
            };
            //ASSERT
            Assert.ThrowsException<ArgumentException>(() => repository.Insert(service));
        }

        [TestMethod]
        public void InsertService_CorrectServiceProvidedWithNullGroup_ThrowArgumentNullException()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: MethodBase.GetCurrentMethod().Name)
                .Options;
            var context = new ApplicationContext(options);
            var repository = new ServiceRepository(context);

            //ARRANGE
            var service = new Service
            {
                Title = "Babysitting",
                Description = "Garde des enfants.",
                Group = null,
                UserServices = null,
            };
            //ACT
            //ASSERT
            Assert.ThrowsException<ArgumentNullException>(() => repository.Insert(service));
        }

        [TestMethod]
        public void InsertService_CorrectServiceProvided_ReturnAddedServiceNotNull()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: MethodBase.GetCurrentMethod().Name)
                .Options;
            var context = new ApplicationContext(options);
            var repositoryService = new ServiceRepository(context);
            var repositoryGroup = new ServiceGroupRepository(context);

            //ARRANGE
            var group = repositoryGroup.Insert(new Entities.ServiceGroup { Title = "Service de Garderie", PointsByHour = 15, Services = new List<Service>() });
            context.SaveChanges();
            var service = new Service
            {
                Title = "Babysitting",
                Description = "Garde des enfants.",
                Group = group,
                UserServices = null,
            };
            //ACT
            var addedService = repositoryService.Insert(service);
            context.SaveChanges();
            var groups = repositoryGroup.GetAll();
            //ASSERT
            Assert.IsNotNull(addedService);
            Assert.AreNotEqual(0, groups.First().Services.Count());
            Assert.AreEqual("Babysitting", addedService.Title);
        }
    }
}