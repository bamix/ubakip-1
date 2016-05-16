using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using System.Data.Entity;
using ubakip.Models;
using MultilingualSite.Filters;

namespace ubakip.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        public int pagesize = 5;
        [AllowAnonymous]
        public ActionResult Index()
        {
            List<Comix> comixes = new List<Comix>();
            List<Tag> tags = new List<Tag>();
            using (var db = new DataBaseConnection())
            {
                comixes = db.Comixes.Include(c => c.Author).Include(c => c.CoverPage).Include(c => c.Pages).Include(c => c.Tags).ToList();
                tags = db.Tags.OrderBy(c => c.Count).Take(10).ToList();
            }
            ComixesRepository comixesRepository = new ComixesRepository();
            comixesRepository.Posts = ComixController.MakePostsFromComixes(comixes);
            comixesRepository.Tags =tags;
            return View(comixesRepository);
        }

        public ActionResult AdminPanel()
        {
            var db = new DataBaseConnection();
            return View(db.Users.ToList());
        }

        public User GetCurrentUser()
        {
           string login = User.Identity.Name;
            User user = new User();
            using (var db = new DataBaseConnection())
            {              
                 user = db.Users.Where(c => c.Login == login).FirstOrDefault();
            }
           return user;
        }

        [AllowAnonymous]
        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.PathAndQuery;
            string d = UpdateCookie("lang", lang, new List<string>() { "ru", "en" });

            if (User.Identity.IsAuthenticated)
                using (var db = new DataBaseConnection())
                {
                    var user = db.Users.Where(u => u.Login == User.Identity.Name).FirstOrDefault();
                    db.Users.Attach(user);
                    user.Lang = d;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }

            return Redirect(returnUrl);
        }

        [AllowAnonymous]
        public ActionResult ChangeTheme(string theme)
        {
            string returnUrl = Request.UrlReferrer.PathAndQuery;
            string d = UpdateCookie("theme", theme, new List<string>() { "light", "dark" });

            if (User.Identity.IsAuthenticated)
                using (var db = new DataBaseConnection())
                {
                    var user = db.Users.Where(u => u.Login == User.Identity.Name).FirstOrDefault();
                    db.Users.Attach(user);
                    user.Theme = d;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }

            return Redirect(returnUrl);
        }

        [AllowAnonymous]
        public ActionResult FindComixByTag(string tag)
        {
            string returnUrl = Request.UrlReferrer.PathAndQuery;
            return Redirect(returnUrl);
        }

        public string UpdateCookie(string key, string value, List<string> validValues)
        {
            if (!validValues.Contains(value))
            {
                value = validValues[0];
            }
            HttpCookie cookie = Request.Cookies[key];
            if (cookie != null)
                cookie.Value = value;   // если куки уже установлено, то обновляем значение
            else
            {
                cookie = new HttpCookie(key);
                cookie.HttpOnly = false;
                cookie.Value = value;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return value;
        }

        [AllowAnonymous]
        public ActionResult Error404()
        {
            return View();
        }

        public ActionResult Delete(int id)
        {
            //Дописать: при удалении пользователя, удалять также записи о нем из всех бд:
            //(Медали, Комменты к нему, его фото...); Либо инициировать цепное удаление через каскады бд.
            //Его комиксы приводить к имени <Пользователь удален> 
            //(Деидентифицировать его материалы)
            using (var db = new DataBaseConnection())
            {
                var model = db.Users.Find(id);

                if (model != null)
                {
                    db.Users.Remove(model);
                    db.SaveChanges();
                    if (model.Login == User.Identity.Name) //If we deleting ourselves
                    {
                        Request.GetOwinContext().Authentication.SignOut("ApplicationCookie");
                        return RedirectToAction("Index", "Home");
                    }
                }
                return RedirectToAction("AdminPanel");
            }
        }

        public ActionResult Reset(int id)
        {
            using (var db = new DataBaseConnection())
            {
                var user = db.Users.Find(id);

                if (user != null)
                {
                    db.Users.Attach(user);
                    user.Password = null;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }
                return RedirectToAction("AdminPanel");
            }
        }
    }
}