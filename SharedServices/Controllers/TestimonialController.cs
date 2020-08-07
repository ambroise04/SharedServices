using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedServices.BL.Domain;
using SharedServices.BL.Extensions;
using SharedServices.BL.UseCases.Clients;
using SharedServices.DAL;
using SharedServices.DAL.UnitOfWork;
using SharedServices.UI.Attributes;
using SharedServices.UI.Models.TestimonialModels;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SharedServices.UI.Controllers
{
    [Authorize]
    public class TestimonialController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Client client;
        public TestimonialController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {

            _unitOfWork = unitOfWork;
            _userManager = userManager;
            client = new Client(this._unitOfWork, userManager);
        }
        public IActionResult Index()
        {
            return View();
        }
        
        [Ajax(HttpVerb = "GET")]
        public IActionResult UserInfos(string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                throw new ArgumentException("Bad params", nameof(target));
            }
            var user = _userManager.Users.Include(u => u.Picture).FirstOrDefault(u => u.Id.Equals(target));
            if (user is null)
            {
                throw new ArgumentException("Bad target was submitted", nameof(user));
            }
            var infos = new TestimonialViewModel
            {
                Id = user.Id,
                FullName = string.Concat(user.FirstName, " ", user.LastName),
                ImageSource = user.ResizePicture(45, 45)
            };

            return PartialView("_TestimonialContent", infos);
        }

        public async Task<IActionResult> Rate(string target, int rate, string advice)
        {
            var cultureFR = CultureInfo.CurrentCulture.Name.Contains("fr");
            if (rate <= 0 || string.IsNullOrEmpty(target))
            {
                var message = cultureFR ? "Veuillez remplir tous les champs s'il vous plaît."
                    : "Please fill in all the fields";

                return Json(new { status = false, message });
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var userToRate = await _userManager.FindByIdAsync(target);
            if (userToRate is null)
            {
                var message = cultureFR ? "Désolé, Votre avis ne peut être envoyé. Veuillez vérifier les données saisies s'il vous plaît."
                    : "Sorry, your feedback cannot be sent. Please check the data entered.";

                return Json(new { status = false, message });
            }

            var feedback = new Feedback
            {
                Comment = advice,
                Mark = rate,
                DisplayAdvisorName = true,
                Advisor = currentUser.Id,
                User = userToRate
            };

            _unitOfWork.CreateTransaction();
            try
            {
                var result = client.AddFeedback(feedback);
                if (result is null)
                {
                    _unitOfWork.RollbackTransaction();
                    var message = cultureFR ? "Désolé, Votre avis ne peut être envoyé. Veuillez réessayer plus tard."
                    : "Sorry, your feedback cannot be sent. Please try again later.";
                    return Json(new { status = false, message });
                }

                int stars = client.UserStars(userId: target, newRate: rate);
                userToRate.Start = stars;
                _unitOfWork.CommitTransaction();
                var successMessage = cultureFR ? "Votre avis a été envoyé avec succès. Veuillez réessayer plus tard."
                    : "Your feedback has been sent successfully. Please try again later.";
                return Json(new { status = true, message = successMessage });
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                var message = cultureFR ? "Désolé, un problème a été rencontré. Veuillez réessayer plus tard."
                : "Sorry, a problem has been encountered. Please try again later.";
                return Json(new { status = false, message });
            }
        }
    }
}