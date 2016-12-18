using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net.Mail;
using WebApplication4.Models;

namespace WebApplication4.App_Start
{
    public class MakeMailMessage
    {
        async public Task sendSync(int destinationID, string subject, string body,string connStr)
        {
            DBManagement DB = new DBManagement();
            string destinationEmail=DB.getEmailOfID(destinationID,connStr);

            MailMessage email = new MailMessage(new MailAddress("noreply@myproject.com", "(do not reply)"),
               new MailAddress(destinationEmail));
            email.Subject = subject;
            email.Body = body;
            using (var mailClient = new WebApplication4.Utility.EmailService())
            {
                try
                {

                    await mailClient.SendMailAsync(email);
                }
                catch(Exception ex)
                {
                    Console.Write(ex.Message);
                    throw new Exception(ex.Message);
                }
            }




        }
    }
}