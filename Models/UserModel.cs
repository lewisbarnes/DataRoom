using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataRoom.Models
{
    public class UserModel
    {
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Display(Name = "Passcode")]
        public string Password { get; set; }
    }
}
