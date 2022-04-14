
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameON.Core.Dtos;
using GameON.Core.Models;

namespace GameON.App_Start
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserDto>();
            CreateMap<Tournament, TournamentDto>();
            CreateMap<Game, GameDto>();
            CreateMap<Notification, NotificationDto>();
        }
    }
}