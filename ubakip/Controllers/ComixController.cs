using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ubakip.Models;

namespace ubakip.Controllers
{
    public class ComixController : Controller
    {
        // GET: Comix
        public ActionResult Index()
        {
            List<Tag> tags = new List<Tag>();
            tags.Add(new Tag() { Name = "tag1" });
            tags.Add(new Tag() { Name = "tag2" });
            tags.Add(new Tag() { Name = "tag3" });
            Users author = new Users() { Name = "bamix" };
            Post post = new Post()
            {
                Name = "test name",
                Author = author,
                Id = 1L,
                Rating = 3.2f,
                UserRating = 2f,
                Cover = "https://pp.vk.me/c633523/v633523851/9920/E93Q5a_KRzE.jpg",
                CreateTime = DateTime.Now,
                MPAARating = new MPAARating() { Photo = "http://1.bp.blogspot.com/-w8rJ7fH6CNQ/TpusFvSdEfI/AAAAAAAAAqw/KiCGps3Cn3s/s1600/pg.png", Description = "PG" },
                Tags = tags
            };
            Page page1 = new Page() { Preview = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e9/Petit_Sammy_%C3%A9ternue.jpg/275px-Petit_Sammy_%C3%A9ternue.jpg" };
            Page page2 = new Page() { Preview = "http://znayka.org.ua/uploads/6fd0644155/e83218673d.jpg" };
            Page page3 = new Page() { Preview = "http://www.gamer.ru/system/attached_images/images/000/339/387/normal/stalkerlegend-ucoz-ru_rdr_comix_04.jpg" };
            Page page4 = new Page() { Preview = "http://acomics.ru/upload/!c/!import/jonbot-vs-martha/000008-3iey7amix7.jpg" };
            Page page5 = new Page() { Preview = "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcRNb62B-C8g7TUICBBygWgMyfNyLkIvs2Fg6VBYr00rE4Szuhw4" };
            post.Pages.Add(page1);
            post.Pages.Add(page2);
            post.Pages.Add(page3);
            post.Pages.Add(page4);
            post.Pages.Add(page5);
            return View(post);
        }

        public ActionResult ComixMaker()
        {
            Cloud cloud1 = new Cloud()
            {
                id = "0",
                type = "cloud1",
                text = "lol",
                posX = 0,
                posY = 0,
                height = "50%",
                width = "50%"
            };
            Cloud cloud2 = new Cloud()
            {
                id = "1",
                type = "cloud3",
                text = "kek",
                posX = 3f,
                posY = 0.3f,
                height = "20%",
                width = "20%"
            };

            ImageCell imageCell1 = new ImageCell()
            {
                id = "0",
                cellId = "sq1",
                isVideo = false,
                src = "https://pp.vk.me/c630516/v630516851/17d3a/o2M3HScGpQc.jpg",
                scale = 1f,
                rotate = 0,
                posX = 0,
                posY = 0
            };

            ImageCell imageCell2 = new ImageCell()
            {
                id = "1",
                cellId = "sq2",
                isVideo = false,
                src = "https://pp.vk.me/c630516/v630516851/17d3a/o2M3HScGpQc.jpg",
                scale = 2f,
                rotate = 90,
                posX = 0,
                posY = 0.4f
            };

            ImageCell imageCell3 = new ImageCell()
            {
                id = "2",
                cellId = "sq3",
                isVideo = true,
                src = "http://clips.vorwaerts-gmbh.de/VfE_html5.mp4",
                scale = 2f,
                rotate = 0,
                posX = 0,
                posY = 0
            };

            Page page = new Page()
            {
                TemplateName = "template1",
                Background = "00ff00"
            };
            page.ImageCell = new List<ImageCell>();
            page.ImageCell.Add(imageCell1);
            page.ImageCell.Add(imageCell2);
            page.ImageCell.Add(imageCell3);
            page.Clouds = new List<Cloud>();
            page.Clouds.Add(cloud1);
            page.Clouds.Add(cloud2);
            return View(page);
        }

        [HttpPost]
        public ActionResult LoadTemplate(string id)
        {
            return PartialView("templates/" + id);
        }

        [HttpPost]
        public ActionResult SavePage(Page page)
        {
            page = page;
            return Json(new { msg = "Successfully added " });
        }
    }
}