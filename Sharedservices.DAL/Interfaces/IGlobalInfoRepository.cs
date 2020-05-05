using SharedServices.Mutual;

namespace SharedServices.DAL.Interfaces
{
    public interface IGlobalInfoRepository
    {
        public GlobalInfo GetInfo();
        public GlobalInfo Update(GlobalInfo entity);
    }
}