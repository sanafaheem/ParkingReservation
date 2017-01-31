using System.Web.Mvc;
using WebApplication4.Models;
using System.Configuration;
using System.Threading.Tasks;

using WebApplication4.App_Start;
using System;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web;
using System.Threading;

namespace WebApplication4.Controllers
{
    public class AccountController : Controller
    {
        string connStr;
       

        // GET: Account
        public ActionResult LogIn()
        {
            
            return View("LoginView", new LoginModel()); 
        }

        //
        [HttpPost]
        public ActionResult LogIn(LoginModel creds)
        {
            connStr = ConfigurationManager.ConnectionStrings["ParkingManagementConnection"].ConnectionString;
            try
            {
                if (ModelState.IsValid)
                {
                    DBManagement db = new DBManagement();
                    Account acc = db.authenticateUser(creds, connStr);
                    if (acc !=null)
                    {
                        var ident = new ClaimsIdentity(
                           new[] { 
                          // adding following 2 claim just for supporting default antiforgery provider
                          new Claim(ClaimTypes.NameIdentifier, acc.Email),
                          new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),

                          new Claim(ClaimTypes.Name,acc.FirstName),
                          new Claim (ClaimTypes.Email,acc.Email),
                          // optionally you could add roles if any
                          new Claim(ClaimTypes.Role, acc.accType)
             

                      },
                      DefaultAuthenticationTypes.ApplicationCookie);

                        HttpContext.GetOwinContext().Authentication.SignIn(
                          new AuthenticationProperties { IsPersistent = false }, ident);
                        var claimsPrincipal = new ClaimsPrincipal(ident);
                        // Set current principal
                        Thread.CurrentPrincipal = claimsPrincipal;
                        if (acc.accType == "admin")
                        {
                           
                            return RedirectToAction("index", "home");
                            //return View("AdminView");
                       }
                        else
                        {  
                          
                            return View("Index", acc);

                        }

                    }

                }
                ViewBag.errorMessage = "Account match Does not exist";
                return LogIn();
            }
            catch(Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return LogIn();
            }
        }

       //Sign Out
       public ActionResult LogOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("LogIn","Account");
        }
      
       
        // GET: Account/Create
        public ActionResult Create()
        {
            
            return View("RegisterView");
           


        }

        // POST: Account/Create
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> CreateAccount(Account account)
        {
            // TODO: Add insert logic here
            try
            {
                connStr = ConfigurationManager.ConnectionStrings["ParkingManagementConnection"].ConnectionString;
                if (ModelState.IsValid)
                {
                    int RowID = new DBManagement().createAccountInDB(account, connStr);
                    if (RowID > 0)
                    {
                        string callbackUrl = await SendEmailConfirmationTokenAsync(RowID, "Account confirmation");
                        return View("ShowMsg");
                    }
                    ViewBag.errorMessage = "Faild to create record";
                }

                return View("Create");
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = ex.Message;
                return View("Create");
            }
        }

        public async Task<string> SendEmailConfirmationTokenAsync(int ID,string subject)
        {
            var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = ID+"", }, protocol: Request.Url.Scheme);
            await new MakeMailMessage().sendSync(ID, subject, "Please confirm your account by <a href=\"" + callbackUrl + "\">clicking here</a>", connStr);
            return callbackUrl;
        }  

        // GET: Account/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        //Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            // var result = await ConfirmEmailAsync(userId, code);
            return View("Create");//(result.Succeeded ? "ConfirmEmail" : "Error");
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
