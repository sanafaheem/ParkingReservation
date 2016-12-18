using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class LoginModel
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "The email address is not valid")]
        public string Email { set; get; }
        [Display(Name="Password")]
        [Required(ErrorMessage ="Password is required")]
        [DataType(DataType.Password)]
        public string Password { set; get; }

    }
}