using Microsoft.EntityFrameworkCore;
using SharedServices.BL.Domain;
using SharedServices.DAL;
using System.Collections.Generic;
using System.Linq;

namespace SharedServices.BL.UseCases.Clients
{
    public partial class Client
    {
        public List<ApplicationUser> SearchResult(string service)
        {
            var users = userManager.Users.Include(u => u.Picture)
                                   .Where(u => u.UserServices.Any(s => s.Service.Title.ToLower().Contains(service.ToLower().Trim())))
                                   .OrderByDescending(u => u.Start);

            return users.ToList();
        }

        public List<ApplicationUser> SearchAllUsers()
        {
            var users = userManager.Users.Include(u => u.Picture).OrderByDescending(u => u.Start);

            return users.ToList();
        }
    }
}