using Microsoft.EntityFrameworkCore;
using SharedServices.BL.Domain;
using SharedServices.DAL;
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
            var dalRequest = Mapping.Mapping.Mapper.Map<DAL.Entities.Request>(request);
            dalRequest.Service = unitOfWork.ServiceRepository.GetById(request.Service.Id);
            var addedRequest = unitOfWork.RequestRepository
                      .Insert(dalRequest);

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
                                     .GetByPredicate(r => r.Receiver.Id.Equals(userId))
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
                                     .GetByPredicate(r => r.Receiver.Id.Equals(userId) && r.Accepted)
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
                                     .GetByPredicate(r => r.Receiver.Id.Equals(userId) && !r.Accepted)
                                     .OrderByDescending(r => r.DateOfRequest)
                                     .Select(r => Mapping.Mapping.Mapper.Map<Request>(r))
                                     .ToList();

            return requests;
        }

        public ApplicationUser UserRequests(string userId)
        {
            var user = userManager.Users
                                    //Requests received
                                  .Include(u => u.RequestsReceived)
                                  .ThenInclude(rs => rs.Requester)
                                  .ThenInclude(req => req.Picture)
                                  //Requests received service
                                  .Include(u => u.RequestsReceived)
                                  .ThenInclude(rs => rs.Service)
                                  .ThenInclude(serv => serv.Group)
                                  //Requests sent
                                  .Include(u => u.RequestsSent)
                                  .ThenInclude(rs => rs.Receiver)
                                  .ThenInclude(rec => rec.Picture)
                                  //Requests sent service
                                  .Include(u => u.RequestsSent)
                                  .ThenInclude(rs => rs.Service)
                                  .ThenInclude(serv => serv.Group)

                                  .FirstOrDefault(u => u.Id.Equals(userId));
            return user;
        }
    }
}