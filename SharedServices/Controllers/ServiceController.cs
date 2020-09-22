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
using SharedServices.UI.Attributes;
using SharedServices.UI.Models.ServiceModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SharedServices.UI.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Client _client;
        private readonly Adminitrator _admin;

        public ServiceController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _client = new Client(_unitOfWork, _userManager);
            _admin = new Adminitrator(_unitOfWork);
        }
        public IActionResult MyServices()
        {
            var userId = _userManager.GetUserId(User);

            var model = new MyServicesViewModel
            {
                UserServices = _client.UserServices(userId),
                SelectServices = _client.GetAllServicesGrouped()
            };
            return View(model);
        }

        [Ajax(HttpVerb = "POST")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MyServices(ServiceViewModel model)
        {
            var cultureFR = CultureInfo.CurrentCulture.Name.Contains("fr");

            if (!ModelState.IsValid)
            {
                var errorMessage = cultureFR ? "Une erreur liée aux données a été rencontrée! Veuillez réessayer s'il vous plaît."
                    : "Parameter related errors have been encountered! Try again, please.";
                return Json(new { status = false, message = errorMessage });
            }

            var services = new List<Service>();
            foreach (var serviceId in model.Services)
            {
                services.Add(_client.GetServiceById(serviceId));
            }

            var currentUserId = _userManager.GetUserId(User);
            var currentUser = _userManager.Users
                                          .Include(u => u.UserServices)
                                          .FirstOrDefault(u => u.Id.Equals(currentUserId));

            currentUser.ManageRelationship(services, _admin);
            var result = await _userManager.UpdateAsync(currentUser);

            if (result.Succeeded)
            {
                var successMessage = cultureFR ? "Vos services ont été ajoutés avec succès"
                        : "Your services has been added successfully.";
                return Json(new { status = true, message = successMessage });
            }

            var error = cultureFR ? "Un problème a été rencontré. Veuillez réessayer s'il vous plaît"
                        : "A problem has been encountered. Please, try again.";

            return Json(new { status = false, message = error });
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            List<ServiceTO> services = _client.GetAllServicesGrouped();

            return View(services);
        }

        [Ajax(HttpVerb = "GET")]
        public IActionResult AddService(int id)
        {
            ViewBag.GroupId = id;
            List<ServiceGroup> categories = _client.GetServiceCategories();

            return PartialView("_AddService", categories);
        }

        [Authorize(Roles = "Admin")]
        [Ajax(HttpVerb = "POST")]
        [ValidateAntiForgeryToken]
        public IActionResult AddService(int group, string service, string description)
        {
            var cultureFR = CultureInfo.CurrentCulture.Name.Contains("fr");

            if (string.IsNullOrEmpty(service) || group <= 0)
            {
                var message = cultureFR ? "Des paramètres incorrects ont été envoyés. Veuillez vérifier vos données et réessayez s'il vous plaît."
                    : "Incorrect parameters have been sent.Please check your details and try again please";
                return Json(new { status = false, message });
            }

            var retrievedGroup = _client.GetGroupServiceById(group);
            if (retrievedGroup is null)
            {
                var message = cultureFR ? "Des paramètres incorrects ont été envoyés. Veuillez vérifier vos données et réessayez s'il vous plaît."
                    : "Incorrect parameters have been sent.Please check your details and try again please";
                return Json(new { status = false, message });
            }

            var newService = new Service
            {
                Title = service,
                Description = description
            };
            _unitOfWork.CreateTransaction();
            try
            {
                retrievedGroup.Services.Add(newService);
                var result = _client.UpdateGroupService(retrievedGroup);

                if (result is null)
                {
                    _unitOfWork.RollbackTransaction();
                    var message = cultureFR ? "Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît."
                        : "An error has been encountered. Try again, please.";
                    return Json(new { status = false, message });
                }
                _unitOfWork.CommitTransaction();
                var successMessage = cultureFR ? "Votre service a été ajouté avec succès." : "Your service has been added successfully.";

                return Json(new { status = true, message = successMessage});
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                var message = cultureFR ? "Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît."
                    : "An error has been encountered. Try again, please.";

                return Json(new { status = false, message });
            }
        }

        public IActionResult AddCategory()
        {
            return PartialView("_AddGroup");
        }

        [Authorize(Roles = "Admin")]
        [Ajax(HttpVerb = "POST")]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategory(string title, int point)
        {
            var cultureFR = CultureInfo.CurrentCulture.Name.Contains("fr");

            if (string.IsNullOrEmpty(title) || point <= 0)
            {
                var message = cultureFR ? "Des paramètres incorrects ont été envoyés. Veuillez vérifier vos données et réessayez s'il vous plaît."
                    : "Incorrect parameters have been sent.Please check your details and try again please";
                return Json(new { status = false, message });
            }

            var newGroup = new ServiceGroup
            {
                Title = title,
                PointsByHour = point
            };
            _unitOfWork.CreateTransaction();
            try
            {
                var result = _client.AddGroupService(newGroup);

                if (result is null)
                {
                    _unitOfWork.RollbackTransaction();
                    var message = cultureFR ? "Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît."
                        : "An error has been encountered. Try again, please.";
                    return Json(new { status = false, message });
                }
                _unitOfWork.CommitTransaction();
                var successMessage = cultureFR ? "La catégorie de service a été ajoutée avec succès." : "The service category has been added successfully.";

                return Json(new { status = true, message = successMessage });
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                var message = cultureFR ? "Une erreur a été rencontrée. Veuillez réessayer s'il vous plaît."
                    : "An error has been encountered. Try again, please.";

                return Json(new { status = false, message });
            }
        }
    }
}