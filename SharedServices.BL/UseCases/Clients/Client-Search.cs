using Microsoft.EntityFrameworkCore;
using SharedServices.DAL;
using System.Collections.Generic;
using System.Linq;

namespace SharedServices.BL.UseCases.Clients
{
    public partial class Client
    {
        public List<ApplicationUser> SearchResult(string service, string userId)
        {
            var users = new List<ApplicationUser>();
            if (!string.IsNullOrEmpty(userId))
            {
                users = userManager.Users.Include(u => u.Picture)
                                   .Where(u => u.UserServices.Any(s => s.Service.Title.ToLower().Contains(service.ToLower().Trim()))
                                        && !u.Id.Equals(userId))
                                   .OrderByDescending(u => u.Start)
                                   .ToList();
            }
            else
            {
                users = userManager.Users.Include(u => u.Picture)
                                   .Where(u => u.UserServices.Any(s => s.Service.Title.ToLower().Contains(service.ToLower().Trim())))
                                   .OrderByDescending(u => u.Start)
                                   .ToList();
            }


            return users;
        }

        public List<ApplicationUser> SearchAllUsers(string userId)
        {
            var users = userManager.Users
                                   .Include(u => u.Picture)
                                   .Where(u => !u.Id.Equals(userId))
                                   .OrderByDescending(u => u.Start);

            return users.ToList();
        }
    }
}