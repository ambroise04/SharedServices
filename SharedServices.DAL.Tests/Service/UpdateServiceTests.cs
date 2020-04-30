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
        public void UpdateService_NullServiceProvided_ThrowArgumentNullException()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: MethodBase.GetCurrentMethod().Name)
                .Options;
            var context = new ApplicationContext(options);

            //ARRANGE
            var repository = new ServiceRepository(context);
            //ACT
            //ASSERT
            Assert.ThrowsException<ArgumentNullException>(() => repository.Update(null));
        }

        [TestMethod]
        public void UpdateService_BadServiceIdProvided_ThrowArgumentException()
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
            addedService.Id = 0;
            //ASSERT
            Assert.ThrowsException<ArgumentException>(() => repository.Update(addedService));
        }

        [TestMethod]
        public void UpdateService_AddServiceThenUpdateTheAddedService_ReturnUpdatedService()
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
            var discussion = new Service
            {
                Title = "Babysitting",
                Description = "Garde des enfants.",
                Group = group,
                UserServices = null,
            };
            var addedService = repository.Insert(discussion);
            context.SaveChanges();
            //ACT
            addedService.Description = "Garde des enfants à domicile.";
            var updatedService = repository.Update(addedService);
            //ASSERT
            Assert.IsNotNull(updatedService);
            Assert.AreEqual("Garde des enfants à domicile.", updatedService.Description);
        }
    }
}