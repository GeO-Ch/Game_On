using GameON.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using GameON.Core.Models;
using GameON.ViewModels;

namespace GameON.ViewModels
{
    public class TournamentFormViewModel
    {
        
        [Required]
        public int Id { get; set; }
        public string Heading { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Venue { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [FutureDate]
        public string Date { get; set; }
        [Required]
        [ValidTime]
        public string Time { get; set; }
        [Required]
        [Display(Name = "Game")]
        public int GameId { get; set; }
        [Display(Name = "Photo")]
        public string ImagePath { get; set; }
        public IEnumerable<Game> Games { get; set; }
        
        public string Action 
        {
            get
            {
                //return (Id != 0) ? "Update" : "Create";
                Expression<Func<TournamentController, ActionResult>> update = (c => c.Update(this));
                Expression<Func<TournamentController, ActionResult>> create = (c => c.Create(this));

                var action = (Id != 0) ? update : create;
                var actionName = (action.Body as MethodCallExpression).Method.Name;

                return actionName;
            }
        }



        public DateTime GetDateTime()
        {
             return  DateTime.Parse(string.Format("{0} {1}", Date, Time));
        }
    }
}