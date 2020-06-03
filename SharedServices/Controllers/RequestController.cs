using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedServices.BL.Domain;
using SharedServices.BL.Extensions;
using SharedServices.BL.UseCases.Admin;
using SharedServices.BL.UseCases.Clients;
using SharedServices.DAL;
using SharedServices.DAL.UnitOfWork;
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
    public class RequestController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBroadcastEmailSender _broadcastEmailSender;
        private readonly Client _client;
        private readonly Adminitrator _admin;
        private readonly string _culture;


        public RequestController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IBroadcastEmailSender broadcastEmailSender)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _broadcastEmailSender = broadcastEmailSender;
            _client = new Client(_unitOfWork, _userManager);
            _admin = new Adminitrator(_unitOfWork);
            _culture = CultureInfo.CurrentCulture.Name;
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
            catch (Exception)
            {
                _unitOfWork.RollbackTransaction();
                var errorMessage = cultureFR ? "Un problème a été rencontré! Veuillez réessayer s'il vous plaît."
                    : "A problem has been encountered! Try again, please.";
                return Json(new { status = false, message = errorMessage });
            }
        }
        
        public IActionResult All()
        {
            var userId = _userManager.GetUserId(User);
            var requests = _client.GetNotAcceptedRequestMulticasts(userId);
            return View(requests);
        }
        
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Multicast()
        {
            var services = _admin.GetAllServicesGrouped();
            ViewBag.Culture = _culture;
            return View(services);
        }

        // POST: Request/Multicast
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Multicast(int service, string date)
        {
            var cultureFR = CultureInfo.CurrentCulture.Name.Contains("fr");
            if (service <= 0)
            {
                var errorMessage = cultureFR ? "Une erreur liée aux données a été rencontré! Veuillez réessayer s'il vous plaît."
                    : "Parameter related errors have been encountered! Try again, please.";
                return Json(new { status = false, message = errorMessage });
            }

            var retrievedService = _client.GetServiceById(service);
            var requester = _userManager.GetUserAsync(User).Result;

            _unitOfWork.CreateTransaction();
            try
            {
                date = cultureFR ? date : string.Join('-', date?.Split("/"));
                var requestDate = !string.IsNullOrEmpty(date) ? DateTime.Parse(date) : DateTime.MinValue;
                var request = new RequestMulticast
                {
                    Accepted = false,
                    DateOfAddition = DateTime.Now,
                    DateOfRequest = requestDate,
                    Service = retrievedService,
                    RequesterMulticast = requester,
                    Point = retrievedService.Group.PointsByHour
                };

                var result = _client.AddRequestMulticast(request);
                if (result != null)
                {
                    var successMessage = cultureFR ? "Demande publiée avec succès."
                        : "Request broadcasted successfully";
                    await Broadcast(result);
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

        public async Task Broadcast(RequestMulticast request)
        {
            var serviceId = request.Service.Id;
            var userEmails = _userManager.Users
                                    .Include(u => u.UserServices)
                                    .ThenInclude(us => us.Service)
                                    .Where(u => u.UserServices.Any(s => s.ServiceId == serviceId) && !u.Id.Equals(request.RequesterMulticast.Id))
                                    .Select(u =>  u.Email)
                                    .ToList();
            string requestUrl = "#";
            string message = $"Cher(ère) abonné(e), <br /> " +
                             $"Une nouvelle demande de service a été publiée : " +
                             $"<bold><em>{request.Service.Title}</em></bold> <br /> " +
                             $"Soyez l'un des premiers à répondre.<br />" +
                             $"<a href=\"{requestUrl}\" class=\"btn btn-primary waves-effect form-control white-text\">Répondre</a>";

            await _broadcastEmailSender.SendEmailAsync(userEmails, "Nouvelle demande de service", message);
        }
    }
}