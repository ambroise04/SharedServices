using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedServices.DAL.Entities;
using SharedServices.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SharedServices.DAL.Tests.ServiceGroup
{
    public partial class ServiceTests
    {
        [TestMethod]
        public void GetServiceById_BadServiceIdProvided_ThrowArgumentException()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: MethodBase.GetCurrentMethod().Name)
                .Options;
            var context = new ApplicationContext(options);

            //ARRANGE
            var repository = new ServiceRepository(context);
            //ACT
            //ASSERT
            Assert.ThrowsException<ArgumentException>(() => repository.GetById(0));
            Assert.ThrowsException<ArgumentException>(() => repository.GetById(-1));
        }

        [TestMethod]
        public void GetService_ExistingServiceIdProvided_ReturnServiceNotNull()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: MethodBase.GetCurrentMethod().Name)
                .Options;
            var context = new ApplicationContext(options);

            //ARRANGE
            var repository = new ServiceRepository(context);
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
            var addedService = repository.Insert(service);
            //ACT
            var gottenService = repository.GetById(addedService.Id);
            //ASSERT
            Assert.IsNotNull(gottenService);
            Assert.AreEqual(addedService.Id, gottenService.Id);
        }
    }
}