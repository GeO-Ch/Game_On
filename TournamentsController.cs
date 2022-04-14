using GameON.Persistence;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using GameON.Core.IRepositories;

namespace GameON.Controllers.API
{
    [Authorize]
    public class TournamentsController : ApiController
    {
        
        private readonly IUnitOfWork _unitOfWork;
         public TournamentsController()
        {

            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }


        [HttpDelete]
        public IHttpActionResult Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            var tournament = _unitOfWork.Tournaments.GetTournamentWithParticipants(id);
            if (tournament is null)
                return NotFound();
            if (tournament.HostId != userId)
                return Unauthorized();

            if (tournament.IsCancelled)
                return NotFound();

            tournament.Cancel();

            _unitOfWork.Complete();
            
            return Ok();
        }

        
    }

}
