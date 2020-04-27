using SharedServices.BL.UseCases.Clients;
using SharedServices.DAL.UnitOfWork;

namespace SharedServices.BL.UseCases.Admin
{
    public partial class Adminitrator : Client
    {
        private IUnitOfWork unitOfWork;
        public Adminitrator(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
    }
}