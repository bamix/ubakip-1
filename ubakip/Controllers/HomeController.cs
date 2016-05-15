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
        [AllowAnonymous]
        public ActionResult Index()
        {
            string preview = "<div class=\"square cell\" id=\"sq1\"><img id=\"0\" src=\"https://pp.vk.me/c630516/v630516851/17d3a/o2M3HScGpQc.jpg\" class=\"image-cell\" style=\"height: 100%; width: auto; transform: rotate(0deg) scale(1) translate(0%, 0%);\"></div>\n<div class=\"square cell\" id=\"sq2\"><img id=\"1\" src=\"https://pp.vk.me/c630516/v630516851/17d3a/o2M3HScGpQc.jpg\" class=\"image-cell\" style=\"height: 100%; width: auto; transform: rotate(90deg) scale(2) translate(0%, 40%);\"></div>\n<div class=\"rectangle rect cell\" id=\"sq3\"><img class=\"video-btn\" src=\"../../Content/Images/play.png\"><video id=\"2\" class=\"image-cell\" style=\"height: auto; width: 100%; transform: rotate(0deg) scale(2) translate(0%, 0%);\"> <source src=\"http://clips.vorwaerts-gmbh.de/VfE_html5.mp4\" type=\"video/mp4\">   Your browser does not support the video tag.</video></div>\n\n <div class=\"cloud\" id=\"-1\" style=\"height: 50%; width: 50%; transform: translate(0%, 0%);\"><textarea class=\"textarea\" id=\"-t1\" name=\"text\" style=\"-webkit-mask-box-image: url('../../Content/Images/cloud1.png'); mask-border: url('../../Content/Images/cloud1.png') ;\">lol</textarea></div> <div class=\"cloud\" id=\"-2\" style=\"height: 20%; width: 20%; transform: translate(300%, 30%);\"><textarea class=\"textarea\" id=\"-t2\" name=\"text\" style=\"-webkit-mask-box-image: url('../../Content/Images/cloud3.png'); mask-border: url('../../Content/Images/cloud3.png') ;\">kek</textarea></div>";
            Page page = new Page()
            {
                Preview = preview,
                Background = "000000"
            };
            List<Page> pages = new List<Page>();
            pages.Add(page);
            List<Tag> tags = new List<Tag>();
            tags.Add(new Tag() { Name = "abc", Count = 10 });
            tags.Add(new Tag() { Name = "tag2", Count = 13 });
            tags.Add(new Tag() { Name = "43f", Count = 1 });
            User author = new User() { Login = "bamix", Photo = "https://pp.vk.me/c630516/v630516851/17d41/3DClFMPdBSk.jpg" };
            Post post1 = new Post()
            {
                Comix = new Comix()
                {
                    Name = "test name",
                    Author = author,
                    Id = 1,
                    Pages = pages,
                    DateCreated = DateTime.Now,
                     Tags = tags,
                    CoverPage = page
                },               
                Rating = 3.2f,
                UserRating = 2f,         
                MPAARating = new MPAARating() { Photo = "http://1.bp.blogspot.com/-w8rJ7fH6CNQ/TpusFvSdEfI/AAAAAAAAAqw/KiCGps3Cn3s/s1600/pg.png", Description = "PG" }               
            };

            Post post2 = new Post()
            {
                Comix = new Comix()
                {
                    Name = "test name",
                    Author = author,
                    Id = 1,
                    Pages = pages,
                    DateCreated = DateTime.Now,
                    Tags = tags,
                    CoverPage = page
                },
                Rating = 3.2f,
                UserRating = 2f,
                MPAARating = new MPAARating() { Photo = "http://1.bp.blogspot.com/-w8rJ7fH6CNQ/TpusFvSdEfI/AAAAAAAAAqw/KiCGps3Cn3s/s1600/pg.png", Description = "PG" }
            };

            Post post3 = new Post()
            {
                Comix = new Comix()
                {
                    Name = "test name",
                    Author = author,
                    Id = 1,
                    Pages = pages,
                    DateCreated = DateTime.Now,
                    Tags = tags,
                    CoverPage = page
                },
                Rating = 3.2f,
                UserRating = 2f,
                MPAARating = new MPAARating() { Photo = "http://1.bp.blogspot.com/-w8rJ7fH6CNQ/TpusFvSdEfI/AAAAAAAAAqw/KiCGps3Cn3s/s1600/pg.png", Description = "PG" }
            };

            ComixesRepository comixesRepository = new ComixesRepository();
            comixesRepository.Posts.Add(post1);
            comixesRepository.Posts.Add(post2);
            comixesRepository.Posts.Add(post3);
            comixesRepository.Tags = tags;
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