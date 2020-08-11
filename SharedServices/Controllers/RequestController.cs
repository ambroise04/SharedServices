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
using SharedServices.Mutual;
using SharedServices.Mutual.Enumerations;
using SharedServices.UI.Attributes;
using SharedServices.UI.Models;
using SharedServices.UI.Models.RequestViewModels;
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
    public partial class RequestController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IBroadcastEmailSender _broadcastEmailSender;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IUserConnectionManager _userConnectionManager;
        private readonly INotificationService _notificationService;
        private readonly Client _client;
        private readonly Adminitrator _admin;
        private readonly IHttpContextAccessor _httpContext;
        private readonly string _culture;

        public RequestController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IBroadcastEmailSender broadcastEmailSender,
            ICompositeViewEngine viewEngine,
            IHttpContextAccessor httpContext,
            IHubContext<NotificationHub> hubContext,
            INotificationService notificationService, 
            IUserConnectionManager userConnectionManager)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager;
            _broadcastEmailSender = broadcastEmailSender;
            _viewEngine = viewEngine;
            _client = new Client(_unitOfWork, _userManager);
            _admin = new Adminitrator(_unitOfWork);
            _culture = CultureInfo.CurrentCulture.Name;
            _httpContext = httpContext;
            _hubContext = hubContext;
            _userConnectionManager = userConnectionManager;
            _notificationService = notificationService;
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
        [AllowAnonymous]
        public async Task<IActionResult> Create(int service, string flag)
        {
            if (!_signInManager.IsSignedIn(User))
            {
                return StatusCode(401);
            }
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
            var currentUser = await _userManager.GetUserAsync(User);
            var model = new RequestFormViewModel
            {
                ServiceId = serviceRetrieved.Id,
                ServiceTitle = serviceRetrieved.Title,
                UserPoint = currentUser.Point,
                TargetId = user.Id,
                TargetFullName = string.Concat(user.FirstName, " ", user.LastName),
                TargetUserPictureSource = user.PictureSource()
            };

            return PartialView("_RequestForm", model);
        }

        [Ajax(HttpVerb = "POST")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRequestVM requestVM)
        {
            var cultureFR = CultureInfo.CurrentCulture.Name.Contains("fr");

            if (string.IsNullOrEmpty(requestVM.Flag) || string.IsNullOrEmpty(requestVM.Date) || requestVM.Service <= 0)
            {
                var errorMessage = cultureFR ? "Une erreur liée aux données a été rencontrée! Veuillez réessayer s'il vous plaît."
                    : "Parameter related errors have been encountered! Try again, please.";
                return Json(new { status = false, message = errorMessage });
            }
            var date = cultureFR ? requestVM.Date : string.Join('-', requestVM.Date.Split("/"));

            var requestDate = DateTime.Parse(date);
            var serviceRetrieved = _client.GetServiceById(requestVM.Service);
            var receiver = await _userManager.FindByIdAsync(requestVM.Flag);
            var user = await _userManager.GetUserAsync(User);

            if (receiver.Id.Equals(user.Id))
            {
                var errorMessage = cultureFR ? "Impossible d'envoyer une demander à vous-mêmes."
                    : "Unable to send a request to yourself";
                return Json(new { status = false, message = errorMessage });
            }

            if (user.Point < requestVM.Point)
            {
                var errorMessage = cultureFR ? "Nous n'avez pas assez de points pour cette demande."
                    : "You don't have enough points for this request.";
                return Json(new { status = false, message = errorMessage });
            }

            _unitOfWork.CreateTransaction();
            try
            {
                var place = new Place
                {
                    City = requestVM.City,
                    Country = requestVM.Country,
                    PostalCode = requestVM.PostalCode
                };

                var request = new Request
                {
                    Service = serviceRetrieved,
                    State = RequestStates.Waiting,
                    DateOfAddition = DateTime.Now,
                    DateOfRequest = requestDate,
                    Requester = user,
                    Point = requestVM.Point,
                    Duration = requestVM.Duration,
                    Receiver = receiver,
                    Source = RequestSource.Personal,
                    Place = place
                };

                var result = _client.AddRequest(request);
                if (request != null)
                {
                    if (!_userManager.IsInRoleAsync(user, Roles.Admin.ToString()).Result)
                    {
                        user.Point -= requestVM.Point;
                        await _userManager.UpdateAsync(user);
                    }
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

        [Ajax(HttpVerb = "POST")]
        public async Task<IActionResult> CancelRequest(int id, RequestSource source)
        {
            if (id <= 0)
                return StatusCode(403);

            var request = _client.GetRequestById(id);
            if (request is null)
                return StatusCode(404);

            var currentUser = _userManager.GetUserAsync(User).Result;

            if (!currentUser.Id.Equals(request.Requester.Id))
                return StatusCode(403);

            var cultureFR = CultureInfo.CurrentCulture.Name.Contains("fr");
            _unitOfWork.CreateTransaction();
            try
            {
                var result = _client.CancelRequest(request);
                if (result)
                {
                    currentUser.Point += request.Point;
                    await _userManager.UpdateAsync(currentUser);
                    _unitOfWork.CommitTransaction();
                    var message = cultureFR ? "Demande annulée avec succès."
                    : "Request canceled successfully.";

                    return Json(new { status = true, message });
                }
                else
                {
                    _unitOfWork.RollbackTransaction();
                    var message = cultureFR ? "L'opération a échoué. Veuillez réessayer s'il vous plaît."
                    : "Operation failed. Try again, please.";

                    return Json(new { status = false, message });
                }
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                var message = cultureFR ? "Une erreur a été rencontrée. Veuillez réessayer plus tard."
                    : "An error was encountered. Please, try again later.";

                return Json(new { status = false, message });
            }
        }

        [Ajax(HttpVerb = "POST")]
        public IActionResult AcceptRequest(int id)
        {
            if (id <= 0)
                return StatusCode(403);

            var request = _client.GetRequestById(id);
            if (request is null)
                return StatusCode(404);

            var currentUserId = _userManager.GetUserId(User);

            if (!currentUserId.Equals(request.Receiver.Id))
                return StatusCode(403);

            var cultureFR = CultureInfo.CurrentCulture.Name.Contains("fr");
            _unitOfWork.CreateTransaction();
            try
            {
                var result = _client.AcceptRequest(request);
                if (result)
                {
                    var message = cultureFR ? "Demande acceptée avec succès."
                    : "Request accepted successfully.";

                    _unitOfWork.CommitTransaction();

                    return Json(new { status = true, message });
                }
                else
                {
                    _unitOfWork.RollbackTransaction();
                    var message = cultureFR ? "L'opération a échoué. Veuillez réessayer s'il vous plaît."
                    : "Operation failed. Try again, please.";

                    return Json(new { status = false, message });
                }
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                var message = cultureFR ? "Une erreur a été rencontrée. Veuillez réessayer plus tard."
                    : "An error was encountered. Please, try again later.";

                return Json(new { status = false, message });
            }
        }

        [Ajax(HttpVerb = "POST")]
        public IActionResult ValidateRequest(int id)
        {
            if (id <= 0)
                return StatusCode(403);

            var request = _client.GetRequestById(id);
            if (request is null)
                return StatusCode(404);

            var currentUserId = _userManager.GetUserId(User);

            if (!currentUserId.Equals(request.Receiver.Id) && !currentUserId.Equals(request.Requester.Id))
                return StatusCode(403);

            var cultureFR = CultureInfo.CurrentCulture.Name.Contains("fr");
            _unitOfWork.CreateTransaction();
            try
            {
                var direction = currentUserId.Equals(request.Requester.Id) ? 0 : 1;
                var result = _client.ValidateRequest(request, direction);
                if (result)
                {
                    _unitOfWork.CommitTransaction();
                    var message = cultureFR ? "Le service a été marqué comme rendu avec succès."
                    : "The service has been marked as rendered successfully.";

                    return Json(new { status = true, message });
                }
                else
                {
                    _unitOfWork.RollbackTransaction();
                    var message = cultureFR ? "L'opération a échoué. Veuillez réessayer s'il vous plaît."
                    : "Operation failed. Try again, please.";

                    return Json(new { status = false, message });
                }
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                var message = cultureFR ? "Une erreur a été rencontrée. Veuillez réessayer plus tard."
                    : "An error was encountered. Please, try again later.";

                return Json(new { status = false, message });
            }
        }

        [Ajax(HttpVerb = "POST")]
        public async Task<IActionResult> RejectRequest(int id)
        {
            if (id <= 0)
                return StatusCode(403);

            var request = _client.GetRequestById(id);
            if (request is null)
                return StatusCode(404);

            var currentUserId = _userManager.GetUserId(User);

            if (!currentUserId.Equals(request.Receiver.Id))
                return StatusCode(403);

            var cultureFR = CultureInfo.CurrentCulture.Name.Contains("fr");
            _unitOfWork.CreateTransaction();
            try
            {
                var direction = currentUserId.Equals(request.Requester.Id) ? 0 : 1;
                var result = _client.RejectRequest(request);
                if (result)
                {
                    var currentUser = await _userManager.FindByIdAsync(request.Requester.Id);
                    currentUser.Point += request.Point;
                    await _userManager.UpdateAsync(currentUser);
                    _unitOfWork.CommitTransaction();
                    var message = cultureFR ? "Le service a été rejeté avec succès."
                    : "The service has been rejected successfully.";

                    return Json(new { status = true, message });
                }
                else
                {
                    _unitOfWork.RollbackTransaction();
                    var message = cultureFR ? "L'opération a échoué. Veuillez réessayer s'il vous plaît."
                    : "Operation failed. Try again, please.";

                    return Json(new { status = false, message });
                }
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                var message = cultureFR ? "Une erreur a été rencontrée. Veuillez réessayer plus tard."
                    : "An error was encountered. Please, try again later.";

                return Json(new { status = false, message });
            }
        }

        public IActionResult RefreshReceivedView()
        {
            var currentUserId = _userManager.GetUserId(User);
            var user = _client.UserRequests(currentUserId);

            return PartialView("_ReceivedRequests", user);
        }

        public IActionResult RefreshSentView()
        {
            var currentUserId = _userManager.GetUserId(User);
            var user = _client.UserRequests(currentUserId);

            return PartialView("_SentRequests", user);
        }
    }
}