using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        // GET: Parking/Edit/5
        public ActionResult Show()
        {
            List<BuildingLevel> li = null;
            connStr = ConfigurationManager.ConnectionStrings["ParkingManagementConnection"].ConnectionString;

            try
            {
                ParkingManagement pm = new ParkingManagement();
                li = pm.getBuilding(connStr);
            }
            catch (Exception ex)
            {
                ViewBag.errorMesage = "could't get building number" + (ex.Message);
            }
            ShowParking showP = new ShowParking();
            showP.buildings=new List<BuildingLevel>(li);

            return View("ShowParkings",showP);
        }

        // POST: Parking/Edit/5
        [HttpPost]
        public ActionResult show( FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult LevelAction(List<BuildingLevel> li)
        {
            ShowParking sp = new ShowParking();
            sp.buildings = li;
            string url = this.Request.UrlReferrer.AbsolutePath;

            return RedirectToAction(url,sp);
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
