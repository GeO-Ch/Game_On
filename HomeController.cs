using GameON.Persistence;
using GameON.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Provider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameON.Core.IRepositories;

namespace GameON.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;
                
        public HomeController()
        {
            
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());           
        }
        public ActionResult Index(string query = null)
        {
            var upcomingTournaments = _unitOfWork.Tournaments.GetUpcomingTournaments();

            if (!string.IsNullOrEmpty(query))
            {
                upcomingTournaments = upcomingTournaments
                    .Where(t =>
                t.Title.Contains(query) ||
                t.Host.Name.Contains(query) ||
                t.Game.Title.Contains(query));
            }

            var userId = User.Identity.GetUserId();
            var participations = _unitOfWork.Participations.GetFutureParticipations(userId)
                .ToLookup(p => p.TournamentId);

            var viewModel = new TournamentViewModel
            {
                UpcomingTournaments = upcomingTournaments,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Upcoming Tournaments",
                SearchTerm = query,
                Participations = participations

            };
            return View("Tournaments", viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}