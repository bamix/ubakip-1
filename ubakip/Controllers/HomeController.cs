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
            tags.Add(new Tag() { Name = "abc" , Count=10 });
            tags.Add(new Tag() { Name = "tag2", Count = 13 });
            tags.Add(new Tag() { Name = "43f", Count = 1 });
            Users author = new Users() { Name = "bamix" ,Photo= "https://pp.vk.me/c630516/v630516851/17d41/3DClFMPdBSk.jpg"};
            Post post1 = new Post()
            {
                Name = "test name",
                Author = author,                
                Id = 1L,
                Pages = pages,
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
                Pages = pages,
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
                Pages = pages,
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

        [AllowAnonymous]
        public ActionResult FindComixByTag(string tag)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            //UpdateCookie("theme", theme, new List<string>() { "light", "dark" });
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