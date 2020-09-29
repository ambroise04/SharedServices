using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharedServices.BL.UseCases.Admin;
using SharedServices.BL.UseCases.Clients;
using SharedServices.DAL;
using SharedServices.DAL.UnitOfWork;
using SharedServices.UI.Attributes;
using System;
using System.Globalization;

namespace SharedServices.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EnterpriseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Client _client;
        private readonly Adminitrator _admin;
        private readonly string _culture;

        public EnterpriseController(
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
        public IActionResult Index()
        {
            var infos = _unitOfWork.GlobalInfoRepository.GetInfo();
            return View(infos);
        }

        [Ajax(HttpVerb = "POST")]
        public IActionResult EditEmail(string email)
        {
            var globalInfos = _unitOfWork.GlobalInfoRepository.GetInfo();
            globalInfos.Email = email;
            _unitOfWork.CreateTransaction();
            try
            {
                _unitOfWork.GlobalInfoRepository.Update(globalInfos);
                _unitOfWork.CommitTransaction();
                return Json(new { status = true, Message = "Email modifié avec succès." });
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                return Json(new { status = false, Message = $"La modification de l'email a échoué [{ex.Message}]" });
            }
        }

        [Ajax(HttpVerb = "POST")]
        public IActionResult EditAddress(string addressFR, string addressEN)
        {
            var globalInfos = _unitOfWork.GlobalInfoRepository.GetInfo();
            globalInfos.AddressFR = addressFR;
            globalInfos.AddressEN = addressEN;
            _unitOfWork.CreateTransaction();
            try
            {
                _unitOfWork.GlobalInfoRepository.Update(globalInfos);
                _unitOfWork.CommitTransaction();
                return Json(new { status = true, Message = "Adresse modifiée avec succès." });
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                return Json(new { status = false, Message = $"La modification de l'adresse a échoué [{ex.Message}]" });
            }
        }

        [Ajax(HttpVerb = "POST")]
        public IActionResult EditDesc(string descFR, string descEN)
        {
            var globalInfos = _unitOfWork.GlobalInfoRepository.GetInfo();
            globalInfos.DescriptionFR = descFR;
            globalInfos.DescriptionEN = descEN;
            _unitOfWork.CreateTransaction();
            try
            {
                _unitOfWork.GlobalInfoRepository.Update(globalInfos);
                _unitOfWork.CommitTransaction();
                return Json(new { status = true, Message = "Description modifiée avec succès." });
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                return Json(new { status = false, Message = $"La modification de la description a échoué [{ex.Message}]" });
            }
        }

        [Ajax(HttpVerb = "POST")]
        public IActionResult EditPhone(string phone)
        {
            var globalInfos = _unitOfWork.GlobalInfoRepository.GetInfo();
            globalInfos.Phone = phone;
            _unitOfWork.CreateTransaction();
            try
            {
                _unitOfWork.GlobalInfoRepository.Update(globalInfos);
                _unitOfWork.CommitTransaction();
                return Json(new { status = true, Message = "Téléphone modifié avec succès." });
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                return Json(new { status = false, Message = $"La modification du numéro de téléphone a échoué [{ex.Message}]" });
            }
        }

        [Ajax(HttpVerb = "POST")]
        public IActionResult EditPoint(int point)
        {
            var globalInfos = _unitOfWork.GlobalInfoRepository.GetInfo();
            globalInfos.DefaultPointForUsers = point;
            _unitOfWork.CreateTransaction();
            try
            {
                _unitOfWork.GlobalInfoRepository.Update(globalInfos);
                _unitOfWork.CommitTransaction();
                return Json(new { status = true, Message = "Nombre de points par défaut modifié avec succès." });
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                return Json(new { status = false, Message = $"La modification du nombre de points par défaut a échoué [{ex.Message}]" });
            }
        }

        [Ajax(HttpVerb = "POST")]
        public IActionResult EditAuthorInfos(string link)
        {
            var globalInfos = _unitOfWork.GlobalInfoRepository.GetInfo();
            globalInfos.AuthorLink = link;
            _unitOfWork.CreateTransaction();
            try
            {
                _unitOfWork.GlobalInfoRepository.Update(globalInfos);
                _unitOfWork.CommitTransaction();
                return Json(new { status = true, Message = "Lien d'auteur modifié avec succès." });
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                return Json(new { status = false, Message = $"La modification du lien d'auteur a échoué [{ex.Message}]" });
            }
        }
    }
}