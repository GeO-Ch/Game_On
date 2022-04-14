using GameON.Persistence;
using GameON.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameON.Core.Models;

namespace GameON.Controllers
{

    [Authorize]
    public class CreditCardController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        public CreditCardController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        
        public ActionResult SubscribeUser()
        {
            var userId = User.Identity.GetUserId();
            var creditCard = _unitOfWork.CreditCards.GetCreditCard(userId);
            creditCard.PaySubsciption(50);
            _unitOfWork.Complete();
            return RedirectToAction("Index", "Home");
        }

        // GET: CreditCard
        public ActionResult Subscribe()
        {
            var userId = User.Identity.GetUserId();
            var creditCard = _unitOfWork.CreditCards.GetCreditCard(userId);
            if (creditCard.User is null)
            return RedirectToAction("Index", "Home");
            return View(creditCard);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreditCardFormViewModel viewModel)
        {
            var userId = User.Identity.GetUserId();

            var creditCard = new CreditCard(userId, viewModel.CardNumber, viewModel.GetDateTime(), viewModel.CCV);

            _unitOfWork.CreditCards.Add(creditCard);
            _unitOfWork.Complete();
            creditCard = _unitOfWork.CreditCards.GetCreditCard(userId);
            return View("Subscribe", creditCard);
        }
    }
}