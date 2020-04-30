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
    public partial class ServiceTests
    {
        [TestMethod]
        public void GetByPredicateService_AddThreeServiceWithTwoEmitters_ThenRetrieveServiceForAspecificEmitter_ReturnTwoServices()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: MethodBase.GetCurrentMethod().Name)
                .Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            //ARRANGE
            var repositoryGroup = new ServiceGroupRepository(context);
            var group = repositoryGroup.Insert(new Entities.ServiceGroup { Title = "Service de Garderie", PointsByHour = 15, Services = new List<Service>() });
            context.SaveChanges();
            var service1 = new Service
            {
                Title = "Babysitting à domicile",
                Description = "Garde des enfants.",
                Group = group,
                UserServices = null,
            };
            var service2 = new Service
            {
                Title = "Babysitting à domicile",
                Description = "Garde des enfants.",
                Group = group,
                UserServices = null,
            };
            var service3 = new Service
            {
                Title = "Garde d'animaux",
                Description = "Garde des animaux de compagnies.",
                Group = group,
                UserServices = null,
            };
            var repository = new ServiceRepository(context);
            repository.Insert(service1);
            repository.Insert(service2);
            repository.Insert(service3);
            context.SaveChanges();
            //ACT
            var services = repository.GetByPredicate(d => d.Title.Equals("Babysitting à domicile"));
            //ASSERT
            Assert.IsNotNull(services);
            Assert.AreEqual(2, services.Count());
        }             
    }
}