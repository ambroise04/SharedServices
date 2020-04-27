using SharedServices.BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedServices.BL.UseCases.Clients
{
    public partial class Client
    {
        public Request AddRequest(Request request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var addedRequest = unitOfWork.RequestRepository
                      .Insert(Mapping.Mapping.Mapper.Map<DAL.Entities.Request>(request));

            return Mapping.Mapping.Mapper.Map<Request>(addedRequest);
        }

        public Request GetRequestById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(nameof(id));
            }

            var result = unitOfWork.RequestRepository.GetById(id);

            return Mapping.Mapping.Mapper.Map<Request>(result);
        }

        public List<Request> GetRequestsByUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var requests = unitOfWork.RequestRepository
                                     .GetByPredicate(r => r.User.Id.Equals(userId))
                                     .OrderByDescending(r => r.DateOfRequest)
                                     .Select(r => Mapping.Mapping.Mapper.Map<Request>(r))
                                     .ToList();

            return requests;
        }

        public List<Request> GetAcceptedRequestsByUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var requests = unitOfWork.RequestRepository
                                     .GetByPredicate(r => r.User.Id.Equals(userId) && r.Accepted)
                                     .OrderByDescending(r => r.DateOfRequest)
                                     .Select(r => Mapping.Mapping.Mapper.Map<Request>(r))
                                     .ToList();

            return requests;
        }

        public List<Request> GetNotAcceptedRequestsByUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var requests = unitOfWork.RequestRepository
                                     .GetByPredicate(r => r.User.Id.Equals(userId) && !r.Accepted)
                                     .OrderByDescending(r => r.DateOfRequest)
                                     .Select(r => Mapping.Mapping.Mapper.Map<Request>(r))
                                     .ToList();

            return requests;
        }
    }
}