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
        public List<BuildingLevel> getBuilding(string conStr)
        {
            List<BuildingLevel> buildingList=new  List<BuildingLevel>();
            //List<string> li = new List<string>();
            SqlConnection conn = new SqlConnection(conStr);
            try
            {
                using (conn) {
                    conn.Open();
                    string queryStr = "Select Distinct BuildingNumber From dbo.Parking";
                    SqlCommand comm = new SqlCommand(queryStr,conn);
                    using (SqlDataReader reader=comm.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            int b = reader.GetInt32(reader.GetOrdinal("BuildingNumber"));
                            List<string> levelList = getlevelList(conStr,b);
                            BuildingLevel bl = new BuildingLevel();
                            bl.building = b.ToString(); ;
                            bl.level=new List<string>(levelList);
                            buildingList.Add(bl);
                            // li.Add(building);
                        }

                    }


                }
                conn.Close();
            }

            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }



            return buildingList;
        }



        public List<string> getlevelList(string conStr,int building)
        {
            List<string> levelList = new List<string>();
            SqlConnection conn = new SqlConnection(conStr);
            using (conn)
            {
                conn.Open();
                string queryStr="Select Distinct ParkingLevel From Parking Where BuildingNumber=@BuildingNumber";
                SqlCommand com = new SqlCommand(queryStr,conn);
                com.Parameters.AddWithValue("@BuildingNumber", building);
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        levelList.Add(reader.GetInt32(reader.GetOrdinal("ParkingLevel")).ToString());
                    }
                }
               
            }


                return levelList;
        }

    }
}