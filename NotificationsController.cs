using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using AutoMapper;
using GameON.Core.Dtos;
using GameON.Core.IRepositories;
using GameON.Core.Models;
using GameON.Persistence;

namespace GameON.Controllers.API
{
    public class NotificationsController : ApiController
    {
        
        private readonly IUnitOfWork _unitOfWork;
        public NotificationsController()
        {

            _unitOfWork = new UnitOfWork(new ApplicationDbContext());

        }
        [HttpPost]
        public IHttpActionResult MarkAsRead() // returns status 500
        {
            var userId = User.Identity.GetUserId();
            var notifications = _unitOfWork.UserNotifications.GetUserNotificationsUnRead(userId);
            notifications.ForEach(n => n.Read());
            _unitOfWork.Complete();
            return Ok();
        }

        public IEnumerable<NotificationDto> GetNotifications() 
        {
            var userId = User.Identity.GetUserId();
            var notifications = _unitOfWork.Notifications.GetNotificationsByUser(userId);

            return notifications.Select(Mapper.Map<Notification, NotificationDto>);
            #region Manual Mapping
            //return notifications.Select(n => new NotificationDto() 
            //{

            //    DateTime = n.DateTime,
            //    Type = n.Type,
            //    OriginalDateTime = n.OriginalDateTime,
            //    OriginalVenue = n.OriginalVenue,
            //    OriginalTitle = n.OriginalTitle,
            //    Tournament = new TournamentDto 
            //    {
            //        Host = new UserDto 
            //        {
            //            Id = n.Tournament.Host.Id,
            //            Name = n.Tournament.Host.Name,


            //        },

            //        DateTime = n.Tournament.DateTime,
            //        Id = n.Tournament.Id,
            //        Title = n.Tournament.Title,
            //        Description = n.Tournament.Description,
            //        IsCancelled = n.Tournament.IsCancelled,
            //        Venue = n.Tournament.Venue
            //    }
            //});

            #endregion

        }
    }
}
