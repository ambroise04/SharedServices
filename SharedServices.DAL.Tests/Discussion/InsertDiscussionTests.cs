using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedServices.DAL.Entities;
using SharedServices.DAL.Repositories;
using System;
using System.Reflection;

namespace SharedServices.DAL.Tests.ServiceGroup
{
    [TestClass]
    public partial class DiscussionTests
    {
        [TestMethod]
        public void InsertDiscussion_NullDiscussionProvided_ThrowArgumentNullException()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: MethodBase.GetCurrentMethod().Name)
                .Options;
            var context = new ApplicationContext(options);
            var repository = new DiscussionRepository(context);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            //ARRANGE
            //ACT
            //ASSERT
            Assert.ThrowsException<ArgumentNullException>(() => repository.Insert(null));
        }

        [TestMethod]
        public void InsertDiscussion_DiscussionProvidedWithId_ThrowArgumentException()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: MethodBase.GetCurrentMethod().Name)
                .Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var repository = new DiscussionRepository(context);

            //ARRANGE
            //ACT
            var discussion = new Discussion
            { 
                Id = 1, 
                Emitter = "asdeeeeee2e4efffd2f1df", 
                Receiver = "dsddeee24e5ed2edefef8gt0", 
                DateHour = DateTime.Now, 
                Message = "Bonjour, Monsieur." 
            };
            //ASSERT
            Assert.ThrowsException<ArgumentException>(() => repository.Insert(discussion));
        }

        [TestMethod]
        public void InsertDiscussion_CorrectDiscussionProvided_ReturnAddedDiscussionNotNull()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: MethodBase.GetCurrentMethod().Name)
                .Options;
            var context = new ApplicationContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var repository = new DiscussionRepository(context);

            //ARRANGE
            var discussion = new Discussion
            {
                Emitter = "asdeeeeee2e4efffd2f1df",
                Receiver = "dsddeee24e5ed2edefef8gt0",
                DateHour = DateTime.Now,
                Message = "Hi, Monsieur."
            };
            //ACT
            var addedDiscussion = repository.Insert(discussion);
            context.SaveChanges();
            //ASSERT
            Assert.IsNotNull(addedDiscussion);
            Assert.AreEqual("asdeeeeee2e4efffd2f1df", addedDiscussion.Emitter);
            Assert.AreEqual("Hi, Monsieur.", addedDiscussion.Message);
        }
    }
}