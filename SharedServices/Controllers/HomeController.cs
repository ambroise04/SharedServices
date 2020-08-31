using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedServices.BL.Domain;
using SharedServices.BL.Services;
using SharedServices.BL.UseCases.Admin;
using SharedServices.BL.UseCases.Clients;
using SharedServices.DAL;
using SharedServices.DAL.UnitOfWork;
using SharedServices.Mutual;
using SharedServices.UI.Attributes;
using SharedServices.UI.Extensions;
using SharedServices.UI.Models;
using SharedServices.UI.Models.FaqViewModels;
using SharedServices.UI.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Threading.Tasks;

namespace SharedServices.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBroadcastEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private Client _client;
        private Adminitrator _admin;
        private IGlobalInfo _globalInfo;

        public HomeController(ILogger<HomeController> logger,
            IBroadcastEmailSender emailSender,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IGlobalInfo globalInfo, 
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _emailSender = emailSender;
            _userManager = userManager;
            _signInManager = signInManager;
            _globalInfo = globalInfo;
            _unitOfWork = unitOfWork;
            _client = new Client(_unitOfWork);
            _admin = new Adminitrator(_unitOfWork);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Steps()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [Ajax(HttpVerb = "POST")]
        public async Task<IActionResult> Contact(string email, string subject, string message)
        {
            var cultureFR = CultureInfo.CurrentCulture.Name.Contains("fr");

            try
            {
                email = _signInManager.IsSignedIn(User) ? _userManager.GetUserAsync(User).Result.Email : email;
                var name = string.Empty;
                if (_signInManager.IsSignedIn(User))
                {
                    var user = _userManager.GetUserAsync(User).Result;
                    name = string.Concat(user.FirstName, " ", user.LastName, " - ");
                }
                await _emailSender.SendContactEmailAsync(email: email, subject: subject, message: message, name: name, receiver: _globalInfo.GetGlobalInfo().Email);

                var statusMessage = cultureFR ? "Message envoyé avec succès" : "Message sent successfully.";

                return Json(new { status = true, message = statusMessage });
            }
            catch (Exception ex)
            {
                var statusMessage = cultureFR ? "Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît." : 
                    "An error occurs. Please try again.";

                return Json(new { status = false, message = statusMessage });
            }
        }

        public IActionResult FAQs()
        {
            bool inFr = CultureInfo.CurrentCulture.Name.ToLower().Contains("fr");
            List<FaqQuestionVM> questions = _client.GetAnsweredFaqQuestions().ToViewModels(inFr);
            
            return View(questions);
        }

        [Authorize]
        [Ajax(HttpVerb = "POST")]
        public IActionResult FAQs(string message)
        {
            var cultureFR = CultureInfo.CurrentCulture.Name.Contains("fr");

            if (string.IsNullOrEmpty(message))
            {
                var statusMessage = cultureFR ? "Données invalides. Veuillez réessayer s'il vous plaît." :
                    "Invalid data. Please try again.";

                return Json(new { status = false, message = statusMessage });
            }
            var question = CreateQuestion(message);

            _unitOfWork.CreateTransaction();
            try
            {
                var result = _client.CreateFaqQuestion(question);

                if (!result)
                {
                    _unitOfWork.RollbackTransaction();
                    var statusMessage = cultureFR ? "Désolé, une erreur a été rencontrée. Veuillez réessayer s'il vous plaît." :
                        "Sorry, an error has been encountered.Try again, please.";

                    return Json(new { status = false, message = statusMessage });
                }
                _unitOfWork.CommitTransaction();
                var succesMessage = cultureFR ? "Votre question a été envoyée avec succès. Notre équipe vous répondra dans les plus brefs délais." :
                    "Your question has been sent successfully. Our team will get back to you as soon as possible.";

                return Json(new { status = true, message = succesMessage });
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                var errorMessage = cultureFR ? "Désolé, une erreur a été rencontrée. Veuillez réessayer s'il vous plaît." :
                        "Sorry, an error has been encountered.Try again, please.";

                return Json(new { status = false, message = errorMessage });
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminFAQs()
        {
            bool inFr = CultureInfo.CurrentCulture.Name.ToLower().Contains("fr");
            List<FaqQuestionVM> questions = _admin.GetFaqQuestions().ToViewModels(inFr);

            return View(questions);
        }

        [Authorize(Roles = "Admin")]
        [Ajax(HttpVerb = "GET")]
        public IActionResult UserQuestions(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException($"'{nameof(id)}' cannot be smaller than zero", nameof(id));
            }
            bool inFr = CultureInfo.CurrentCulture.Name.ToLower().Contains("fr");
            var userQuestion = _admin.GetFaqQuestion(id).ToViewModel(inFr);

            return Json(new { status = true, question = userQuestion });
        }

        [Authorize(Roles = "Admin")]
        [Ajax(HttpVerb = "POST")]
        public IActionResult FaqAnswer(int question, string response)
        {
            _unitOfWork.CreateTransaction();
            try
            {
                FaqResponse faqResponse = new FaqResponse
                {
                    Message = response,
                    Date = DateTime.Now,
                    IsDeleted = false
                };
                
                var result = _admin.AnswerToFaqQuestion(question, faqResponse);
                _unitOfWork.CommitTransaction();

                bool inFr = CultureInfo.CurrentCulture.Name.ToLower().Contains("fr");
                var viewResponse = faqResponse.ToViewModel(inFr);

                return Json(new { status = true, response = viewResponse });
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                return Json(new { status = false, message = $"Error. {ex.Message}" });
            }
        }

        private FaqQuestion CreateQuestion(string message)
        {
            var user = _userManager.GetUserAsync(User).Result;

            var question = new FaqQuestion
            {
                Message = message,
                User = user,
                Date = DateTime.Now,
                IsDeleted = false,
            };

            return question;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}