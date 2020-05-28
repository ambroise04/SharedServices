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
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Create(int service, string flag, string date)
        {
            var cultureFR = CultureInfo.CurrentCulture.Name.Contains("fr");

            if (string.IsNullOrEmpty(flag) || string.IsNullOrEmpty(date) || service <= 0)
            {
                var errorMessage = cultureFR ? "Une erreur liée aux données a été rencontré! Veuillez réessayer s'il vous plaît."
                    : "Parameter related errors have been encountered! Try again, please.";
                return Json(new { status = false, message = errorMessage });
            }
            date = cultureFR ? date : string.Join('-', date.Split("/"));

            var requestDate = DateTime.Parse(date);
            var serviceRetrieved = _client.GetServiceById(service);
            var receiver = await _userManager.FindByIdAsync(flag);
            var user = await _userManager.GetUserAsync(User);

            if (receiver.Id.Equals(user.Id))
            {
                var errorMessage = cultureFR ? "Impossible d'envoyer une demander à vous-mêmes."
                    : "Unable to send a request to yourself";
                return Json(new { status = false, message = errorMessage });
            }

            var request = new Request
            {
                Service = serviceRetrieved,
                Accepted = false,
                DateOfAddition = DateTime.Now,
                DateOfRequest = requestDate,
                Requester = user,
                Receiver = receiver
            };

            _unitOfWork.CreateTransaction();
            try
            {
                var result = _client.AddRequest(request);                
                if (request != null)
                {
                    var successMessage = cultureFR ? "Demande envoyée avec succès."
                        : "Request sent successfully";
                    _unitOfWork.CommitTransaction();
                    return Json(new { status = true, message = successMessage });
                }
                else
                {
                    _unitOfWork.RollbackTransaction();
                    var errorMessage = cultureFR ? "Votre demande a échoué! Veuillez réessayer s'il vous plaît."
                        : "Your request has failed! Try again, please.";
                    return Json(new { status = false, message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                var errorMessage = cultureFR ? "Un problème a été rencontré! Veuillez réessayer s'il vous plaît."
                    : "A problem has been encountered! Try again, please.";
                return Json(new { status = false, message = errorMessage });
            }
        }
    }
}