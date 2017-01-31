using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication4.Models
{
    public class Parking
    {
       
        [Display(Name = "Parking Number")]
        public string ParkingNumber
        {
            set;
            get;
        }
        [Required]
        [Range(0,5)]
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
        public int NumberOfAsiel { get; set; }
       public List<Parking> ParkingList { set; get; }

        private List<SelectListItem> _building = new List<SelectListItem>();

        public List<SelectListItem> _level = new List<SelectListItem>();

        public List<SelectListItem> BuildingList
        {
            get
            {
                _building.Add(new SelectListItem() { Text = "1234", Value = "1234" });
                _building.Add(new SelectListItem() { Text = "1235", Value = "1235" });
                return _building;
            }

        }

        public List<SelectListItem> LevelList
        {
            get
            {
                _level.Add(new SelectListItem() { Text = "one", Value = "1" });
                _level.Add(new SelectListItem() { Text = "two", Value = "2" });
                _level.Add(new SelectListItem() { Text = "three", Value = "3" });
                _level.Add(new SelectListItem() { Text = "four", Value = "4" });
                _level.Add(new SelectListItem() { Text = "five", Value = "5" });
                return _level;
            }

        }
    }
}