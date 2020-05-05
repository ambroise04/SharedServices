using SharedServices.BL.UseCases.Admin;
using SharedServices.DAL.UnitOfWork;
using System;

namespace SharedServices.BL.Services
{
    public class GlobalInfo : IGlobalInfo
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly Adminitrator admin;

        public GlobalInfo(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            admin = new Adminitrator(unitOfWork);
        }

        public Mutual.GlobalInfo GetGlobalInfo()
        {
            return admin.GetInfo();
        }
    }
}