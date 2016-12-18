using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class Parking
    {
        [Required]
        [Display(Name = "Parking Number")]
        public string ParkingNumber
        {
            set;
            get;
        }
        [Required]
        [Range(0,50)]
        [Display(Name = "Parking Level")]
        public string Level { set; get; }
        [Required]
        [Display(Name = "Parking Asiel")]
        public string Asiel { set; get; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C}")]
        [Display(Name = "Parking Price")]
        public decimal Price { set; get; }
        [Required]
        [MinLength(0)]
        [Display(Name = "Building Number")]
        public string BuildingNumber { set; get; }
    }
}