using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class Vehicle
    {
        [Display(Name ="Plate Number")]
        [Required(ErrorMessage ="Please enter Vehicle Number")]
        public string VehicleNO { set; get; }
        [Required(ErrorMessage ="Please Enter Model")]
        public string Model { set; get; }
        [Required(ErrorMessage ="Please enter Made")]
        public string Made { set; get; }
        public string color { set; get; }
        public int CustomerId {set; get;}
       
    }
}