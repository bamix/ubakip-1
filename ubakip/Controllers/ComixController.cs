using CloudinaryDotNet;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ubakip.Models;

namespace ubakip.Controllers
{
    public class ComixController : Controller
    {
        static Cloudinary m_cloudinary;
        // GET: Comix
        public ActionResult Index()
        {
            //Todo if isAuthor
            return RedirectToAction("Creator");

            Post post = GetTestPost();           
           
            return View(post);
        }

        static ComixController()
        {
            Account acc = new Account(
                    "ubakip-ru",
                    "558288263223776",
                    "IqzfFUQdOiwxYab-wi0a_ppyO-A");

            m_cloudinary = new Cloudinary(acc);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public void UploadDirect()
        {
            var headers = HttpContext.Request.Headers;

            string content = null;
            using (StreamReader reader = new StreamReader(HttpContext.Request.InputStream))
            {
                content = reader.ReadToEnd();
            }

            if (String.IsNullOrEmpty(content)) return;

            Dictionary<string, string> results = new Dictionary<string, string>();

            string[] pairs = content.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var pair in pairs)
            {
                string[] splittedPair = pair.Split('=');

                if (splittedPair[0].StartsWith("faces"))
                    continue;

                results.Add(splittedPair[0], splittedPair[1]);
            }
        }

        private static Post GetTestPost()
        {
            List<Tag> tags = new List<Tag>();
            tags.Add(new Tag() { Name = "tag1" });
            tags.Add(new Tag() { Name = "tag2" });
            tags.Add(new Tag() { Name = "tag3" });
            Users author = new Users() { Name = "bamix",Photo= "https://pp.vk.me/c630516/v630516851/17d41/3DClFMPdBSk.jpg" };
            Post post = new Post()
            {
                Name = "test name",
                Author = author,
                Id = 1L,
                Rating = 3.2f,
                UserRating = 2f,
                //Cover = "https://pp.vk.me/c633523/v633523851/9920/E93Q5a_KRzE.jpg",
                CoverPageId = 1,
                CreateTime = DateTime.Now,
                MPAARating = new MPAARating() { Photo = "http://1.bp.blogspot.com/-w8rJ7fH6CNQ/TpusFvSdEfI/AAAAAAAAAqw/KiCGps3Cn3s/s1600/pg.png", Description = "PG" },
                Tags = tags
            };
            Page page1 = new Page() { Id = 1, Preview = "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e9/Petit_Sammy_%C3%A9ternue.jpg/275px-Petit_Sammy_%C3%A9ternue.jpg" };
            Page page2 = new Page() { Id = 2, Preview = "http://znayka.org.ua/uploads/6fd0644155/e83218673d.jpg" };
            Page page3 = new Page() { Id = 3, Preview = "http://www.gamer.ru/system/attached_images/images/000/339/387/normal/stalkerlegend-ucoz-ru_rdr_comix_04.jpg" };
            Page page4 = new Page() { Id = 4, Preview = "http://acomics.ru/upload/!c/!import/jonbot-vs-martha/000008-3iey7amix7.jpg" };
            Page page5 = new Page() { Id = 5, Preview = "https://encrypted-tbn2.gstatic.com/images?q=tbn:ANd9GcRNb62B-C8g7TUICBBygWgMyfNyLkIvs2Fg6VBYr00rE4Szuhw4" };
            post.Pages.Add(page1);
            post.Pages.Add(page2);
            post.Pages.Add(page3);
            post.Pages.Add(page4);
            post.Pages.Add(page5);
            return post;
        }

        public ActionResult Creator(Post post)
        {    
            ViewBag.AvailableMPAARatings = MPAARating.MPAARatings;
            return View(GetTestPost());
        }

        public ActionResult ComixMaker()
        {
            Cloud cloud1 = new Cloud()
            {
                id = 0,
                type = "cloud1",
                text = "lotgfcrfcl",
                posX = 0,
                posY = 0,
                height = "50%",
                width = "50%"
            };
            Cloud cloud2 = new Cloud()
            {
                id = 1,
                type = "cloud3",
                text = "kek",
                posX = 3f,
                posY = 0.3f,
                height = "20%",
                width = "20%"
            };

            ImageCell imageCell1 = new ImageCell()
            {
                id = 0,
                cellId = "sq1",
                isVideo = 0,
                src = "https://pp.vk.me/c630516/v630516851/17d3a/o2M3HScGpQc.jpg",
                scale = 1f,
                rotate = 0,
                posX = 0,
                posY = 0
            };

            ImageCell imageCell2 = new ImageCell()
            {
                id = 1,
                cellId = "sq2",
                isVideo = 0,
                src = "https://pp.vk.me/c630516/v630516851/17d3a/o2M3HScGpQc.jpg",
                scale = 2f,
                rotate = 90,
                posX = 0,
                posY = 0.4f
            };

            ImageCell imageCell3 = new ImageCell()
            {
                id = 2,
                cellId = "sq3",
                isVideo = 1,
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
            ViewBag.Cloudinary = new DictionaryModel(m_cloudinary, new Dictionary<string, string>() { { "unsigned", "false" } });
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

           // page = page;
            SaveClouds(page.Clouds);
            return Json(new { msg = "Successfully added " });
        }

        [HttpPost]
        public ActionResult GetTag(string quote)
        {
            //TODO Load tags from bd
            List<Tag> tags = new List<Tag>();
            Tag tag1 = new Tag() { Name = "tag1" };
            Tag tag2 = new Tag() { Name = "tag2" };
            Tag tag3 = new Tag() { Name = "tag3" };
            Tag tag4 = new Tag() { Name = "tag4" };
            Tag tag5 = new Tag() { Name = "tag5" };
            tags.Add(tag1);
            tags.Add(tag2);
            tags.Add(tag3);
            tags.Add(tag4);
            tags.Add(tag5);
            return Json(tags);
        }

        [HttpPost]
        public string Upload(string data)
        {
            //            if (file != null && file.ContentLength > 0){
            CloudinaryDotNet.Account account = new CloudinaryDotNet.Account("ubakip-ru", "558288263223776", "IqzfFUQdOiwxYab-wi0a_ppyO-A"); //название, ключ, секретный ключ аккаунта на Cloudinary
            CloudinaryDotNet.Cloudinary cloudinary = new CloudinaryDotNet.Cloudinary(account); //тут все понятно
            CloudinaryDotNet.Actions.ImageUploadParams uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams() //к параметрам, которые передадим в post запросе присоединяем имя файла и путь к нему (путь - локально; тип string)
            {
                File = new CloudinaryDotNet.Actions.FileDescription(data)
            };
            CloudinaryDotNet.Actions.ImageUploadResult uploadResult = cloudinary.Upload(uploadParams); //POSTим на cloudinary
            string url = cloudinary.Api.UrlImgUp.BuildUrl(String.Format("{0}.{1}", uploadResult.PublicId, uploadResult.Format)); //ответный url: содержит прямую ссылку на файл
                                                                                                                                 //           }
            return url;
        }

        private void SaveClouds(List<Cloud> clouds)
        {
            DeleteEmptyClouds(clouds);
            SaveCloudsToDatabase(clouds);
        }

        private void DeleteEmptyClouds(List<Cloud> clouds)
        {
            List<Cloud> cloudsToRemove = new List<Cloud>();
            for (int i = 0; i < clouds.Count; i++)
                if (clouds[i].text == null) {
                    cloudsToRemove.Add(clouds[i]);
                    clouds.RemoveAt(i);
                    i--;                  
                }
            RemoveCloudsFromDatabase(cloudsToRemove);
        }

        private void RemoveCloudsFromDatabase(List<Cloud> clouds)
        {
            //TODO Realize
        }

        //public int SaveImageCell(ImageCell imageCell)
        //{
        //    using (var db = new MainDbContext())
        //    {
        //        db.ImageCell.Add(imageCell);
        //        db.SaveChanges();
        //    }
        //    return imageCell.id;
        //}

        private List<int> SaveImageCellsToDatabase(List<ImageCell> imageCell)
        {
            List<int> ids = new List<int>();
            using (var db = new MainDbContext())
            {
                foreach (var imgCell in imageCell)
                {
                    db.ImageCell.Add(imgCell);
                    db.SaveChanges();
                    ids.Add(imgCell.id);
                }
            }
            return ids;
        }

        //public int SaveCloudToDatabase(Cloud cloud)
        //{
        //    using (var db = new MainDbContext())
        //    {              
        //        db.Cloud.Add(cloud);
        //        db.SaveChanges();
        //    }
        // return cloud.id;
        //}

        private List<int> SaveCloudsToDatabase(List<Cloud> clouds)
        {
            List<int> ids = new List<int>();
            using (var db = new MainDbContext())
            {
                var c = db.Cloud.Any(o => o.id == clouds[0].id);
                foreach (var cld in clouds)
                {
                    ids.Add(db.Cloud.Any(o => o.id == cld.id) ? UpdateCloud(db, cld) : AddCloud(db, cld));
                }
                db.SaveChanges();
            }
            return ids;
        }

        private int UpdateCloud(MainDbContext db, Cloud cloud)
        {
            db.Entry<Cloud>(cloud).State = EntityState.Modified;
            return cloud.id;
        }

        private int AddCloud(MainDbContext db, Cloud cloud)
        {
            db.Cloud.Add(cloud);
            return cloud.id;
        }

    }
}