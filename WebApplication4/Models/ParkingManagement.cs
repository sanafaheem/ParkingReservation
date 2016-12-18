using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication4.Models
{
    public class ParkingManagement
    {
        public int addParkingInDB(Parking p,string connStr)
        {
            int AffectedRows = 0;

            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                using (conn)
                {
                    conn.Open();

                    string queryString = "Insert into Parking  values(@ParkingNumber,@ParkingLevel,@BuildingNumber,@AsielNo,@ParkingPrice) ";
                    SqlCommand comm = new SqlCommand(queryString, conn);

                    comm.Parameters.AddWithValue("@ParkingNumber", p.ParkingNumber);
                    comm.Parameters.AddWithValue("@ParkingLevel", p.Level);
                    comm.Parameters.AddWithValue("@BuildingNumber", p.BuildingNumber);
                    comm.Parameters.AddWithValue("@AsielNo", p.Asiel);
                    comm.Parameters.AddWithValue("@ParkingPrice", p.Price);
                    AffectedRows = comm.ExecuteNonQuery();
                }
                       
                  
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
                
            }


            return AffectedRows; 
        }
    }
}