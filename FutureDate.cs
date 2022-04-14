using GameON.Migrations;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace GameON.ViewModels
{
    public class FutureDate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dateTime;
            bool isValid = DateTime.TryParseExact(Convert.ToString(value), "d MMM yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTime);

            // isValid == true (correct parsing) AND dateTime of future Date
            return (isValid && dateTime > DateTime.Now); 
        }
    }
}