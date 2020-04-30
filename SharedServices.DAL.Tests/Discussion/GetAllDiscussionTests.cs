using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedServices.DAL.Entities;
using SharedServices.DAL.Repositories;
using System;
using System.Linq;
using System.Reflection;

namespace SharedServices.DAL.Tests.ServiceGroup
{
    public partial class DiscussionTests
    {
        [TestMethod]
        public void GetAllDiscussion_AddThreeDiscussionThenRetrieveThem_ReturnThreeDiscussions()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: MethodBase.GetCurrentMethod().Name)
                .Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            //ARRANGE
            var discussion1 = new Discussion
            {
                Emitter = "asdeeeeee2e4efffd2f1df",
                Receiver = "dsddeee24e5ed2edefef8gt0",
                DateHour = DateTime.Now,
                Message = "Hi man!"
            };
            var discussion2 = new Discussion
            {
                Emitter = "asdeeeeee2e4efffd2f1df",
                Receiver = "dsddeee24e5ed2edefef8gt0",
                DateHour = DateTime.Now,
                Message = "How are you ?"
            };
            var discussion3 = new Discussion
            {
                Emitter = "bdd24e5ed2edefef8gt0",
                Receiver = "ccd0d14dddd25edrrrg5gr",
                DateHour = DateTime.Now,
                Message = "Bonjour, Monsieur."
            };
            var repository = new DiscussionRepository(context);
            repository.Insert(discussion1);
            repository.Insert(discussion2);
            repository.Insert(discussion3);
            context.SaveChanges();
            //ACT
            var discussions = repository.GetAll();
            //ASSERT
            Assert.IsNotNull(discussions);
            Assert.AreEqual(3, discussions.Count());
        }               
    }
}