﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedServices.BL.Domain;
using SharedServices.BL.Extensions;
using SharedServices.BL.UseCases.Clients;
using SharedServices.DAL;
using SharedServices.DAL.UnitOfWork;
using SharedServices.Mutual.Extensions;
using SharedServices.UI.Attributes;
using SharedServices.UI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SharedServices.UI.Controllers
{
    public class DiscussionController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly Client client;
        public DiscussionController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.signInManager = signInManager;
            client = new Client(this.unitOfWork, userManager);
        }
        // GET: Discussion
        [HttpGet]
        public ActionResult Index()
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

        public IActionResult UserInfo(string target)
        {
            if (!signInManager.IsSignedIn(User))
                return StatusCode(401);

            if (string.IsNullOrEmpty(target))
                throw new ArgumentException("Bad params", nameof(target));

            var user = userManager.Users.Include(u => u.Picture).FirstOrDefault(u => u.Id.Equals(target));
            if (user is null)
            {
                throw new ArgumentException("Bad target was submitted", nameof(user));
            }
            var infos = new SearchMessageUserInfos
            {
                Id = user.Id,
                FullName = string.Concat(user.FirstName, " ", user.LastName),
                ImageSource = user.ResizePicture(45, 45)
            };

            return PartialView("_SearchMessage", infos);
        }

        [Ajax(HttpVerb = "POST")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Discuss(string target, string message)
        {
            var cultureFR = CultureInfo.CurrentCulture.Name.Contains("fr");

            if (string.IsNullOrEmpty(target) || string.IsNullOrEmpty(message))
            {
                var errorMessage = cultureFR ? "Des paramètres incorrects ont été envoyés. Veuillez vérifier vos données et réessayez s'il vous plaît."
                    : "Incorrect parameters have been sent.Please check your details and try again please";
                return Json(new { status = false, message = errorMessage });
            }

            var targetUser = await userManager.FindByIdAsync(target);

            if (targetUser is null)
            {
                var errorMessage = cultureFR ? "Votre correspondant n'a pas été trouvé. Veuillez réessayer s'il vous plaît."
                : "Your correspondent was not found.Try again, please";

                return Json(new { status = false, message = errorMessage });
            }

            var currentUser = await userManager.GetUserAsync(User);
            Discussion discussion = new Discussion
            {
                Emitter = currentUser.Id,
                Receiver = targetUser.Id,
                Message = message,
                DateHour = DateTime.Now,
            };

            unitOfWork.CreateTransaction();
            try
            {
                var result = client.AddDiscussion(discussion);

                if (result is null)
                {
                    unitOfWork.RollbackTransaction();
                    var errorMessage = cultureFR ? "Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît."
                        : "An error has been encountered. Try again, please.";
                    return Json(new { status = false, message = errorMessage });
                }
                result.EmitterUser = userManager.Users.Include(u => u.Picture).FirstOrDefault(u => u.Id.Equals(result.Emitter));
                var successMessage = cultureFR ? "Votre message a été envoyé avec succès." : "Your messsage has been sent successfully.";
                unitOfWork.CommitTransaction();

                return Json(new { status = true, message = successMessage, discussion = result.ToTransfer(), current = currentUser.Id });
            }
            catch (Exception)
            {
                unitOfWork.RollbackTransaction();
                var errorMessage = cultureFR ? "Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît."
                    : "An error has been encountered. Try again, please.";

                return Json(new { status = false, message = errorMessage });
            }
        }

        //private async Task UserContact()
        //{
        //    var currentUser = await userManager.GetUserAsync(User);
        //    var user = userManager.Users.Include(u => u.Contacts).FirstOrDefault(u => u.Id.Equals(currentUser.Id));
        //    if (user.Contacts is null || user.Contacts.Count() == 0)
        //    {
        //        user.Contacts = new List<ApplicationUser>();
        //        var gauthier = userManager.FindByEmailAsync("gauthier.lallem@gmail.com").Result;
        //        var pauline = userManager.FindByEmailAsync("pauline.j@gmail.com").Result;
        //        user.Contacts.Add(gauthier);
        //        user.Contacts.Add(pauline);
        //    }
        //    else
        //    {
        //        while (user.Contacts.Count() > 2)
        //        {
        //            user.Contacts.Remove(user.Contacts.Last());
        //            user.Contacts.Remove(user.Contacts.Last());
        //        }
        //    }

        //    await userManager.UpdateAsync(user);
        //}
    }
}