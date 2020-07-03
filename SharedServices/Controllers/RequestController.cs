using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SharedServices.BL.Domain;
using SharedServices.BL.Extensions;
using SharedServices.BL.UseCases.Admin;
using SharedServices.BL.UseCases.Clients;
using SharedServices.DAL;
using SharedServices.DAL.UnitOfWork;
using SharedServices.Mutual.Enumerations;
using SharedServices.UI.Extensions;
using SharedServices.UI.Models;
using SharedServices.UI.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SharedServices.UI.Controllers
{
    [Authorize]
    public partial class RequestController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBroadcastEmailSender _broadcastEmailSender;
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly Client _client;
        private readonly Adminitrator _admin;
        private readonly IHttpContextAccessor _httpContext;
        private readonly string _culture;


        public RequestController(
            IUnitOfWork unitOfWork, 
            UserManager<ApplicationUser> userManager, 
            IBroadcastEmailSender broadcastEmailSender,
            IHttpContextAccessor httpContext,
            IHubContext<SignalRHub> hubContext)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _broadcastEmailSender = broadcastEmailSender;
            _client = new Client(_unitOfWork, _userManager);
            _admin = new Adminitrator(_unitOfWork);
            _culture = CultureInfo.CurrentCulture.Name;
            _httpContext = httpContext;
            _hubContext = hubContext;
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
            var cultureFR = CultureInfo.CurrentCulture.Name.Contains("fr");
            if (service <= 0 || string.IsNullOrEmpty(flag))
            {
                var message = cultureFR ? "Veuillez saisir le service dont vous avez besoin dans le champs de recherche."
                    : "Please type the service you need in the search input.";

                return Json(new { status = false, message });
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
        public async Task<IActionResult> Create(int service, string flag, string date, 
            string city, string country, int postalcode)
        {
            var cultureFR = CultureInfo.CurrentCulture.Name.Contains("fr");

            if (string.IsNullOrEmpty(flag) || string.IsNullOrEmpty(date) || service <= 0)
            {
                var errorMessage = cultureFR ? "Une erreur liée aux données a été rencontrée! Veuillez réessayer s'il vous plaît."
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

            _unitOfWork.CreateTransaction();
            try
            {
                var place = new Place 
                { 
                    City = city, 
                    Country = country, 
                    PostalCode = postalcode
                };

                var request = new Request
                {
                    Service = serviceRetrieved,
                    Accepted = false,
                    DateOfAddition = DateTime.Now,
                    DateOfRequest = requestDate,
                    Requester = user,
                    Receiver = receiver,
                    Source = RequestSource.Personal,
                    Place = place
                };

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