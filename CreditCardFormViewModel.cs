using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameON.ViewModels
{
    public class CreditCardFormViewModel
    {
        public string UserId { get; set; }
        [Required]
        public string CardNumber { get; set; }
        [Required]
        [FutureDate]
        public string ExpireDate { get; set; }
        [Required]
        public short CCV { get; set; }

        public DateTime GetDateTime()
        {
            return DateTime.Parse(string.Format(ExpireDate));
        }
    }
}