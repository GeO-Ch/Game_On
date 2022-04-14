using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameON.Core.Models;

namespace GameON.ViewModels
{
    public class TournamentViewModel
    {
        //public int Id { get; set; }
        public IEnumerable<Tournament> UpcomingTournaments { get; set; }
        public bool ShowActions { get; set; }
        public string Heading { get; set; }
        public string SearchTerm { get; set; }
        public ILookup<int, Participation> Participations { get; set; }
    }
}