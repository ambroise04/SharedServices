using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharedServices.BL.Domain;
using SharedServices.BL.UseCases.Clients;
using SharedServices.DAL.UnitOfWork;
using System;
using System.Collections.Generic;

namespace SharedServices.BL.Tests.Clients
{
    [TestClass]
    public partial class ServiceGroupTests
    {
        [TestMethod]
        public void AddServiceGroup_CorrectGroupProvided__ReturnAddedGroupNotNull()
        {
            //ARRANGE
            var unitOfWork = new Mock<IUnitOfWork>();
            var group = new DAL.Entities.ServiceGroup {Id = 1, Title = "Garderie", PointsByHour = 15, Services = new List<DAL.Entities.Service>() };
            unitOfWork.Setup(u => u.ServiceGroupRepository.Insert(It.IsAny<DAL.Entities.ServiceGroup>())).Returns(group);

            var groupToAdd = new ServiceGroup {Title = "Garderie", PointsByHour = 15, Services = new List<Service>() };
            var client = new Client(unitOfWork.Object);
            //ACT
            var addedGroup = client.AddGroupService(groupToAdd);
            //ASSERT
            unitOfWork.Verify(u => u.ServiceGroupRepository.Insert(It.IsAny<DAL.Entities.ServiceGroup>()), Times.Once);
            Assert.IsNotNull(addedGroup);
            Assert.AreEqual("Garderie", addedGroup.Title);
        }

        [TestMethod]
        public void AddServiceGroup_NullGroupProvided__ThrowArgumentNullException()
        {
            //ARRANGE
            var unitOfWork = new Mock<IUnitOfWork>();
            var group = new DAL.Entities.ServiceGroup { Id = 1, Title = "Garderie", PointsByHour = 15, Services = new List<DAL.Entities.Service>() };
            unitOfWork.Setup(u => u.ServiceGroupRepository.Insert(It.IsAny<DAL.Entities.ServiceGroup>())).Returns(group);
            var client = new Client(unitOfWork.Object);
            //ACT
            //ASSERT
            Assert.ThrowsException<ArgumentNullException>(() => client.AddGroupService(null));
            unitOfWork.Verify(u => u.ServiceGroupRepository.Insert(It.IsAny<DAL.Entities.ServiceGroup>()), Times.Never);
        }
    }
}