using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class Account
    {
        [DataType(DataType.EmailAddress)]
        public string Email { set; get; }
        [DataType(DataType.Password)]
        public string Password { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { set; get; }

       
        
    }
}