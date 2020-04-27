using SharedServices.DAL.UnitOfWork;
using System;

namespace SharedServices.BL.UseCases.Clients
{
    public partial class Client
    {
        private IUnitOfWork unitOfWork;

        public Client(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
    }
}