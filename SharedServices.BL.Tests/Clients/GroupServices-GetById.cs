using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharedServices.BL.UseCases.Clients;
using SharedServices.DAL.UnitOfWork;
using System;
using System.Collections.Generic;

namespace SharedServices.BL.Tests.Clients
{
    public partial class ServiceGroupTests
    {
        [TestMethod]
        public void GetServiceGroupById_CorrectGroupIdProvided__ReturnGroupNotNull()
        {
            //ARRANGE
            var unitOfWork = new Mock<IUnitOfWork>();
            var group = new DAL.Entities.ServiceGroup { Id = 1, Title = "Garderie", PointsByHour = 15, Services = new List<DAL.Entities.Service>() };
            unitOfWork.Setup(u => u.ServiceGroupRepository.GetById(It.IsAny<int>())).Returns(group);

            var client = new Client(unitOfWork.Object);
            //ACT
            var retrievedGroup = client.GetGroupServiceById(1);
            //ASSERT
            unitOfWork.Verify(u => u.ServiceGroupRepository.GetById(It.IsAny<int>()), Times.Once);
            Assert.IsNotNull(retrievedGroup);
            Assert.AreEqual("Garderie", retrievedGroup.Title);
        }

        [TestMethod]
        public void GetServiceGroupById_BadGroupIdProvided__ThrowArgumentException()
        {
            //ARRANGE
            var unitOfWork = new Mock<IUnitOfWork>();
            var group = new DAL.Entities.ServiceGroup { Id = 1, Title = "Garderie", PointsByHour = 15, Services = new List<DAL.Entities.Service>() };
            unitOfWork.Setup(u => u.ServiceGroupRepository.GetById(It.IsAny<int>())).Returns(group);

            var client = new Client(unitOfWork.Object);
            //ACT
            //ASSERT
            Assert.ThrowsException<ArgumentException>(() => client.GetGroupServiceById(0));
            Assert.ThrowsException<ArgumentException>(() => client.GetGroupServiceById(-25));
            unitOfWork.Verify(u => u.ServiceGroupRepository.GetById(It.IsAny<int>()), Times.Never);
        }
    }
}