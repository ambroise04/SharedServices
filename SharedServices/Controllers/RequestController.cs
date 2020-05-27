using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedServices.BL.Domain;
using SharedServices.BL.Extensions;
using SharedServices.BL.UseCases.Clients;
using SharedServices.DAL;
using SharedServices.DAL.UnitOfWork;
using SharedServices.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedServices.UI.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private Client _client;

        public RequestController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _client = new Client(_unitOfWork, _userManager);
        }
        class RequestViewModel
        {
            public ICollection<Request> ReceivedRequests { get; set; }
            public ICollection<Request> SentRequests { get; set; }
        }
        // GET: Request
        public ActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var user = _client.UserRequests(userId);

            return View(user);
        }

        [HttpGet]
        public IActionResult Create(int service, string flag)
        {
            if (service <= 0 || string.IsNullOrEmpty(flag))
            {
                throw new Exception("Bad request.");
            }
            var serviceRetrieved = _client.GetServiceById(service);
            var user = _userManager.Users.Include(u => u.Picture).FirstOrDefault(u => u.Id.Equals(flag));

            if (serviceRetrieved == null || user == null)
            {
                throw new Exception("One or more parameters are bad.");
            }

            var model = new RequestFormViewModel
            {
                ServiceId = serviceRetrieved.Id,
                ServiceTitle = serviceRetrieved.Title,
                OperationPoint = serviceRetrieved.Group.PointsByHour,
                TargetId = user.Id,
                TargetFullName = string.Concat(user.FirstName, " ", user.LastName),
                TargetUserPictureSource = user.PictureSource()
            };

            return PartialView("_RequestForm", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int service, string flag, DateTime date)
        {            
           return null;
        }
    }
}