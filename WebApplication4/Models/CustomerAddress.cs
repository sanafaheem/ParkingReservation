using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class CustomerAddress
    {
        public string HouseNO { set; get; }
        public string Street { set; get; }
        public string City { set; get; }
        
        [Required(ErrorMessage ="Please enter valid postal code")]
        [MaxLength(6)]
        [Display(Name = "Postal/Zip Code")]
        public int PostalCode { set; get; }
        [Required(ErrorMessage = "Please enter country")]
        [Display(Name ="Country")]
        public string country { set; get; }

        [Required(ErrorMessage = "Please enter country")]
        [Display(Name ="Province/State")]
        public string province { set; get; }
        public int CustomerID { set; get; }

    }
}