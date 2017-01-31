using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.App_Start;

namespace WebApplication4.Models
{
    public class ParkingManagement
    {
        /// <summary>
        /// Add Parkings In ParkingTable 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="connStr"></param>
        /// <returns></returns>
        public int addParkingInDB(Parking p,string connStr)
        {
            int AffectedRows = 0;

            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                p.ParkingNumber= countNumberOfParking(connStr);
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
        public string countNumberOfParking(String conStr)
        {
            int count = 0;
            string parkingID = "PId";
            SqlConnection con = new SqlConnection(conStr);
            using (con)
            {
                con.Open();
                string querryStr = "SELECT COUNT(*) FROM Parking";
                SqlCommand com = new SqlCommand(querryStr,con);
                count=(int)com.ExecuteScalar();

            }
            con.Close();
            count += 1;
            parkingID = parkingID+count;

            return parkingID;
        }

        public void DaleteRow(int iD)
        {
            
            bool reserved = false;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ParkingManagementConnection"].ConnectionString);
            using (con)
            {
                con.Open();
                string querryStr = "Delete From Reservations Where ReservationID=@ID And Reserved=@reserved";
                SqlCommand comm = new SqlCommand(querryStr,con);
                comm.Parameters.AddWithValue("@reserved", reserved);
                comm.Parameters.AddWithValue("@ID",iD);
                comm.ExecuteNonQuery();

            }

        }

        /// <summary>
        /// Get ParkingList from Parking Table With specific Building Number and Level  
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<Parking> getParkingList(string connStr,Object obj)
        {
            ShowParking sp = (ShowParking)obj;

            List<Parking> parkingList = new List<Parking>();
            SqlConnection conn = new SqlConnection(connStr);
            using (conn)
            {
                conn.Open();
                string queryStr = "Select * From Parking  Where (BuildingNumber=@BuildingNumber And ParkingLevel=@ParkingLevel) Order By AsielNo";
                SqlCommand comm = new SqlCommand(queryStr,conn);
                comm.Parameters.AddWithValue("@BuildingNumber",sp.Selectedbuilding);
                comm.Parameters.AddWithValue("@ParkingLevel",sp.Selectedlevel);
                using (SqlDataReader reader =comm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Parking p = new Parking();
                        p.ParkingNumber = reader.GetString(0);
                        p.Level = reader.GetInt32(reader.GetOrdinal("ParkingLevel")).ToString();
                        p.BuildingNumber = reader.GetInt32(reader.GetOrdinal("BuildingNumber")).ToString();
                        p.Asiel = reader.GetString(3);
                        p.Price = reader.GetInt32(4);
                        parkingList.Add(p);

                    }

                }
                conn.Close();
            }
            return parkingList;

        } 
      

        public List<string> getNumberOfAsiel(string conStr,string building,string level)
        {
           
            List<string> ListOfAsiel = new List<string>();
            SqlConnection conn = new SqlConnection(conStr);
            using (conn)
            {
                conn.Open();
                string queryStr = "Select Distinct AsielNo From Parking Where (BuildingNumber=@BuildingNumber And ParkingLevel=@ParkingLevel)";
                using (SqlCommand com = new SqlCommand(queryStr, conn))
                {
                    com.Parameters.AddWithValue("@BuildingNumber", building);
                    com.Parameters.AddWithValue("@ParkingLevel", level);

                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string item=null;
                            item = (reader["AsielNo"].ToString());
                            ListOfAsiel.Add(item);
                        }
                    }

                }

             }

            return ListOfAsiel;
        }
        /// <summary>
        /// update expired parking by date 
        /// </summary>
        /// <param name="conStr"></param>
        /// 
        public void updateExpiredParkingByDate(string conStr)
        {

            SqlConnection conn = new SqlConnection(conStr);
            DateTime currenDate = DateTime.Today;

            using (conn)
            {
                conn.Open();
                Boolean expired = true;
                string querryStr = "Update Reservations Set IsExpired=@expired Where DateOut<@CurrentDate";   


                SqlCommand comm = new SqlCommand(querryStr,conn);
                comm.Parameters.AddWithValue("@expired",expired);
                 comm.Parameters.AddWithValue("@CurrentDate",currenDate.ToString("d"));
                comm.ExecuteNonQuery();
            }
            conn.Close();

        }
        /// <summary>
        /// Get the resrved Parkings From reservation Table which are not Expired
        /// </summary>
        /// <param name="conStr"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public List<ShowParking> getParkingFromReservation(string conStr,ShowParking sp)
        {
            List<ShowParking> ListOfReservedParking = new List<ShowParking>();
            SqlConnection con = new SqlConnection(conStr);
            using (con)
            {
                con.Open();
                Boolean expired = true;
                string querryStr = "Select DateIn,DateOut,TimeIn,TimeOut,ParkingID from Reservations Where IsExpired!=@expired And(BuildingNumber=@BuildingNumber And LevelNumber=@LevelNumber)";
                SqlCommand comm = new SqlCommand(querryStr,con);
                comm.Parameters.AddWithValue("@expired",expired);
                comm.Parameters.AddWithValue("@BuildingNumber", sp.Selectedbuilding);
                comm.Parameters.AddWithValue("@LevelNumber", sp.Selectedlevel);
                using (SqlDataReader reader=comm.ExecuteReader()) {
                    while (reader.Read()) {
                        ShowParking p = new ShowParking();
                        p.DateIn = reader.GetDateTime(0);
                        p.DateOut = reader.GetDateTime(1);
                        p.TimeIn = reader.GetDateTime(2);
                        p.TimeOut = reader.GetDateTime(3);
                        p.ParkingID = reader.GetString(9);
                        ListOfReservedParking.Add(p);
                            }
                }

            }
            return ListOfReservedParking;
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
                        levelList.Add( reader.GetInt32(reader.GetOrdinal("ParkingLevel")).ToString() );
                    }
                }
               
            }


                return levelList;
        }

        // Add reservation in Reservation Table Without Payment and Confirmation 
        public int addReservation(Reserve reserve)
        {
            int insertedId = 0;
            bool reserved = false;
            reserve.ReserveNo="Res"+GenerateUniqueNumber.Next();

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ParkingManagementConnection"].ConnectionString);
            using (conn)
            {
                conn.Open();
                string querryStr = "Insert into Reservations(ReservationNo,DateIn,DateOut, TimeIn,TimeOut,Reserved,Amount,ParkingID,BuildingNumber,LevelNumber) Output Inserted.ReservationID values(@ReservationNo, @DateIn, @DateOut, @TimeIn,@TimeOut,@Reserved,@Amount,@ParkingID,@BuildingNumber,@LevelNumber)";
                SqlCommand comm = new SqlCommand(querryStr,conn);
                comm.Parameters.AddWithValue("@ReservationNo", reserve.ReserveNo);
                comm.Parameters.AddWithValue("@DateIn", reserve.DateIn);
                comm.Parameters.AddWithValue("@DateOut",reserve.DateOut);
                comm.Parameters.AddWithValue("@TimeIn",reserve.TimeIn);
                comm.Parameters.AddWithValue("@TimeOut",reserve.TimeOut);
                comm.Parameters.AddWithValue("@Reserved",reserved);
                comm.Parameters.AddWithValue("@Amount",reserve.amount);
                comm.Parameters.AddWithValue("@ParkingID",reserve.ParkingId);
                comm.Parameters.AddWithValue("@BuildingNumber",reserve.BuildingNumber);
                comm.Parameters.AddWithValue("@LevelNumber",reserve.LevelNumber);

                insertedId=Convert.ToInt32(comm.ExecuteScalar());


            }

            return insertedId;

        }
    }
}