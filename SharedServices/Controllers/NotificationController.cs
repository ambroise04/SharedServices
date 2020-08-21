using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharedServices.BL.UseCases.Clients;
using SharedServices.DAL;
using SharedServices.DAL.UnitOfWork;
using SharedServices.UI.Extensions;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace SharedServices.UI.Controllers
{
    [Authorize]
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

        public async Task<IActionResult> Notifications()
        {
            var userId = _userManager.GetUserId(User);
            var notifications = _client.GetUserNotifications(userId);
            var isFR = CultureInfo.CurrentCulture.Name.ToLower().Contains("fr");
            var viewModel = notifications.ToViewModelCollection(isFR);

            await _client.MarkUserNotificationsAsTriggered(userId);

            return View(viewModel);
        }

        public IActionResult CheckMulticastNotifications()
        {
            var userId = _userManager.GetUserId(User);
            var count = _client.GetUserNotTriggeredNotifications(userId);

            return Json(new { status = true, count });
        }
    }
}