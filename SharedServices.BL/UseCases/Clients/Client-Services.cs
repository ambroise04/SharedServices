using SharedServices.BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedServices.BL.UseCases.Clients
{
    public partial class Client
    {
        public Service AddService(Service service)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            var addedService = unitOfWork.ServiceRepository
                      .Insert(Mapping.Mapping.Mapper.Map<DAL.Entities.Service>(service));

            return Mapping.Mapping.Mapper.Map<Service>(addedService);
        }

        public List<Service> GetAllServices()
        {
            var services = unitOfWork.ServiceRepository
                      .GetAll()
                      .Select(s => Mapping.Mapping.Mapper.Map<Service>(s))
                      .ToList();

            return services;
        }

        public List<Service> GetServicesByGroup(int groupId)
        {
            var services = unitOfWork.ServiceRepository
                      .GetByPredicate(s => s.Group.Id == groupId)
                      .Select(s => Mapping.Mapping.Mapper.Map<Service>(s))
                      .ToList();

            return services;
        }
    }
}