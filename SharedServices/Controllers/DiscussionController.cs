using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedServices.BL.Domain;
using SharedServices.BL.UseCases.Clients;
using SharedServices.DAL;
using SharedServices.DAL.UnitOfWork;
using SharedServices.Mutual.Extensions;
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
        private readonly Client client;
        public DiscussionController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {

            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            client = new Client(this.unitOfWork, userManager);
        }
        // GET: Discussion
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            string user = userManager.GetUserId(User);

            var contacts = client.GetContacts(user);

            var lastDiscussion = client.GetUserDiscussion(user).FirstOrDefault();
            var discussions = new List<Discussion>();
            if (lastDiscussion != null)
            {
                discussions = lastDiscussion.Emitter.Equals(user) ?
                    client.GetDiscussionBetweenCurrentUserAndAnOtherUser(user, lastDiscussion.Receiver) :
                    client.GetDiscussionBetweenCurrentUserAndAnOtherUser(user, lastDiscussion.Emitter);
            }
            await UserContact();                        

            ViewBag.Contacts = contacts;

            return View(discussions);
        }

        public IActionResult Contact(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { status = false, message = "Bad parameter" });
            }

            string user = userManager.GetUserId(User);
            var discussions = client.GetDiscussionBetweenCurrentUserAndAnOtherUser(user, id)
                                    .Select(d => d.ToTransfer())
                                    .Reverse()
                                    .ToList();

            return Json(new { status = true, discussions });
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
                while (user.Contacts.Count() > 2)
                {
                    user.Contacts.Remove(user.Contacts.Last());
                    user.Contacts.Remove(user.Contacts.Last());
                }
            }

            await userManager.UpdateAsync(user);
        }
    }
}