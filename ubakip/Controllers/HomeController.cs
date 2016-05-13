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
            List<Tag> tags = new List<Tag>();
            tags.Add(new Tag() { Name = "abc" , Count=10 });
            tags.Add(new Tag() { Name = "tag2", Count = 13 });
            tags.Add(new Tag() { Name = "43f", Count = 1 });
            Users author = new Users() { Name = "bamix" };
            Post post1 = new Post()
            {
                Name = "test name",
                Author = author,
                Id = 1L,
                Rating = 3.2f,
                UserRating = 2f,
                CoverPageId = 1,
                CreateTime = DateTime.Now,
                MPAARating = new MPAARating() { Photo = "http://1.bp.blogspot.com/-w8rJ7fH6CNQ/TpusFvSdEfI/AAAAAAAAAqw/KiCGps3Cn3s/s1600/pg.png", Description = "PG" },
                Tags = tags
            };

            Post post2 = new Post()
            {
                Name = "test name",
                Author = author,
                Id = 2L,
                Rating = 3.2f,
                UserRating = 4f,
                CoverPageId = 2,
                CreateTime = DateTime.Now,
                MPAARating = new MPAARating() { Photo = "http://1.bp.blogspot.com/-w8rJ7fH6CNQ/TpusFvSdEfI/AAAAAAAAAqw/KiCGps3Cn3s/s1600/pg.png", Description = "PG" },
                Tags = tags
            };

            Post post3 = new Post()
            {
                Name = "test name",
                Author = author,
                Id = 3L,
                Rating = 3.2f,
                UserRating = 0f,
                CoverPageId = 3,
                CreateTime = DateTime.Now,
                MPAARating = new MPAARating() { Photo = "http://1.bp.blogspot.com/-w8rJ7fH6CNQ/TpusFvSdEfI/AAAAAAAAAqw/KiCGps3Cn3s/s1600/pg.png", Description = "PG" },
                Tags = tags
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
            var db = new MainDbContext();
            return View(db.Users.ToList());
        }

       [AllowAnonymous]
        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;      
            UpdateCookie("lang", lang, new List<string>() { "ru", "en" });
            return Redirect(returnUrl);
        }

        [AllowAnonymous]
        public ActionResult ChangeTheme(string theme)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;          
            UpdateCookie("theme",theme, new List<string>() { "light", "dark" });
            return Redirect(returnUrl);
        }
            
        public void UpdateCookie (string key, string value, List<string> validValues )
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
        }
    }
}