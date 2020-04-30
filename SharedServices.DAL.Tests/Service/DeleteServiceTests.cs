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
        public void DeleteService_BadServiceIdProvided_ThrowArgumentException()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: MethodBase.GetCurrentMethod().Name)
                .Options;
            var context = new ApplicationContext(options);

            //ARRANGE
            var repository = new ServiceRepository(context);
            //ACT
            //ASSERT
            Assert.ThrowsException<ArgumentException>(() => repository.Delete(0));
            Assert.ThrowsException<ArgumentException>(() => repository.Delete(-1));
        }

        [TestMethod]
        public void DeleteService_AddServiceThenRomoveTheAddedService_ReturnTrue()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: MethodBase.GetCurrentMethod().Name)
                .Options;
            var context = new ApplicationContext(options);

            //ARRANGE
            var repository = new ServiceRepository(context);
            var repositoryGroup = new ServiceGroupRepository(context);
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
            context.SaveChanges();
            //ACT
            var deletionResult = repository.Delete(addedService.Id);
            //ASSERT
            Assert.IsTrue(deletionResult);
        }
    }
}