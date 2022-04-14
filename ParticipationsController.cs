using GameON.Persistence;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GameON.Core.Dtos;
using GameON.Core.IRepositories;
using GameON.Core.Models;

namespace GameON.Controllers.API
{
    [Authorize]
    public class ParticipationsController : ApiController
    {
        
        private readonly IUnitOfWork _unitOfWork;
        public ParticipationsController()
        {

            _unitOfWork = new UnitOfWork(new ApplicationDbContext());
        }
        
        [HttpPost]
        public IHttpActionResult Participate(ParticipationDto dto)
        {
            var userId = User.Identity.GetUserId();
            var exists = _unitOfWork.Participations.GetParticipation(dto.TournamentId, userId) != null;
            if (exists)
                return BadRequest("The participation already exists!");
            var participation = new Participation
            {
                GamerId = userId,
                TournamentId = dto.TournamentId
            };

            _unitOfWork.Participations.Add(participation);
            _unitOfWork.Complete();
            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult DeleteParticipation(int id) 
        {
            var userId = User.Identity.GetUserId();
            var participation = _unitOfWork.Participations.GetParticipation(id, userId);

            if (participation == null)
                return NotFound();

            _unitOfWork.Participations.Remove(participation);
            _unitOfWork.Complete();

            return Ok(id);
        }
    }
}
