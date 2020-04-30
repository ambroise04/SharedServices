using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedServices.DAL.Entities;
using SharedServices.DAL.Repositories;
using System;
using System.Reflection;

namespace SharedServices.DAL.Tests.ServiceGroup
{
    public partial class DiscussionTests
    {
        [TestMethod]
        public void UpdateDiscussion_NullDiscussionProvided_ThrowArgumentNullException()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: MethodBase.GetCurrentMethod().Name)
                .Options;
            var context = new ApplicationContext(options);

            //ARRANGE
            var repository = new DiscussionRepository(context);
            //ACT
            //ASSERT
            Assert.ThrowsException<ArgumentNullException>(() => repository.Update(null));
        }

        [TestMethod]
        public void UpdateDiscussion_BadDiscussionIdProvided_ThrowArgumentException()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: MethodBase.GetCurrentMethod().Name)
                .Options;
            var context = new ApplicationContext(options);

            //ARRANGE
            var repository = new DiscussionRepository(context);
            var discussion = new Discussion
            {
                Emitter = "asdeeeeee2e4efffd2f1df",
                Receiver = "dsddeee24e5ed2edefef8gt0",
                DateHour = DateTime.Now,
                Message = "Bonjour, Monsieur."
            };
            var addedDiscussion = repository.Insert(discussion);
            context.SaveChanges();
            //ACT            
            addedDiscussion.Id = 0;
            //ASSERT
            Assert.ThrowsException<ArgumentException>(() => repository.Update(addedDiscussion));
        }

        [TestMethod]
        public void UpdateDiscussion_AddDiscussionThenUpdateTheAddedDiscussion_ReturnUpdatedDiscussion()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: MethodBase.GetCurrentMethod().Name)
                .Options;
            var context = new ApplicationContext(options);

            //ARRANGE
            var repository = new DiscussionRepository(context);
            var discussion = new Discussion
            {
                Emitter = "asdeeeeee2e4efffd2f1df",
                Receiver = "dsddeee24e5ed2edefef8gt0",
                DateHour = DateTime.Now,
                Message = "Bonjour, Monsieur."
            };
            var addedDiscussion = repository.Insert(discussion);
            context.SaveChanges();
            //ACT
            addedDiscussion.Message = "Je vous salue, Monsieur";
            var updatedDiscussion = repository.Update(addedDiscussion);
            //ASSERT
            Assert.IsNotNull(updatedDiscussion);
            Assert.AreEqual("Je vous salue, Monsieur", updatedDiscussion.Message);
        }
    }
}