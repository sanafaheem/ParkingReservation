using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class ShowParking
    {
        [DataType(DataType.Date)]

        public DateTime DateIn { set; get; }
        
        [DataType(DataType.Date)]
        public DateTime DateOut { set; get; }
        [DataType(DataType.Time)]
        public DateTime TimeIn { set; get; }
           [DataType(DataType.Time)]
           public DateTime TimeOut{ set; get; } 
        public IEnumerable<BuildingLevel> buildings{ set; get; }
        public  string SelectedItem { set; get; }
    }

}