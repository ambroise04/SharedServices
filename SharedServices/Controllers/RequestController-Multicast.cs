using Geolocation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedServices.BL.Domain;
using SharedServices.Mutual;
using SharedServices.Mutual.Enumerations;
using SharedServices.UI.Attributes;
using SharedServices.UI.Models;
using SharedServices.UI.Models.RequestMulticastViewModels;
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
        [AllowAnonymous]
        public async Task<IActionResult> All(int? pageIndex, SearchOptions search, double latitude, double longitude)
        {
            var userId = _userManager.GetUserId(User);
            var user = _userManager.Users.Include(u => u.UserServices).FirstOrDefault(u => u.Id.Equals(userId));
            var coordinate = new Coordinate { Latitude = latitude, Longitude = longitude };
            PaginatedRequests<RequestMulticast> data;
            try
            {
                var requests = _client.GetNotAcceptedRequestMulticasts(user, search, coordinate).AsQueryable();
                data = await PaginatedRequests<RequestMulticast>.CreateAsync(requests, pageIndex ?? 1, 9);

                return View(data);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
                //return View("Error", nameof(HomeController), new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        [AllowAnonymous]
        [Ajax(HttpVerb = "GET")]
        public async Task<IActionResult> Filter(int? pageIndex, SearchOptions search, double latitude, double longitude)
        {
            var userId = _userManager.GetUserId(User);
            var user = _userManager.Users.Include(u => u.UserServices).FirstOrDefault(u => u.Id.Equals(userId));
            var coordinate = new Coordinate { Latitude = latitude, Longitude = longitude };
            var requests = _client.GetNotAcceptedRequestMulticasts(user, search, coordinate).AsQueryable();
            var data = await PaginatedRequests<RequestMulticast>.CreateAsync(requests, pageIndex ?? 1, 9);

            return PartialView("_AllRequestMulticast", data);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Multicast()
        {
            var services = _admin.GetAllServicesGrouped();
            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.Culture = _culture;
            ViewBag.Points = currentUser.Point;
            return View(services);
        }

        // POST: Request/Multicast
        [Ajax(HttpVerb = "POST")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Multicast(CreateMulticastVM request)
        {
            var cultureFR = CultureInfo.CurrentCulture.Name.Contains("fr");
            if (request.Service <= 0)
            {
                var errorMessage = cultureFR ? "Une erreur liée aux données a été rencontrée! Veuillez réessayer s'il vous plaît."
                    : "Parameter related errors have been encountered! Try again, please.";
                return Json(new { status = false, message = errorMessage });
            }

            var retrievedService = _client.GetServiceById(request.Service);
            var requester = _userManager.GetUserAsync(User).Result;

            _unitOfWork.CreateTransaction();
            try
            {
                request.Date = cultureFR ? request.Date : string.Join('-', request.Date?.Split("/"));
                var requestDate = !string.IsNullOrEmpty(request.Date) ? DateTime.Parse(request.Date) : DateTime.MinValue;
                var place = new Place
                {
                    City = request.City,
                    Country = request.Country,
                    PostalCode = request.PostalCode,
                    Latitude = double.Parse(request.Lat.Replace('.', ',')),
                    Longitude = double.Parse(request.Lng.Replace('.', ','))
                };
                var newRequest = new RequestMulticast
                {
                    Accepted = false,
                    DateOfAddition = DateTime.Now,
                    DateOfRequest = requestDate,
                    Service = retrievedService,
                    RequesterMulticast = requester,
                    Point = request.Point,
                    Place = place,
                    Duration = request.Duration,
                    Description = request.Description
                };

                var result = _client.AddRequestMulticast(newRequest);
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
            catch (Exception)
            {
                _unitOfWork.RollbackTransaction();
                var errorMessage = cultureFR ? "Un problème a été rencontré! Veuillez réessayer s'il vous plaît."
                    : "A problem has been encountered! Try again, please.";
                return Json(new { status = false, message = errorMessage });
            }
        }

        [Ajax(HttpVerb = "GET")]
        public IActionResult Postulate(int request)
        {
            var cultureFR = CultureInfo.CurrentCulture.Name.Contains("fr");
            if (request <= 0)
            {
                var errorMessage = cultureFR ? "Une erreur liée aux données a été rencontrée! Veuillez réessayer s'il vous plaît."
                    : "Parameter related errors have been encountered! Try again, please.";
                return Json(new { status = false, message = errorMessage });
            }

            var retrievedRequest = _client.GetRequestMulticastById(request);
            if (retrievedRequest != null)
            {
                var currentUser = _userManager.GetUserAsync(User).Result;
                if (retrievedRequest.Responses != null && retrievedRequest.Responses.Count() != 0)
                {
                    if (retrievedRequest.Responses.Any(r => r.Responder.Id.Equals(currentUser.Id)))
                    {
                        var errorMessage = cultureFR ? "Vous avez déjà postulé pour cette demande. Vous aurez une réponse très bientôt."
                         : "You have already applied for this request. Please wait for a response from the publisher";
                        return Json(new { status = false, message = errorMessage });
                    }
                }

                var response = new DAL.Entities.ResponseMulticastRequest
                {
                    RequestMulticastId = retrievedRequest.Id,
                    ApplicationUserId = currentUser.Id,
                    Choosen = false
                };

                if (retrievedRequest.Responses is null)
                    retrievedRequest.Responses = new List<ResponseMulticastRequest>();

                _unitOfWork.CreateTransaction();
                try
                {
                    if (currentUser.Responses is null)
                        currentUser.Responses = new List<DAL.Entities.ResponseMulticastRequest>();
                    currentUser.Responses.Add(response);
                    _unitOfWork.CommitTransaction();
                    var successMessage = cultureFR ? "Merci pour votre réponse à cette demande."
                     : "Thank you for your response to this request.";
                    return Json(new { status = true, message = successMessage });
                }
                catch (Exception ex)
                {
                    _unitOfWork.RollbackTransaction();
                    var errorMessage = cultureFR ? "Un problème a été rencontré! Veuillez réessayer s'il vous plaît."
                        : "A problem has been encountered! Try again, please.";
                    return Json(new { status = false, message = errorMessage });
                }
            }

            var message = cultureFR ? "Un problème a été rencontré! Veuillez réessayer s'il vous plaît."
                        : "A problem has been encountered! Try again, please.";
            return Json(new { status = false, message });
        }

        public IActionResult MyRequests()
        {
            var userId = _userManager.GetUserId(User);
            var user = _userManager.Users
                                   .Include(u => u.RequestMulticasts)
                                   .ThenInclude(rm => rm.Responses)
                                   .ThenInclude(r => r.Responder)
                                   .ThenInclude(resp => resp.Picture)
                                   .Include(u => u.RequestMulticasts)
                                   .ThenInclude(rm => rm.Service)
                                   .Include(u => u.RequestMulticasts)
                                   .ThenInclude(rm => rm.Place)
                                   .FirstOrDefault(u => u.Id.Equals(userId));
            return View(user);
        }

        [Ajax(HttpVerb = "POST")]
        public IActionResult Choose(int request, string target)
        {
            var cultureFR = CultureInfo.CurrentCulture.Name.Contains("fr");
            if (request <= 0 || string.IsNullOrEmpty(target))
            {
                var errorMessage = cultureFR ? "Une erreur liée aux données a été rencontrée! Veuillez réessayer s'il vous plaît."
                    : "Parameter related errors have been encountered! Try again, please.";
                return Json(new { status = false, message = errorMessage });
            }

            var retrievingRequest = _client.GetRequestMulticastById(request);
            var currentUser = _userManager.GetUserAsync(User).Result;
            if (currentUser.Point < retrievingRequest.Point)
            {
                var errorMessage = cultureFR ? "Nous n'avez pas assez de points pour cette demande."
                    : "You don't have enough points for this request.";
                return Json(new { status = false, message = errorMessage });
            }

            var choosenUser = _userManager.Users.AsNoTracking().FirstOrDefault(u => u.Id.Equals(target));
            if (retrievingRequest == null || choosenUser == null)
            {
                var errorMessage = cultureFR ? "De mauvaises données a été envoyées! Veuillez réessayer s'il vous plaît."
                    : "Bad data have been sent! Try again, please.";
                return Json(new { status = false, message = errorMessage });
            }
            _unitOfWork.CreateTransaction();
            try
            {
                retrievingRequest.Accepted = true;
                var updatedRequest = _client.UpdateRequestMulticastForChoice(retrievingRequest);

                var user = _userManager.Users
                                       .Include(u => u.RequestMulticasts)
                                       .ThenInclude(rm => rm.Responses)
                                       .FirstOrDefault(u => u.Id.Equals(target));
                var response = user.Responses.First(r => r.RequestMulticastId == retrievingRequest.Id);
                response.Choosen = true;
                var updatedUser = _userManager.UpdateAsync(user).Result;

                var create = CreateRequest(requestM: retrievingRequest, flag: choosenUser.Id, place: updatedRequest.Place);
                if (create)
                {                    
                    if (!_userManager.IsInRoleAsync(currentUser, Roles.Admin.ToString()).Result)
                    {
                        currentUser.Point -= retrievingRequest.Point;
                        _userManager.UpdateAsync(currentUser);
                    }
                    _unitOfWork.CommitTransaction();
                    var successMessage = cultureFR ? "Votre choix a été enregistré avec succes."
                        : "Your choice has been successfully saved.";
                    return Json(new { status = true, message = successMessage });
                }
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                var errorMessage = cultureFR ? "Un problème a été rencontré! Veuillez réessayer s'il vous plaît."
                            : "A problem has been encountered! Try again, please.";
                return Json(new { status = false, message = errorMessage });
            }
            var message = cultureFR ? "Un problème a été rencontré! Veuillez réessayer s'il vous plaît."
                            : "A problem has been encountered! Try again, please.";
            return Json(new { status = false, message });
        }

        private bool CreateRequest(RequestMulticast requestM, string flag, Place place)
        {
            var cultureFR = CultureInfo.CurrentCulture.Name.Contains("fr");

            if (string.IsNullOrEmpty(flag) || string.IsNullOrEmpty(requestM.DateOfRequest.ToString()) || requestM.Service.Id <= 0)
            {
                return false;
            }
            var date = cultureFR ? requestM.DateOfRequest.ToString() : string.Join('-', requestM.DateOfRequest.ToString().Split("/"));

            var requestDate = DateTime.Parse(date);
            var serviceRetrieved = _client.GetServiceById(requestM.Service.Id);
            var receiver = _userManager.FindByIdAsync(flag).Result;
            var user = _userManager.GetUserAsync(User).Result;

            if (receiver.Id.Equals(user.Id))
            {
                return false;
            }
            Place newPlace = place;
            newPlace.Id = 0;
            var request = new Request
            {
                Service = serviceRetrieved,
                Point = requestM.Point,
                Duration = requestM.Duration,
                State = RequestStates.Accepted,
                DateOfAddition = DateTime.Now,
                DateOfRequest = requestDate,
                Requester = user,
                Receiver = receiver,
                Source = RequestSource.Broadcast,
                Place = newPlace
            };

            try
            {
                var result = _client.AddRequest(request);
                if (request != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task Broadcast(RequestMulticast request)
        {
            var serviceId = request.Service.Id;
            var userEmails = _userManager.Users
                                    .Include(u => u.UserServices)
                                    .ThenInclude(us => us.Service)
                                    .Where(u => u.UserServices.Any(s => s.ServiceId == serviceId) && !u.Id.Equals(request.RequesterMulticast.Id))
                                    .Select(u => u.Email)
                                    .ToList();
            string requestUrl = "#";
            string message = $"Cher(ère) abonné(e), <br /><br /> " +
                             $"Une nouvelle demande de service a été publiée : " +
                             $"<bold><em>{request.Service.Title}</em></bold> <br /> " +
                             $"Soyez l'un des premiers à répondre.<br /><br />" +
                             $"<a href=\"{requestUrl}\" class=\"btn btn-primary waves-effect form-control white-text\">Répondre</a>";

            await _broadcastEmailSender.SendEmailAsync(userEmails, "Nouvelle demande de service", message);
        }
    }
}