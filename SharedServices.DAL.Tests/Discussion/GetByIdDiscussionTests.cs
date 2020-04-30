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
        public void GetDiscussionById_BadDiscussionIdProvided_ThrowArgumentException()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: MethodBase.GetCurrentMethod().Name)
                .Options;
            var context = new ApplicationContext(options);

            //ARRANGE
            var repository = new DiscussionRepository(context);
            //ACT
            //ASSERT
            Assert.ThrowsException<ArgumentException>(() => repository.GetById(0));
            Assert.ThrowsException<ArgumentException>(() => repository.GetById(-1));
        }

        [TestMethod]
        public void GetDiscussion_ExistingDiscussionIdProvided_ReturnDiscussionNotNull()
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
            //ACT
            var gottenDiscussion = repository.GetById(addedDiscussion.Id);
            //ASSERT
            Assert.IsNotNull(gottenDiscussion);
            Assert.AreEqual(addedDiscussion.Id, gottenDiscussion.Id);
        }        
    }
}