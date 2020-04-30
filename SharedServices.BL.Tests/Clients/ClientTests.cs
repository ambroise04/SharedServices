using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SharedServices.DAL;
using SharedServices.DAL.UnitOfWork;

namespace SharedServices.BL.Tests.Clients
{
    [TestClass]
    public partial class ClientTests
    {
        [TestMethod]
        public void UserServices_AddUserWitSomeServices_RetrieveServices_REturnCorrectCountServices()
        {
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var passwordHasher = new Mock<IPasswordHasher<ApplicationUser>>();
            var userManager = new UserManager<ApplicationUser>(userStore.Object, options.Object, passwordHasher.Object, null, null, null, null, null, null);
        }
    }
}