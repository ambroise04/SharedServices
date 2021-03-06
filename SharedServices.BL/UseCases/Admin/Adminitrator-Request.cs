﻿using SharedServices.BL.Domain;
using SharedServices.BL.UseCases.Clients;
using SharedServices.Mutual.Enumerations;
using System.Collections.Generic;
using System.Linq;

namespace SharedServices.BL.UseCases.Admin
{
    public partial class Adminitrator : Client
    {
        public List<Request> GetAllNotAcceptedRequests()
        {
            var requests = unitOfWork.RequestRepository
                                     .GetByPredicate(r => r.State == RequestStates.Waiting)
                                     .OrderByDescending(r => r.DateOfRequest)
                                     .Select(r => Mapping.Mapping.Mapper.Map<Request>(r))
                                     .ToList();

            return requests;
        }
    }
}