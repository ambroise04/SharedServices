using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class SearchController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private Client _client;

        public SearchController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _client = new Client(_unitOfWork);
        }

        // GET: Service
        public async Task<ActionResult> Index(int? pageIndex, string search, int? order)
        {
            var pageSize = 8;
            List<ApplicationUser> applicationUsers;
            _client = new Client(_unitOfWork, _userManager);
            if (!string.IsNullOrEmpty(search))
            {
                applicationUsers = _client.SearchResult(search);
            }
            else
            {
                applicationUsers = _client.SearchAllUsers();
            }
            var users = await PaginatedList<ApplicationUser>.CreateAsync(applicationUsers.AsQueryable().AsNoTracking(), pageIndex ?? 1, pageSize);
            var model = new SearchModel
            {
                Users = users
            };

            if (!string.IsNullOrEmpty(search))
            {                
                var serviceId = _client.GetServiceByTitle(search);
                ViewBag.Service = serviceId;
            }
            ViewBag.Search = search;
            ViewBag.Culture = CultureInfo.CurrentCulture.Name;
            return View(model);
        }

        public string[] Services()
        {
            return _client.GetAllServices()
                        .Select(s => s.Title)
                        .ToArray();
        }

        // GET: Service/Details/5
        [HttpGet]
        public ActionResult Details(string id, int flag)
        {
            var user = _userManager.Users
                                   .Include(u => u.Picture)
                                   .Include(u => u.Feedbacks)
                                   .Include(u => u.UserServices)
                                   .ThenInclude(us => us.Service)
                                   .FirstOrDefault(u => u.Id.Equals(id));
            if (flag > 0)
            {
                var service = _client.GetServiceById(flag);
                ViewBag.Service = service.Id;
            }
            return View(user);
        }
    }
}