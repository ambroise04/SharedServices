using SharedServices.BL.Domain;
using SharedServices.BL.UseCases.Clients;

namespace SharedServices.BL.UseCases.Admin
{
    public partial class Adminitrator : Client
    {
        public ServiceGroup UpdateGroupService(ServiceGroup group)
        {
            var updatedGroup = unitOfWork.ServiceGroupRepository
                      .Update(Mapping.Mapping.Mapper.Map<DAL.Entities.ServiceGroup>(group));

            return Mapping.Mapping.Mapper.Map<ServiceGroup>(updatedGroup);
        }
    }
}