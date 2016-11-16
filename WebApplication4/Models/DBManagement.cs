using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;




namespace WebApplication4.Models
{
    public class DBManagement
    {
        public int createAccountInDB(Account user,string connStr) {
            SqlConnection conn = new SqlConnection(connStr);
            SqlTransaction transaction = conn.BeginTransaction();
            int affectedRows = 0;    
            try
            { string accType = "customer";
                

                using (conn)
                {
                    string queryString = "Insert into LogIn values(@Email,@Password,@AccountType)";
                    SqlCommand comm = new SqlCommand(queryString, conn, transaction);
                    conn.Open();
                    comm.Parameters.AddWithValue("@Email",user.Email);
                    comm.Parameters.AddWithValue("@Password",user.Password);
                    comm.Parameters.AddWithValue("@AccountType",accType);
                    int nRows = comm.ExecuteNonQuery();
                    if (nRows > 0)
                    {
                        queryString = "Insert into Customer values(@FirstName,@PhoneNumber,@Email,@LastName)";
                        comm = new SqlCommand(queryString, conn, transaction);
                        comm.Parameters.AddWithValue("@FirstName",user.FirstName);
                        comm.Parameters.AddWithValue("@PhoneNumber",user.PhoneNumber);
                        comm.Parameters.AddWithValue("@Email",user.Email);
                        comm.Parameters.AddWithValue("@LastName", user.LastName);
                        affectedRows= comm.ExecuteNonQuery();

                        transaction.Commit();

                    }

                }
            }
            catch(SqlException Exc) {
                transaction.Rollback();
                //HttpResponse.Write(Exc);
            }
            return affectedRows;
        }
    }
}