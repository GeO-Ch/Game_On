using GameON.Persistence;
using GameON.ViewModels;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Provider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime;
using System.Web;
using System.Web.Mvc;
using GameON.Core.IRepositories;
using GameON.Core.Models;

namespace GameON.Controllers
{
    public class TournamentController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;
        public TournamentController()
        {
            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        public ActionResult Search(TournamentViewModel tour)
        {
            return RedirectToAction("Index", "Home", new { query = tour.SearchTerm });
        }
        public ActionResult Details(int id)
        {
            var tournament = _unitOfWork.Tournaments.GetTournament(id);



            var viewModel = new TournamentDetailsViewModel
            {
                Tournament = tournament
            };
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                viewModel.IsParticipating = _unitOfWork.Participations.GetParticipation(tournament.Id, userId) != null;
            }
            return View("Details", viewModel);
        }

        // GET: Tournament
        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var tours = _unitOfWork.Tournaments.GetUpcomingTournamentsByHost(userId);

            return View(tours);
        }
        [Authorize]
        public ActionResult Participating()
        {
            var userId = User.Identity.GetUserId();
            var tours = _unitOfWork.Tournaments.GetTournamentsGamerParticipating(userId);

            var participations = _unitOfWork.Participations.GetFutureParticipations(userId).ToLookup(p => p.TournamentId);

            var viewModel = new TournamentViewModel
            {
                UpcomingTournaments = tours,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Tournaments I attend",
                Participations = participations

            };

            return View("Tournaments", viewModel);
        }

        [Authorize]
        public ActionResult Create()
        {

            var viewModel = new TournamentFormViewModel
            {
                Heading = "Add a Tournament",
                Games = _unitOfWork.Games.GetAllGames()
            };
            return View("TournamentForm", viewModel);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var tournament = _unitOfWork.Tournaments.GetTournament(id);
            if (tournament is null)
                return HttpNotFound();
            if (tournament.HostId != userId)
                return new HttpUnauthorizedResult();

            var viewModel = new TournamentFormViewModel
            {
                Id = tournament.Id,
                Date = tournament.DateTime.ToString("d MMM yyyy"),
                Time = tournament.DateTime.ToString("HH:mm"),
                Venue = tournament.Venue,
                Description = tournament.Description,
                GameId = tournament.GameId,
                Games = _unitOfWork.Games.GetAllGames(),
                Heading = "Edit Tournament"
            };
            return View("TournamentForm", viewModel);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TournamentFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Games = _unitOfWork.Games.GetAllGames();
                return View("TournamentForm", viewModel);
            }

            var tournament = new Tournament
            {
                HostId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GameId = viewModel.GameId,
                Description = viewModel.Description,
                Title = viewModel.Title,
                Venue = viewModel.Venue
            };

            _unitOfWork.Tournaments.Add(tournament);
            _unitOfWork.Complete();
            return RedirectToAction("Mine", "Tournament");
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(TournamentFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Games = _unitOfWork.Games.GetAllGames();
                return View("TournamentForm", viewModel);
            }

            var userId = User.Identity.GetUserId();
            var tournament = _unitOfWork.Tournaments.GetTournamentWithParticipants(viewModel.Id);
            if (tournament is null)
                return HttpNotFound();

            if (tournament.HostId != userId)
                return new HttpUnauthorizedResult();

            tournament.Modify(viewModel.Title, viewModel.Description, viewModel.GameId, viewModel.GetDateTime(), viewModel.Venue);
            _unitOfWork.Complete();

            return RedirectToAction("Mine", "Tournament");
        }




    }
}