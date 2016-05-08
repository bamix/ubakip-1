using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using System.Data.Entity;
using ubakip.Models;

namespace ubakip.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminPanel()
        {
            var db = new MainDbContext();
            return View(db.Users.ToList());
        }
    }
}