using Microsoft.AspNetCore.Identity;
using SharedServices.DAL;
using SharedServices.DAL.UnitOfWork;
using System;

namespace SharedServices.BL.UseCases.Clients
{
    public partial class Client
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public Client(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Client(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.userManager = userManager;
        }
    }
}