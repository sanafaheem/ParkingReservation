using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class CheckOutViewModel
    {
        [Required(ErrorMessage ="Please enter Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { set; get; }
       public Vehicle vehicle { set; get;}
        public CustomerAddress Address { set; get; }
      public  Account Acc { set; get; }
        public Reserve reserve { set; get; }
    }
}