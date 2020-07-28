using SharedServices.BL.Domain;
using SharedServices.Mutual.Enumerations;
using System;
using System.Threading.Tasks;

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

        public bool AcceptRequest(Request request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var dalRequest = Mapping.Mapping.Mapper.Map<DAL.Entities.Request>(unitOfWork.RequestRepository.GetById(request.Id));
            dalRequest.State = RequestStates.Accepted;
            var result = unitOfWork.RequestRepository
                      .Update(dalRequest);

            return !(result is null);
        }

        public bool ValidateRequest(Request request, int direction = 0)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var dalRequest = Mapping.Mapping.Mapper.Map<DAL.Entities.Request>(unitOfWork.RequestRepository.GetById(request.Id));
            switch (direction)
            {
                case 0:
                    dalRequest.RequesterValidation = true;
                    break;
                default:
                    dalRequest.ReceiverValidation = true;
                    break;
            }
            var result = unitOfWork.RequestRepository
                      .Update(dalRequest);
            if (result.RequesterValidation && result.ReceiverValidation)
            {
                result.State = RequestStates.Closed;
                result = unitOfWork.RequestRepository
                      .Update(result);

                PointTransfer(Mapping.Mapping.Mapper.Map<Request>(result));
            }
            return !(result is null);
        }

        public bool RejectRequest(Request request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var dalRequest = Mapping.Mapping.Mapper.Map<DAL.Entities.Request>(unitOfWork.RequestRepository.GetById(request.Id));
            dalRequest.State = RequestStates.Rejected;
            var result = unitOfWork.RequestRepository
                      .Update(dalRequest);

            return !(result is null);
        }

        public async void PointTransfer(Request request)
        {
            var workMaker = await userManager.FindByIdAsync(request.Receiver.Id);
            var serviceGroup = GetServiceGroup(request.Service.Id);
            workMaker.Point += serviceGroup.PointsByHour;
        }
    }
}