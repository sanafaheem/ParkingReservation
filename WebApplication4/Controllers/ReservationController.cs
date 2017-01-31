using Hangfire;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class ReservationController : Controller
    {
        // GET: Reservation
        
        public ActionResult ReserveParking(ShowParking sp)
        {
            
           //ShowParking sp = (ShowParking)ob;
            Reserve reserve = new Reserve();
            reserve.ParkingId =sp.ParkingID;
            reserve.DateIn = sp.DateIn;
            reserve.DateOut = sp.DateOut;
            reserve.TimeIn = sp.TimeIn;
            reserve.TimeOut = sp.TimeOut;
            reserve.Asiel = sp.Asiel;
            reserve.LevelNumber =Convert.ToInt32(sp.Selectedlevel);
            reserve.BuildingNumber = Convert.ToInt32(sp.Selectedbuilding);
            reserve.amount = (float)sp.Price;
         

            
           try {
                ParkingManagement pm = new ParkingManagement();
                
                reserve.ID= pm.addReservation(reserve);
                Session["reserve"] = reserve;
              

                BackgroundJob.Schedule(()=>deleteRow(reserve.ID),TimeSpan.FromMinutes(10));

                return View("ReserveView",reserve);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
               
        }

        public void deleteRow(int ID)
        {
            try
            {
                ParkingManagement pm = new ParkingManagement();
                pm.DaleteRow(ID);

               // Session["reserve"] = null;
                Console.Write("Row Deleted");
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            }

        public ActionResult ReserveView()
        {

            Reserve reserve=(Reserve)Session["reserve"];
            return View("ReserveView", reserve);
        } 
        public ActionResult ConfirmReservation(Reserve reserve)
        {

            if (!User.Identity.IsAuthenticated)
            {
                  return RedirectToAction("LogIn","Account");
            }
            else
            {
                CheckOutViewModel ck = new CheckOutViewModel();
                try
                {
                    string conStr = ConfigurationManager.ConnectionStrings["ParkingManagementConnection"].ConnectionString;
                    string userEmail = getEmail();
                    DBManagement db = new DBManagement();
                    Account acc = db.getUserInfo(userEmail, conStr);
                    CustomerAddress address = db.getCustomerAdress(acc.ID, conStr);
                    Vehicle vehicle = db.getVehicle(acc.ID, conStr);
                    
                    ck.Acc = acc;
                    ck.Address = address;
                    ck.vehicle = vehicle;
                    ck.reserve = reserve;

                }
                catch(Exception e)
                {
                    ViewBag.errorMessage="could't reserve parking error in system,"+e.Message;
                    return View("ReserveView",reserve);
                }
                    return View("CheckOutView",ck);
            }

            
        }
       
        protected string getEmail()
        {
            string curruser = "";
            if (System.Web.HttpContext.Current.User.Identity != null)
            {

                var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;

               curruser= identity.Claims.Where(c => c.Type == ClaimTypes.Email)
                   .Select(c => c.Value).SingleOrDefault();
                

            }
            return curruser;

        }
        
    }
}