using Geolocation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedServices.BL.Domain;
using SharedServices.BL.Services;
using SharedServices.DAL;
using SharedServices.Mutual.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

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

            var result = unitOfWork.RequestMulticastRepository.GetById(id);

            return Mapping.Mapping.Mapper.Map<RequestMulticast>(result);
        }

        public List<RequestMulticast> GetRequestMulticastsByUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var requests = unitOfWork.RequestMulticastRepository
                                     .GetByPredicate(r => r.RequesterMulticast.Id.Equals(userId))
                                     .OrderByDescending(r => r.DateOfRequest)
                                     .Select(r => Mapping.Mapping.Mapper.Map<RequestMulticast>(r))
                                     .ToList();

            return requests;
        }

        public List<RequestMulticast> GetAcceptedRequestMulticasts()
        {
            var requests = unitOfWork.RequestMulticastRepository
                                     .GetByPredicate(r => r.Accepted)
                                     .OrderByDescending(r => r.DateOfRequest)
                                     .Select(r => Mapping.Mapping.Mapper.Map<RequestMulticast>(r))
                                     .ToList();

            return requests;
        }

        public List<RequestMulticast> GetNotAcceptedRequestMulticasts(ApplicationUser user, SearchOptions search, Coordinate coordinate)
        {
            var requests = new List<RequestMulticast>();

            try
            {
                switch (search)
                {
                    case SearchOptions.MyServices:
                        List<int> userSericesId = user.UserServices.Select(us => us.ServiceId).Distinct().ToList();
                        requests = unitOfWork.RequestMulticastRepository
                                         .GetByPredicate(r => !r.RequesterMulticast.Id.Equals(user.Id)
                                                        && !r.Accepted
                                                        && userSericesId.Contains(r.Service.Id))
                                         .OrderByDescending(r => r.DateOfRequest)
                                         .Select(r => Mapping.Mapping.Mapper.Map<RequestMulticast>(r))
                                         .ToList();
                        break;
                    case SearchOptions.NearMe:
                        requests = unitOfWork.RequestMulticastRepository
                                         .GetByPredicate(r => !r.RequesterMulticast.Id.Equals(user.Id)
                                                        && !r.Accepted)
                                         .OrderBy(r => Geocoding.DistanceCalculator(coordinate, new Coordinate { Latitude = r.Place.Latitude, Longitude = r.Place.Longitude }))
                                         .ThenByDescending(r => r.DateOfRequest)
                                         .Select(r => Mapping.Mapping.Mapper.Map<RequestMulticast>(r))
                                         .ToList();
                        break;
                    default:
                        requests = unitOfWork.RequestMulticastRepository
                                         .GetByPredicate(r => !r.RequesterMulticast.Id.Equals(user.Id) && !r.Accepted)
                                         .OrderByDescending(r => r.DateOfRequest)
                                         .Select(r => Mapping.Mapping.Mapper.Map<RequestMulticast>(r))
                                         .ToList();
                        break;
                }

                return requests;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<RequestMulticast> GetNotAcceptedRequestMulticasts()
        {
            var requests = new List<RequestMulticast>();

            try
            {
                requests = unitOfWork.RequestMulticastRepository
                                 .GetByPredicate(r => !r.Accepted)
                                 .OrderByDescending(r => r.DateOfRequest)
                                 .Select(r => Mapping.Mapping.Mapper.Map<RequestMulticast>(r))
                                 .ToList();
                return requests;
            }
            catch (Exception)
            {
                throw;
            }
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

        public RequestMulticast UpdateRequestMulticast(RequestMulticast request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var dalRequest = Mapping.Mapping.Mapper.Map<DAL.Entities.RequestMulticast>(request);
            dalRequest.Service = unitOfWork.ServiceRepository.GetById(request.Service.Id);
            var addedRequest = unitOfWork.RequestMulticastRepository
                                         .Update(dalRequest);

            return Mapping.Mapping.Mapper.Map<RequestMulticast>(addedRequest);
        }

        public RequestMulticast UpdateRequestMulticastForChoice(RequestMulticast request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var dalRequest = unitOfWork.RequestMulticastRepository.GetByIdWithTracking(request.Id);
            dalRequest.Service = unitOfWork.ServiceRepository.GetById(request.Service.Id);
            dalRequest.Accepted = true;
            var addedRequest = unitOfWork.RequestMulticastRepository
                                         .Update(dalRequest);

            return Mapping.Mapping.Mapper.Map<RequestMulticast>(addedRequest);
        }
    }
}