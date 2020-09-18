using SharedServices.DAL.Entities;
using System.Collections.Generic;

namespace SharedServices.DAL.Interfaces
{
    public interface IUserSessionRepository
    {
        bool AddSessionInfo(UserSession userSession);
        IEnumerable<UserSession> GetSessionInfos();
        bool SessionExists(string ipAddress);
    }
}