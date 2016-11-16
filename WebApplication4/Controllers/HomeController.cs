using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View("HomeView");
        }

        public ActionResult Show()
        {
            Student st = new Student();
            st.Name = Request.Form["name"];
            st.Roll = Int32.Parse(Request.Form["roll"]);

            return View("ShowView", st);
        }
    }
}