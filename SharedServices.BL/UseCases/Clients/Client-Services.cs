using SharedServices.BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        public Service GetServiceById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException($"Service id is wrong. {nameof(id)}");
            }

            var retrievedService = unitOfWork.ServiceRepository
                      .GetById(id);

            return Mapping.Mapping.Mapper.Map<Service>(retrievedService);
        }

        public int GetServiceByTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException($"Bad service title. {nameof(title)}");
            }

            var retrievedServiceId = unitOfWork.ServiceRepository
                      .GetByPredicate(s => s.Title.ToLower().Equals(title.Trim().ToLower()))
                      .Select(s => s.Id)
                      .FirstOrDefault();

            return retrievedServiceId;
        }

        public DAL.Entities.Service GetServiceByIdWithoutConverting(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException($"Service id is wrong. {nameof(id)}");
            }

            var retrievedService = unitOfWork.ServiceRepository
                      .GetById(id);

            return retrievedService;
        }

        public List<Service> GetAllServices()
        {
            var services = unitOfWork.ServiceRepository
                      .GetAll()
                      .Select(s => Mapping.Mapping.Mapper.Map<Service>(s))
                      .ToList();

            return services;
        }

        public List<ServiceTO> GetAllServicesGrouped()
        {
            var services = unitOfWork.ServiceRepository
                      .GetAll()
                      .GroupBy(s => s.Group, s => s, (key, serv) =>
                            new { Group = key, Services = serv.ToList() })
                      .OrderBy(s => s.Group.Title)
                      .Select(s => new ServiceTO
                      {
                          ServiceGroup = Mapping.Mapping.Mapper.Map<ServiceGroup>(s.Group),
                          Services = s.Services.Select(service => Mapping.Mapping.Mapper.Map<Service>(service))
                                               .OrderBy(service => service.Title)
                                               .ToList()
                      }).ToList();

            return services;
        }

        public List<ServiceGroup> GetServiceCategories()
        {
            var categories = unitOfWork.ServiceGroupRepository
                      .GetAll()
                      .OrderBy(c => c.Title)
                      .Select(s => Mapping.Mapping.Mapper.Map<ServiceGroup>(s))
                      .ToList();

            return categories;
        }

        public ICollection<ServiceTO> UserServices(string user)
        {
            var services = unitOfWork.ServiceRepository
                                     .GetByPredicate(s => s.UserServices.Any(us => us.ApplicationUserId.Equals(user)))
                                     .GroupBy(s => s.Group, s => s, (key, serv) =>
                                            new { Group = key, Services = serv.ToList() })
                                     .OrderBy(s => s.Group.Title)
                                     .Select(s => new ServiceTO
                                     {
                                          ServiceGroup = Mapping.Mapping.Mapper.Map<ServiceGroup>(s.Group),
                                          Services = s.Services.Select(service => Mapping.Mapping.Mapper.Map<Service>(service))
                                                               .OrderBy(service => service.Title)
                                                               .ToList()
                                      }).ToList();
            return services;
        }

        public ServiceGroup GetServiceGroup(int serviceId)
        {
            if (serviceId <= 0)
            {
                throw new ArgumentException(nameof(serviceId));
            }

            var retrieveServiceWithGroup = unitOfWork.ServiceRepository.GetById(serviceId);
            var serviceGroup = retrieveServiceWithGroup.Group;

            return Mapping.Mapping.Mapper.Map<ServiceGroup>(serviceGroup);
        }
    }
}