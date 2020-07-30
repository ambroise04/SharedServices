using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharedServices.BL.UseCases.Clients;
using SharedServices.DAL;
using SharedServices.DAL.UnitOfWork;
using SharedServices.UI.Extensions;
using System;
using System.Globalization;

namespace SharedServices.UI.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private Client _client;

        public NotificationController(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _client = new Client(_unitOfWork);
        }

        public IActionResult Notifications()
        {
            var userId = _userManager.GetUserId(User);
            var notifications = _client.GetUserNotifications(userId);
            var isFR = CultureInfo.CurrentCulture.Name.ToLower().Contains("fr");

            return View(notifications.ToViewModelCollection(isFR));
        }
    }
}