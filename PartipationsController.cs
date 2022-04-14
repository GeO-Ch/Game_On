using GameON.Dtos;
using GameON.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GameON.Controllers.API
{
    [Authorize]
    public class PartipationsController : ApiController
    {
        private ApplicationDbContext context;
        public PartipationsController()
        {
            context = new ApplicationDbContext();
        }
        [HttpPost]
        public IHttpActionResult Participate(ParticipationDto dto)
        {
            var userId = User.Identity.GetUserId();
            var exists = context.Participations.Any(p => p.GamerId == userId && p.TournamentId == dto.TournamentId);
            var participation = new Participation
            {
                GamerId = userId,
                TournamentId = dto.TournamentId
            };

            context.Participations.Add(participation);
            context.SaveChanges();
            return Ok();
        }
    }
}
