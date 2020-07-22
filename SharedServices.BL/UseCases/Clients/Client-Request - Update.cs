using SharedServices.BL.Domain;
using System;

namespace SharedServices.BL.UseCases.Clients
{
    public partial class Client
    {
        public bool CancelRequest(Request request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var result = unitOfWork.RequestRepository
                      .Delete(request.Id);

            return result;
        }
    }
}