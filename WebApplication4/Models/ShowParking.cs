using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication4.Models
{
    public class ShowParking

    {
          public string ParkingID { set;  get;}

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Please select Start date")]
        public DateTime DateIn { set; get; }

        [Required(ErrorMessage = "Plese select End date")]
        [DataType(DataType.Date)]
        public DateTime DateOut { set; get; }

        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Please select start time")]
        public DateTime TimeIn { set; get; }

           [DataType(DataType.Time)]
        [Required(ErrorMessage = "Please select End time")]
        public DateTime TimeOut{ set; get; } 

        //public IEnumerable<BuildingLevel> buildings{ set; get; }
        //public  string SelectedItem { set; get; }
        private List<SelectListItem> _building = new List<SelectListItem>();

        public List<SelectListItem> _level = new List<SelectListItem>();

        public List<SelectListItem> Building
        {
            get
            {
                _building.Add(new SelectListItem() { Text = "1234", Value = "1234" });
                _building.Add(new SelectListItem() { Text = "1235", Value = "1235" });
                return _building;
            }
            
        }
        
        public List<SelectListItem> Level
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
        [Required(ErrorMessage = "Please select a building")]
        [DisplayName("Building")]
        public string Selectedbuilding { get; set; }



        [Required(ErrorMessage = "Please select a level")]
        [DisplayName("Lavel")]
        public string Selectedlevel{ get; set; }
        public List<string>  ListOfAsiel{ get; set; }
        public List<Parking> ParkingList
        {
            set; get;
        }
       public decimal Price { set; get; }
        public string Asiel { set; get; }


    }

}