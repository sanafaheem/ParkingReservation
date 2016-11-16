using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;
using System.Configuration;
using System.Data.SqlClient;



namespace WebApplication4.Controllers
{
    public class AccountController : Controller
    {
        string connStr;
       

        // GET: Account
        public ActionResult LogIn()
        {
            return View("LoginView"); 
        }

        //
        public ActionResult UserAccount()
        {
            Account acc = new Account();
            return View();
        }

      
       
        // GET: Account/Create
        public ActionResult Create()
        {
            connStr = ConfigurationManager.ConnectionStrings["ParkingManagementConnection"].ConnectionString;
            return View("RegisterView");
           


        }

        // POST: Account/Create
        [HttpPost]
        public ActionResult CreateAccount(Account account)
        {
            try
            {
                // TODO: Add insert logic here
               
                 int affectedRows=new DBManagement().createAccountInDB(account,connStr);
                
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Account/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Account/Edit/5
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

        // GET: Account/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Account/Delete/5
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
