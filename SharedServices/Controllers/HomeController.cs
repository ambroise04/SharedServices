using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedServices.BL.Services;
using SharedServices.DAL;
using SharedServices.UI.Attributes;
using SharedServices.UI.Models;
using SharedServices.UI.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private IGlobalInfo _globalInfo;

        public HomeController(ILogger<HomeController> logger,
            IBroadcastEmailSender emailSender,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, 
            IGlobalInfo globalInfo)
        {
            _logger = logger;
            _emailSender = emailSender;
            _userManager = userManager;
            _signInManager = signInManager;
            _globalInfo = globalInfo;
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