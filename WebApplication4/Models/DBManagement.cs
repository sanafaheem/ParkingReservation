using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using WebApplication4.Utility;

namespace WebApplication4.Models
{
    public class DBManagement
    {
        const int DUPLICATE_KEY_ERROR = 2627;

        public int createAccountInDB(Account user,string connStr) {
            SqlConnection conn = new SqlConnection(connStr);
            SqlTransaction transaction = null;
            int RowID = 0; 
            try
            { string accType = "customer";

                user.IsEmailConfirmed = false;

                using (conn)
                {
                    conn.Open();
                    using (transaction = conn.BeginTransaction()) { 
                    string queryString = "Insert into LogIn Output Inserted.ID values(@Email,@Password,@AccountType,@IsEmailConfirmed) ";
                    SqlCommand comm = new SqlCommand(queryString, conn, transaction);
                    
                    comm.Parameters.AddWithValue("@Email",user.Email);
                    comm.Parameters.AddWithValue("@Password", Hash.getPasswordHash(user.Password));
                    comm.Parameters.AddWithValue("@AccountType",accType);
                        comm.Parameters.AddWithValue("@IsEmailConfirmed",user.IsEmailConfirmed);
                     RowID = Convert.ToInt32(comm.ExecuteScalar());
                    if (RowID > 0)
                    {
                        queryString = "Insert into Customer (FirstName, PhoneNumber, Email, LastName) values (@FirstName,@PhoneNumber,@Email,@LastName)";
                        comm = new SqlCommand(queryString, conn, transaction);
                        comm.Parameters.AddWithValue("@FirstName",user.FirstName);
                        comm.Parameters.AddWithValue("@PhoneNumber",user.PhoneNumber);
                        comm.Parameters.AddWithValue("@Email",user.Email);
                        comm.Parameters.AddWithValue("@LastName", user.LastName);
                       comm.ExecuteNonQuery();

                        transaction.Commit();
                            }
                    }


                }
            }
            catch(SqlException ex) {
                if (transaction != null && conn.State == System.Data.ConnectionState.Open)
                {
                    transaction.Rollback();
                    RowID = 0;
                }

                if (ex.Number == DUPLICATE_KEY_ERROR)
                {
                    throw new Exception("Email already exists");
                } 
                else
                {
                    throw new Exception(ex.Message);
                }
            }
            return RowID;
        }
        // gets the Email From LogIn
        public string getEmailOfID(int ID,string connStr)
        {
            string EmailOfID=null;
            SqlConnection conn = new SqlConnection(connStr);

            try
            {

                using (conn)
                {
                    conn.Open();
                    string queryString = "Select Email FROM LogIn Where ID=@ID";
                    SqlCommand com = new SqlCommand(queryString,conn);
                    com.Parameters.AddWithValue("@ID",ID);
                    using (SqlDataReader reader = com.ExecuteReader()){
                        if (reader.Read())
                        {
                            EmailOfID = reader.GetString(0);
                        }
                    }


                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }

            return EmailOfID;
        }
        //authenticate user
        public Account authenticateUser(LoginModel creds, string connStr)
        {
            SqlConnection conn = new SqlConnection(connStr);
            Account acc=null;
            try
            {

                using (conn)
                {
                    conn.Open();
                    string passHash = Hash.getPasswordHash(creds.Password);
                    string queryString = "Select Email,AccountType FROM LogIn Where Email=@Email and Password=@Password";
                    SqlCommand com = new SqlCommand(queryString, conn);
                    com.Parameters.AddWithValue("@Email", creds.Email);
                    com.Parameters.AddWithValue("@Password", passHash);
                    var dbEmail = "";
                    var accType = "";
                    // var dbEmail = com.ExecuteScalar();
                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        
                      if(reader.Read())
                        {
                            dbEmail= reader.GetString(0);
                             accType= reader.GetString(1);

                            
                        }
                        
                    }
                    conn.Close();
                    if (dbEmail != null)
                    {
                        

                        acc = getUserInfo(dbEmail + "",connStr);
                        acc.accType = accType;
                        return acc;
                    }



                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to authenticate user with error: {0}", ex.Message);
                throw new Exception(ex.Message) ;
            }

            return acc;
        }
        //get the user information
        public Account getUserInfo(string dbemail, string connStr)
        {
            Account acc=new Account();
            
            SqlConnection conn = new SqlConnection(connStr);
            using (conn)
            {
                conn.Open();
                string queryString = "Select * FROM Customer Where Email=@Email";
                SqlCommand com = new SqlCommand(queryString, conn);
                com.Parameters.AddWithValue("@Email", dbemail);
                using (SqlDataReader reader = com.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        acc.FirstName = reader.GetString(0);
                        acc.PhoneNumber = reader.GetString(1);
                        acc.Email = reader.GetString(2);
                        acc.LastName = reader.GetString(3);
                        acc.ID = reader.GetInt32(4);
                        return acc;
                    }

                }
            }

            return acc;
        }
       public CustomerAddress getCustomerAdress(int ID,string conStr)
        {
            CustomerAddress custAdr = new CustomerAddress();

            SqlConnection sqlConn = new SqlConnection(conStr);
            using (sqlConn)
            {
                sqlConn.Open();
                string querryStr = "Select * From CustomerAddress Where CustomerID=@ID";
                SqlCommand com=new SqlCommand(querryStr,sqlConn);
                com.Parameters.AddWithValue("@ID",ID);
                using(SqlDataReader reader = com.ExecuteReader()){
                    if (reader.Read())
                    {
                        
                        custAdr.HouseNO = reader.GetString(0);
                        custAdr.Street = reader.GetString(1);
                        custAdr.City = reader.GetString(2);

                        custAdr.PostalCode =Convert.ToInt32(reader.GetString(4));
                        custAdr.country = reader.GetString(5);
                        custAdr.CustomerID = reader.GetInt32(6);
                    }
                    sqlConn.Close();

                }


            }
            return custAdr;


        }
        
        public Vehicle getVehicle(int ID,string ConStr)
        {
            Vehicle vehicle = new Vehicle();
            SqlConnection con = new SqlConnection(ConStr);
            using (con)
            {
                con.Open();
                string querryStr = "Select * From Vehicle Where CustomerID=@ID";
                SqlCommand com = new SqlCommand(querryStr,con);
                com.Parameters.AddWithValue("@ID",ID);
                using(SqlDataReader reader = com.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        vehicle.VehicleNO = reader.GetString(0);
                        vehicle.Model = reader.GetString(1);
                        vehicle.Made = reader.GetString(2);
                        vehicle.color = reader.GetString(3);
                        vehicle.CustomerId = reader.GetInt32(4);
                    }
                }
                con.Close();

            }
            return vehicle;
        }

    }
}