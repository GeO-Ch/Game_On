using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameON.Core.Models;

namespace GameON.ViewModels
{
    public class TournamentDetailsViewModel
    {
        public Tournament Tournament { get; set; }
        public bool IsParticipating { get; set; }
    }
}