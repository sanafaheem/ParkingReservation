using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class Account
    {   
        public int ID { set; get; }
        public string Email { set; get; }
        
        public string Password { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
       
        public string PhoneNumber { set; get; }
        public bool IsEmailConfirmed { set; get; }
        public string accType { set; get; }
       
        
    }
}