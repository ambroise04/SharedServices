using Microsoft.EntityFrameworkCore;
using SharedServices.BL.Domain;
using SharedServices.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedServices.BL.UseCases.Clients
{
    public partial class Client
    {
        public RequestMulticast AddRequestMulticast(RequestMulticast request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var dalRequest = Mapping.Mapping.Mapper.Map<DAL.Entities.RequestMulticast>(request);
            dalRequest.Service = unitOfWork.ServiceRepository.GetById(request.Service.Id);
            var addedRequest = unitOfWork.RequestMulticastRepository
                      .Insert(dalRequest);

            return Mapping.Mapping.Mapper.Map<RequestMulticast>(addedRequest);
        }

        public RequestMulticast GetRequestMulticastById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(nameof(id));
            }

            var result = unitOfWork.RequestRepository.GetById(id);

            return Mapping.Mapping.Mapper.Map<RequestMulticast>(result);
        }

        public List<RequestMulticast> GetRequestMulticastsByUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var requests = unitOfWork.RequestRepository
                                     .GetByPredicate(r => r.Receiver.Id.Equals(userId))
                                     .OrderByDescending(r => r.DateOfRequest)
                                     .Select(r => Mapping.Mapping.Mapper.Map<RequestMulticast>(r))
                                     .ToList();

            return requests;
        }

        public List<RequestMulticast> GetAcceptedRequestMulticastsByUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var requests = unitOfWork.RequestRepository
                                     .GetByPredicate(r => r.Receiver.Id.Equals(userId) && r.Accepted)
                                     .OrderByDescending(r => r.DateOfRequest)
                                     .Select(r => Mapping.Mapping.Mapper.Map<RequestMulticast>(r))
                                     .ToList();

            return requests;
        }

        public List<RequestMulticast> GetNotAcceptedRequestMulticastsByUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var requests = unitOfWork.RequestRepository
                                     .GetByPredicate(r => r.Receiver.Id.Equals(userId) && !r.Accepted)
                                     .OrderByDescending(r => r.DateOfRequest)
                                     .Select(r => Mapping.Mapping.Mapper.Map<RequestMulticast>(r))
                                     .ToList();

            return requests;
        }

        public ApplicationUser UserRequestMulticasts(string userId)
        {
            var user = userManager.Users
                                  .Include(u => u.Responses)
                                  .ThenInclude(r => r.Responder)
                                  .ThenInclude(resp => resp.Picture)
                                  .Include(u => u.Responses)                                  
                                  .ThenInclude(r => r.RequestMulticast)
                                  .FirstOrDefault(u => u.Id.Equals(userId));
            return user;
        }
    }
}