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
                    ViewBag.errorMessage = "could not add pparking in the system due to" + ex.Message+" Try Again!";
                    return Create();
                }
            }
            return Create();


        }

        // GET: Parking/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Parking/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
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
