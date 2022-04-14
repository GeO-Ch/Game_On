using GameON.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameON.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Tournament> UpcomingTournaments { get; set; }
        public bool ShowActions { get; set; }
    }
}