using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SharedServices.BL.Domain;
using SharedServices.BL.Extensions;
using SharedServices.BL.UseCases.Admin;
using SharedServices.BL.UseCases.Clients;
using SharedServices.DAL;
using SharedServices.DAL.UnitOfWork;
using SharedServices.Mutual.Enumerations;
using SharedServices.UI.Attributes;
using SharedServices.UI.Models;
using SharedServices.UI.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SharedServices.UI.Controllers
{
    [Authorize]
    public partial class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Client _client;
        private readonly Adminitrator _admin;
        private readonly string _culture;

        public UserController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _client = new Client(_unitOfWork, _userManager);
            _admin = new Adminitrator(_unitOfWork);
            _culture = CultureInfo.CurrentCulture.Name;
        }

        // GET: Request
        public IActionResult Profile()
        {
            return View();
        }

        public async Task<IActionResult> Points()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var userPoints = user.Point;

                return Json(new { status = true, points = userPoints });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "We encountered an error. " });
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Visitors()
        {
            var visitors = _admin.GetVisitors();
            return View(visitors);
        }
    }
}