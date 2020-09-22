using Newtonsoft.Json;
using SharedServices.BL.Domain;
using SharedServices.BL.UseCases.Clients;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace SharedServices.BL.UseCases.Admin
{
    public partial class Adminitrator : Client
    {
        private static UserSession GetUserInfoByIpAddress(string ipAddress)
        {
            UserSession userInfo = new UserSession();
            try
            {                
                string info = new WebClient().DownloadString("http://ipinfo.io/" + ipAddress);
                userInfo = JsonConvert.DeserializeObject<UserSession>(info);
                if (!string.IsNullOrEmpty(userInfo.Country))
                {
                    RegionInfo regionInfo = new RegionInfo(userInfo.Country);
                    userInfo.Country = regionInfo.EnglishName;
                }
                userInfo.SessionDate = DateTime.Now;
            }
            catch (Exception)
            {
                throw;
            }

            return userInfo;
        }

        public void SaveSessionInfos(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                throw new ArgumentNullException($"Ip address cannot be null or empty. {nameof(ip)}");

            ip = ip.Split(':').First();
            var exists = unitOfWork.UserSessionRepository.SessionExists(ip);
            if (exists) return;

            try
            {
                var session = GetUserInfoByIpAddress(ip);
                var dalSession = Mapping.Mapping.Mapper.Map<DAL.Entities.UserSession>(session);
                unitOfWork.UserSessionRepository.AddSessionInfo(dalSession);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                //Log exception
                throw;
            }
        }

        public ICollection<UserSession> GetVisitors()
        {
            return unitOfWork.UserSessionRepository
                             .GetSessionInfos()
                             .Select(s => Mapping.Mapping.Mapper.Map<UserSession>(s))
                             .ToList();
        }
    }

}