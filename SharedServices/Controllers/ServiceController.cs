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
    }
}