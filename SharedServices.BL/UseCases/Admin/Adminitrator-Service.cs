using SharedServices.BL.Domain;
using SharedServices.BL.UseCases.Clients;

namespace SharedServices.BL.UseCases.Admin
{
    public partial class Adminitrator : Client
    {
        public Service UpdateService(Service service)
        {
            var addedService = unitOfWork.ServiceRepository
                      .Update(Mapping.Mapping.Mapper.Map<DAL.Entities.Service>(service));

            return Mapping.Mapping.Mapper.Map<Service>(addedService);
        }
    }
}