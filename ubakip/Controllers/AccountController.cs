using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using ubakip.CustomLibraries;
using ubakip.Models;
using MultilingualSite.Filters;
namespace ubakip.Controllers
{
    [AllowAnonymous]
    [Culture]
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public void SignIn(string login, string email)
        {
            var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, login),
                        new Claim(ClaimTypes.Email, email)}, "ApplicationCookie");
            Request.GetOwinContext().Authentication.SignIn(identity);
        }

        [HttpPost]
        public ActionResult Login(Users model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using (var db = new DataBaseConnection())
            {
                var email_base = db.Users.FirstOrDefault(u => u.Email == model.Email);

                if (email_base == null)
                    return View(model);
                
                var password_model = CustomEncrypt.Encrypt(model.Password);
                var password_base = db.Users.Where(u => u.Email == model.Email).FirstOrDefault().Password;

                if (password_base == null)
                    return View(model);

                if (model.Email != null && password_model == password_base)
                {
                    var login = db.Users.Where(u => u.Email == model.Email).FirstOrDefault().Login;
                    var email = db.Users.Where(u => u.Email == model.Email).FirstOrDefault().Email;

                    SignIn(login, email);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if(email_base != null && model.Email != null && password_base == null)
                    {
                        //create new password here
                    }
                }
            }

            ModelState.AddModelError("", "Invalid email or password");
            return View(model);
        }

        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut("ApplicationCookie");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(Users model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new DataBaseConnection())
                {
                    var queryEmail = db.Users.FirstOrDefault(u => u.Email == model.Email);
                    var queryLogin = db.Users.FirstOrDefault(u => u.Login == model.Login);
                    if (queryEmail == null && queryLogin == null)
                    {
                        var encryptedPassword = CustomEncrypt.Encrypt(model.Password);
                        var user = db.Users.Create();
                        user.Email = model.Email;
                        user.Password = encryptedPassword;
                        user.Login = model.Login;
                        user.Role = 1;
                        user.Lang = "ru";
                        user.Theme = "light";
                        db.Users.Add(user);
                        db.SaveChanges();

                        SignIn(user.Login, user.Email);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                        return RedirectToAction("Registration");
                }
            }
            else
                ModelState.AddModelError("", 
                    "Registration error. Please check your input. " + 
                    "It is worth noting that the login must consist of letters, " +
                    "its length should be between 5 and 15 characters.");

            return View(model);
        }
    }
}