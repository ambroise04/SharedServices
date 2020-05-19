using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedServices.BL.Domain;
using SharedServices.BL.UseCases.Clients;
using SharedServices.DAL;
using SharedServices.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedServices.UI.Controllers
{
    public class DiscussionController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private Client client;
        public DiscussionController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {

            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            client = new Client(this.unitOfWork);
        }
        // GET: Discussion
        [HttpGet]
        public async Task<ActionResult> Contact()
        {
            string user = userManager.GetUserId(User);

            var discussions = client.GetUserDiscussion(user);
            await UserContact();
            discussions = discussions.Select(d => new Discussion
            {
                Id = d.Id,
                DateHour = d.DateHour,
                Message = d.Message,
                Emitter = d.Emitter,
                Receiver = d.Receiver,
                EmitterUser = userManager.Users.Include(u => u.Picture).Include(u => u.Contacts).FirstOrDefault(u => u.Id.Equals(d.Emitter)),
                ReceiverUser = userManager.Users.Include(u => u.Picture).Include(u => u.Contacts).FirstOrDefault(u => u.Id.Equals(d.Receiver))
            }).ToList();

            return View(discussions);
        }

        private async Task UserContact()
        {
            var currentUser = await userManager.GetUserAsync(User);
            var user = userManager.Users.Include(u => u.Contacts).FirstOrDefault(u => u.Id.Equals(currentUser.Id));
            if (user.Contacts is null || user.Contacts.Count() == 0)
            {
                user.Contacts = new List<ApplicationUser>();
                var gauthier = userManager.FindByEmailAsync("gauthier.lallem@gmail.com").Result;
                var pauline = userManager.FindByEmailAsync("pauline.j@gmail.com").Result;
                user.Contacts.Add(gauthier);
                user.Contacts.Add(pauline);
            }
            else 
            {
                while(user.Contacts.Count() > 2) 
                { 
                    user.Contacts.Remove(user.Contacts.Last());
                    user.Contacts.Remove(user.Contacts.Last());
                }
            }

            await userManager.UpdateAsync(user);
        }
    }
}