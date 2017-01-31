using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class ParkingController : Controller
    {
        private string connStr = "";

        

        // GET: Parking/Create
        public ActionResult Create()
        {
            Parking p = new Parking();
          
            return View("AddParking", p);
        }

        // POST: Parking/Create
        [HttpPost]
        [Authorize(Roles ="admin")]
        public ActionResult Create(Parking p)
        {
            connStr = ConfigurationManager.ConnectionStrings["ParkingManagementConnection"].ConnectionString;
            if (ModelState.IsValid)
            {
                ParkingManagement pdb = new ParkingManagement();
               
                try
                {
                    if (pdb.addParkingInDB(p, connStr)>0)
                    {
                        TempData["Success"] = "Parking have been added Successfully!";

                    }
                    return Create();
                }
                catch(Exception ex)
                {
                    ViewBag.errorMessage = "could not add parking in the system due to" + ex.Message+" Try Again!";
                    return Create();
                }
            }
            return Create();


        }

        // GET: Parking/Edit/5 get the View of Parking menu 
        public ActionResult Show()
        {
            connStr = ConfigurationManager.ConnectionStrings["ParkingManagementConnection"].ConnectionString;
            ShowParking showP = new ShowParking();
            try
            {
                ParkingManagement p = new ParkingManagement();
                p.updateExpiredParkingByDate(connStr);
                return View("ShowParkings", showP);
            }
            catch(Exception ex) 
            {
                ViewBag.errorMessage = "Could Not Update Expired Parking "+ex.Message;
                return View("ShowParkings", showP);
            }
        }

        // POST: Parking/Edit/5 get the ParkingList based on the selection from the parking menu
        [HttpPost]
        public ActionResult ShowController( ShowParking sp)
        {
            connStr = ConfigurationManager.ConnectionStrings["ParkingManagementConnection"].ConnectionString;
            try
            {
                ParkingManagement pm = new ParkingManagement();
                 List<Parking> parkingList=pm.getParkingList(connStr,sp);
                List<ShowParking> ListOfReservation = pm.getParkingFromReservation(connStr,sp);                     

                List<string> reservedId= getReservedList(sp,ListOfReservation);

                int price=getPricePerHour(sp);
                for (int i=0; i<parkingList.Count; i++)
                {
                    for (int j = 0; j < reservedId.Count; j++)
                    {
                        if (parkingList[i].ParkingNumber == reservedId[j])
                        {
                            parkingList[i].ParkingNumber=null;
                        }
                        
                    }
                    parkingList[i].Price = price;
                }
               
                sp.ParkingList = parkingList;
                sp.ListOfAsiel=pm.getNumberOfAsiel(connStr,sp.Selectedbuilding,sp.Selectedlevel);
                 sp.ListOfAsiel.Sort();
                Session["parkings"] = parkingList;
                return View("ShowParkings",sp);
            }
            catch(Exception ex)
            {
                ViewBag.errorMessage = "Error occur while fetching parking from DataBase"+ex.Message;
                return View("ShowParkings",sp);
            }
        }
        
       public  List<string> getReservedList(ShowParking sp,List<ShowParking> lsp)
        {
            List<string> listOFID = new List<string>();
            for (int i=0; i<lsp.Count; i++)
            {

                if (sp.DateIn > lsp[i].DateOut & sp.DateIn<lsp[i].DateIn)
                {
                    listOFID.Add(lsp[i].ParkingID);
                    
                }
                else if (sp.DateIn == lsp[i].DateIn)
                {
                    listOFID.Add(lsp[i].ParkingID);
                }
                else if(sp.DateOut>lsp[i].DateIn & sp.DateOut < lsp[i].DateOut)
                {
                    listOFID.Add(lsp[i].ParkingID);

                }

            }
            return listOFID;

        }
        public int getPricePerHour(ShowParking sp)
        { int price = 0;
            var timeStart=sp.TimeIn.ToString("HH:mm");
            DateTime st = Convert.ToDateTime(timeStart);

            DateTime startDt = new DateTime(sp.DateIn.Date.Year,sp.DateIn.Date.Month,sp.DateIn.Date.Day,st.Hour,st.Minute,st.Second);

            var timeEnd = sp.TimeOut.ToString("HH:mm");
            DateTime et = Convert.ToDateTime(timeEnd);
            DateTime endtDt = new DateTime(sp.DateOut.Date.Year, sp.DateOut.Date.Month, sp.DateOut.Date.Day, et.Hour, et.Minute, et.Second);
            TimeSpan span =endtDt.Subtract(startDt);
            var hours = span.Hours;
            var days = span.Days;
             
            if (hours  <=2  && days==0)
            {
                price=2;
            }
            else if(days>0 )
            {
                price = hours+(days*24);
            }
            else
            {
                price = hours;
            }
            return price;


        }
        // reserve the selected parking 
        [HttpPost]
        public ActionResult ReserveParking(ShowParking sp,string parkingIdButton,List<Parking> parking)
        {
            
            sp.ParkingID=parkingIdButton;
            sp.ParkingList = (List<Parking>)Session["parkings"];
            //get the selected Parking from Parking List 
          if(sp.ParkingList!=null)
            {
                for (int i=0; i<sp.ParkingList.Count; i++)
                {
                    if (sp.ParkingList[i].ParkingNumber == sp.ParkingID)
                    {
                        sp.Asiel = sp.ParkingList[i].Asiel;
                        sp.Price = sp.ParkingList[i].Price;
                    }
                }
            }
            Session["parkings"] = null;        
            return RedirectToAction("ReserveParking", "Reservation", new RouteValueDictionary(sp));
        }
        // GET: Parking/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Parking/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
