using SharedServices.BL.Domain;
using System;

namespace SharedServices.BL.UseCases.Clients
{
    public partial class Client
    {
        public ServiceGroup AddGroupService(ServiceGroup group)
        {
            if (group is null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            var addedGroup = unitOfWork.ServiceGroupRepository
                      .Insert(Mapping.Mapping.Mapper.Map<DAL.Entities.ServiceGroup>(group));

            return Mapping.Mapping.Mapper.Map<ServiceGroup>(addedGroup);
        }

        public ServiceGroup GetGroupServiceById(int groupId)
        {
            if (groupId <= 0)
            {
                throw new ArgumentException(nameof(groupId));
            }

            var addedGroup = unitOfWork.ServiceGroupRepository
                      .GetById(groupId);

            return Mapping.Mapping.Mapper.Map<ServiceGroup>(addedGroup);
        }
    }
}