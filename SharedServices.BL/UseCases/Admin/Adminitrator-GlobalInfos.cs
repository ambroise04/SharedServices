using SharedServices.BL.UseCases.Clients;
using SharedServices.Mutual;

namespace SharedServices.BL.UseCases.Admin
{
    public partial class Adminitrator : Client
    {
        public GlobalInfo GetInfo()
        {
            var retrievedInfo = unitOfWork.GlobalInfoRepository
                      .GetInfo();

            return retrievedInfo;
        }

        public GlobalInfo UpdateGlobalInfos(GlobalInfo infos)
        {
            var updatedInfo = unitOfWork.GlobalInfoRepository
                      .Update(infos);

            return updatedInfo;
        }
    }
}