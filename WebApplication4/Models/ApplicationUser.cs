using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class ApplicationUser
    {
        public string Email { set; get; }
        public bool IsEmailConfirmed { set; get;}
    }
}