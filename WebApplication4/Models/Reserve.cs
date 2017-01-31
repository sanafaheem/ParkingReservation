using System;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class Reserve
    {
        public int ID { set; get; }
        public string ParkingId { set; get; }
        public string ReserveNo { set; get; }
        //[ DataType(DataType.Date)]
        public DateTime DateIn { set; get; }
        //[DataType(DataType.Date)]
        public DateTime DateOut { set; get; }
        //[DataType(DataType.Time)]
        public DateTime TimeIn { set; get; }
        //[DataType(DataType.Time)]
        public DateTime TimeOut { set; get; }
        public Boolean reserved { set; get; }
        public float amount { set; get; }
        public int BuildingNumber { set; get; }
        public  int LevelNumber { set; get; }
        public string Asiel { set; get; }
        public string VehicleNo { set; get; }


    }
}