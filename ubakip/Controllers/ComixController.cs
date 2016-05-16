using CloudinaryDotNet;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MultilingualSite.Filters;
using ubakip.Models;
using System.Text.RegularExpressions;

namespace ubakip.Controllers
{
    [Culture]
    public class ComixController : Controller
    {
      
        // GET: Comix
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(int id)
        {           
           if(id==0 || isAuthor(id)) return RedirectToAction("Creator", new { id = id });
            List<Comix> comixes = new List<Comix>();
            comixes.Add(LoadComixById(id));
            Post post = MakePostsFromComixes(comixes).FirstOrDefault();              
            return View(post);
        }
        
        public bool isAuthor(int comixId)
        {
            var isAuth = false;
            using (var db = new DataBaseConnection())
            {
               Comix comix = db.Comixes.Where(c => c.Id == comixId).Include(c => c.Author).FirstOrDefault();
                if (comix != null && comix.Author.Login == User.Identity.Name) isAuth = true;
            }
            return isAuth;
        }

        public ActionResult MyComixes()
        {
            ComixesRepository comixRepository = new ComixesRepository()
            {
                Posts = MakePostsFromComixes(LoadComixesOfUser(User.Identity.Name))
            };
            return View(comixRepository);
        }

        public List<Comix> LoadComixesOfUser(string user)
        {
            List<Comix> comixes;
            using (var db = new DataBaseConnection())
            {
                comixes = db.Comixes
                    .Where(c => c.Author.Login == user)
                    .Include(c=>c.Author)
                    .Include(c => c.Pages)
                    .Include(c => c.Tags)                   
                    .ToList();
            }
            if (comixes == null) comixes = new List<Comix>();
            return comixes;
        }

        public Comix LoadComixById(int id)
        {
            Comix comix;
            using (var db = new DataBaseConnection())
            {
                comix = db.Comixes
                    .Where(c => c.Id == id)
                    .Include(c => c.Author)
                    .Include(c => c.Tags)
                    .Include(c => c.Tags.Select(t => t.Comixes))
                    .Include(c => c.Pages)
                    .Include(c => c.Pages.Select(p => p.Clouds))
                    .Include(c => c.Pages.Select(p => p.ImageCell))
                    .FirstOrDefault();
                DeleteComixSelfReference(comix);
            }
            if (comix == null) comix = new Comix();
            return comix;
        }

        private static void DeleteComixSelfReference(Comix comix)
        {
            foreach (Page page in comix.Pages)
            {
                DeletePageSelfReference(page);
            }
            foreach (Tag tag in comix.Tags)
            {
                tag.Comixes = null;
            }
        }

        private static void DeletePageSelfReference(Page page)
        {            
                foreach (Cloud cloud in page.Clouds)
                    cloud.Page = null;
                foreach (ImageCell image in page.ImageCell)
                    image.Page = null;            
        }

        public List<Post> MakePostsFromComixes(List<Comix> comixes)
        {
            List<Post> posts = new List<Post>();
            foreach( Comix comix in comixes)
            {
                FixComix(comix);
                posts.Add(new Post()
                {
                    Comix = comix,
                    MPAARating = MPAARating.MPAARatings[comix.MPAARatingId],
                    Rating = GetTotalRating(comix.Id),
                    UserRating = GetUserRating(comix.Id)
                });
            }
            return posts;
        }

        private void FixComix(Comix comix)
        {
            if (comix.CoverPage == null)
                comix.CoverPage = new Page()
                {
                    Preview = "This comix doesn't contain cover",
                    Background = "000000"
                };            
        }

        public float GetTotalRating(int comixId)
        {
            // TODO Realize
            return 2f;
        }

        public float GetUserRating(int comixId)
        {
            // TODO Realize
            return 2f;
        }

        [AllowAnonymous]
        public ActionResult List(int postId, int pageId)
        {
            //TODO Load from bd
            Post post = GetTestPost();
            ViewBag.TargetId = pageId;
            return View(post);
        }

        private static Post GetTestPost()
        {
            List<Tag> tags = new List<Tag>();
            tags.Add(new Tag() { Name = "tag1" });
            tags.Add(new Tag() { Name = "tag2" });
            tags.Add(new Tag() { Name = "tag3" });
            User author = new User() { Login = "bamix",Photo= "https://pp.vk.me/c630516/v630516851/17d41/3DClFMPdBSk.jpg" };
            string preview = "<div class=\"square cell\" id=\"sq1\"><img id=\"0\" src=\"https://pp.vk.me/c630516/v630516851/17d3a/o2M3HScGpQc.jpg\" class=\"image-cell\" style=\"height: 100%; width: auto; transform: rotate(0deg) scale(1) translate(0%, 0%);\"></div>\n<div class=\"square cell\" id=\"sq2\"><img id=\"1\" src=\"https://pp.vk.me/c630516/v630516851/17d3a/o2M3HScGpQc.jpg\" class=\"image-cell\" style=\"height: 100%; width: auto; transform: rotate(90deg) scale(2) translate(0%, 40%);\"></div>\n<div class=\"rectangle rect cell\" id=\"sq3\"><img class=\"video-btn\" src=\"../../Content/Images/play.png\"><video id=\"2\" class=\"image-cell\" style=\"height: auto; width: 100%; transform: rotate(0deg) scale(2) translate(0%, 0%);\"> <source src=\"http://clips.vorwaerts-gmbh.de/VfE_html5.mp4\" type=\"video/mp4\">   Your browser does not support the video tag.</video></div>\n\n <div class=\"cloud\" id=\"-1\" style=\"height: 50%; width: 50%; transform: translate(0%, 0%);\"><textarea class=\"textarea\" id=\"-t1\" name=\"text\" style=\"-webkit-mask-box-image: url('../../Content/Images/cloud1.png'); mask-border: url('../../Content/Images/cloud1.png') ;\">lol</textarea></div> <div class=\"cloud\" id=\"-2\" style=\"height: 20%; width: 20%; transform: translate(300%, 30%);\"><textarea class=\"textarea\" id=\"-t2\" name=\"text\" style=\"-webkit-mask-box-image: url('../../Content/Images/cloud3.png'); mask-border: url('../../Content/Images/cloud3.png') ;\">kek</textarea></div>".Replace("'", "&#39"); ;

            //.Replace("'", "&#39"); important! angular don't understand apostrophe


            Page page1 = new Page() { Id = 1, Preview = preview, Background = "000000" };
            Page page2 = new Page() { Id = 2, Preview = preview, Background = "000000" };
            Page page3 = new Page() { Id = 3, Preview = preview, Background = "000000" };
            Page page4 = new Page() { Id = 4, Preview = preview, Background = "000000" };
            Page page5 = new Page() { Id = 5, Preview = preview, Background = "000000" };
            Post post = new Post()
            {
                Comix = new Comix() { 
                Name = "test name",
                Author = author,
                Id = 1,
                DateCreated = DateTime.Now,
                CoverPage = page1,
                 Tags = tags
                },
                Rating = 3.2f,
                UserRating = 2f,        
                MPAARating = new MPAARating() { Photo = "http://1.bp.blogspot.com/-w8rJ7fH6CNQ/TpusFvSdEfI/AAAAAAAAAqw/KiCGps3Cn3s/s1600/pg.png", Description = "PG" }               
            };
                       post.Comix.Pages.Add(page1);
            post.Comix.Pages.Add(page2);
            post.Comix.Pages.Add(page3);
            post.Comix.Pages.Add(page4);
            post.Comix.Pages.Add(page5);
            return post;
        }

        [Authorize]      
        public ActionResult Creator(int id)
        {          
            Post post = id == 0 ? CreatePost() : LoadPost(id);
          //  post.Comix.CoverPage.Preview= post.Comix.CoverPage.Preview.Replace("'", "&#39");//Important
            FixPostApostrophr(post);
            ViewBag.AvailableMPAARatings = MPAARating.MPAARatings;
            return View(post);
        }

        private void FixPostApostrophr(Post post)
        {
            post.Comix.CoverPage = null;
            post.Comix.Name = post.Comix.Name.Replace("'", "&#39");
            foreach (Page page in post.Comix.Pages)
            {
                FixApostroph(page);
                page.Clouds = null;
                page.ImageCell = null;
            }
        }

        [HttpPost]
        public ActionResult DeletePage(int id, int comixId)
        {
            using (var db = new DataBaseConnection())
            {
                Page page = db.Pages
                    .Include(p=> p.Clouds)
                    .Include(p => p.Clouds.Select(c=>c.Page))
                    .Include(p => p.ImageCell)
                     .Include(p => p.ImageCell.Select(c => c.Page))
                    .FirstOrDefault(c => c.Id == id);               
                if (page != null)
                {
                    Comix comix = db.Comixes
                        .Include(c=>c.CoverPage)
                        .Include(c => c.Pages)
                        .FirstOrDefault(c => c.Id == comixId);
                    if (comix.CoverPage.Id == id)
                        if (comix.Pages.Count == 1) comix.CoverPage = null;
                        else comix.CoverPage = ((List<Page>)comix.Pages)[1];
                    RemoveCloudsFromDatabase((List<Cloud>)page.Clouds);
                    RemoveImagesFromDatabase((List<ImageCell>)page.ImageCell);
                   Page removedPage = comix.Pages.FirstOrDefault(p => p.Id == page.Id);
                    if(removedPage != null)
                    {                        
                        bool isGood = false;
                        do
                        {
                            try
                            {
                                comix.Pages.Remove(removedPage);                               
                                db.SaveChanges();
                                isGood = true;
                            }
                            catch (Exception e)
                            {
                                isGood = false;
                            }
                        } while (!isGood);
                    }                   
                }
            }
            return Json(new { msg = "Successfully" });
        }

        private Post LoadPost(int id)
        {
          List<Comix> comix = new List<Comix>();
          comix.Add(LoadComixById(id));
          Post post =  MakePostsFromComixes(comix).FirstOrDefault();
          return post;
        }

        private Page LoadPage(int pageId)
        {
            Page page = new Page();
            using (var db = new DataBaseConnection())
            {
                page = db.Pages
                    .Where(p => p.Id == pageId)
                    .Include(p => p.Clouds)
                    .Include(p => p.ImageCell)                    
                    .FirstOrDefault();
                DeletePageSelfReference(page);
            }
            return page;
        }

        private Post CreatePost()
        {
            Post post = new Post()
            {
                Comix = new Comix() {
                    Author = GetCurrentUser(),
                    DateCreated = DateTime.Now,
                    Name = "New name",
                    CoverPage = null,
                    MPAARatingId = 1                 
                },
                MPAARating = new MPAARating()
            };
            SaveComixToDatabase(post.Comix);
            return post;
        }

        //FIX
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

        public ActionResult CreateNewPage(int comixId)
        {
            Page newPage = GetDefaultPage();
            using (var db = new DataBaseConnection())
            {
                db.Pages.Add(newPage);
                db.Comixes.Where(c => c.Id == comixId).FirstOrDefault().Pages.Add(newPage);
                db.SaveChanges();
            }
                return RedirectToAction("ComixMaker", new { pageId = newPage.Id });
        }

        private Page GetTestPage()
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
            return page;
        }

        private Page GetDefaultPage()
        {
            return new Page() { Background = "000000", TemplateName="template1", Preview="No preview"};
        }
               
        public ActionResult ComixMaker(int pageId)
        {
            Page page = pageId == 0 ? GetDefaultPage() : LoadPage(pageId);           
            //TODO check if user is author
            FixApostroph(page);
            page.Preview = null;
            return View(page);
        }

        private void FixApostroph(Page page)
        {
            if(page.Preview!=null) page.Preview = page.Preview.Replace("'", "&#39");
            if (page.Clouds != null)
                foreach (Cloud cloud in page.Clouds)
                    cloud.text = cloud.text.Replace("'", "&#39");//.Replace("\n", "");
        }

        [HttpPost]
        public ActionResult LoadTemplate(string id)
        {
            return PartialView("templates/" + id);
        }

        [HttpPost]
        public ActionResult SavePage(Page page)
        {
            page.Preview = page.Preview.Replace("<textarea ", "<textarea disabled ");
            SavePageToDatabase(page);
            SaveClouds(page);
            SaveImages(page);
            return Json(new { msg = "Successfully added " });
        }

        [HttpPost]
        public ActionResult SaveComix(Comix comix)
        {
            if (comix.Pages != null && comix.Pages.Count != 0) comix.CoverPage = LoadPreviewPage(((List<Page>)comix.Pages)[0].Id);       
            SaveComixToDatabase(comix);
            return Json(new { msg = "Successfully added " });
        }

        [HttpPost]
        public ActionResult GetTag(string quote)
        {
            List<Tag> tags = new List<Tag>();
            using (var db = new DataBaseConnection())
            {
               tags = db.Tags.Where(t => t.Name.StartsWith(quote)).ToList();
            }
            List<string> tagsStr = new List<string>();
            foreach (var tag in tags)
                tagsStr.Add(tag.Name);
           return Json(tagsStr);
        }

        [HttpPost]
        public string Upload(string data)
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> 28e1f9d133626d412c6e278820e45e02574cba7d
        {           
            CloudinaryDotNet.Account account = new CloudinaryDotNet.Account("ubakip-ru", "558288263223776", "IqzfFUQdOiwxYab-wi0a_ppyO-A"); 
            CloudinaryDotNet.Cloudinary cloudinary = new CloudinaryDotNet.Cloudinary(account); 
            CloudinaryDotNet.Actions.ImageUploadParams uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams() 
<<<<<<< HEAD
            {
                File = new CloudinaryDotNet.Actions.FileDescription(data)
            };
            CloudinaryDotNet.Actions.ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);                                                                                                                                         
            return cloudinary.Api.UrlImgUp.BuildUrl(String.Format("{0}.{1}", uploadResult.PublicId, uploadResult.Format));
        }

        private Page LoadPreviewPage(int pageId)
        {
            Page page = new Page();
            using (var db = new DataBaseConnection())
            {
                page = db.Pages.FirstOrDefault(p => p.Id == pageId);
            }
            if (page == null) { page = new Page();
                page.Preview = "No preview";
            }
            return page;
        }
        private void SaveTags(Comix comix)
        {
            using (var db = new DataBaseConnection())
            {
                DeleteOldTags(db.Comixes.Include(c=>c.Tags).FirstOrDefault(c=> c.Id== comix.Id), db);
                int count = comix.Tags.Count;
                for(int i=0; i<count; i++)
                {
                    Tag tag = ((List<Tag>)comix.Tags)[i];
                    Tag dbTag = db.Tags
                        .Include(t=> t.Comixes)
                        .FirstOrDefault(t => t.Name == tag.Name);                    
                    if (dbTag != null)
                    {
                        if(dbTag.Comixes != null && !dbTag.Comixes.Any(c=>c.Id == comix.Id))
                        {
                            dbTag.Count++;
                            dbTag.Comixes.Add(comix);
                            db.Entry<Tag>(dbTag).State = EntityState.Modified;
                        }                       
                    }
                    else
                    {
                       // tag.Comixes.Add(comix);
                        db.Tags.Add(tag);
                    }
                    db.Comixes.FirstOrDefault(c => c.Id == comix.Id).Tags.Add(tag);
                }               
                db.SaveChanges();
            }                
        }

        private void DeleteOldTags(Comix comix, DataBaseConnection db)
        {            
            List<Tag> oldTags = (List<Tag>)db.Comixes.Include(t => t.Tags).Include(c => c.Tags.Select(t => t.Comixes)).FirstOrDefault(c => c.Id == comix.Id).Tags;
            for(int i=0; i< oldTags.Count; i++) {
                oldTags[i].Count--;
                Tag tag = comix.Tags.FirstOrDefault(t => t.Id == oldTags[i].Id);
                tag = null;
                if (oldTags[i].Count == 0)
                {
                    db.Entry(oldTags[i]).State = System.Data.Entity.EntityState.Deleted;
                    i--;
                }                                      
             
            }
            db.SaveChanges();
=======
            {
                File = new CloudinaryDotNet.Actions.FileDescription(data)
            };
            CloudinaryDotNet.Actions.ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);                                                                                                                                         
            return cloudinary.Api.UrlImgUp.BuildUrl(String.Format("{0}.{1}", uploadResult.PublicId, uploadResult.Format));
        }

        private Page LoadPreviewPage(int pageId)
        {
            Page page = new Page();
            using (var db = new DataBaseConnection())
            {
                page = db.Pages.FirstOrDefault(p => p.Id == pageId);
            }
            if (page == null) { page = new Page();
                page.Preview = "No preview";
            }
            return page;
        }
        private void SaveTags(Comix comix)
        {
            using (var db = new DataBaseConnection())
            {
                DeleteOldTags(db.Comixes.Include(c=>c.Tags).FirstOrDefault(c=> c.Id== comix.Id), db);
                int count = comix.Tags.Count;
                for(int i=0; i<count; i++)
                {
                    Tag tag = ((List<Tag>)comix.Tags)[i];
                    Tag dbTag = db.Tags
                        .Include(t=> t.Comixes)
                        .FirstOrDefault(t => t.Name == tag.Name);                    
                    if (dbTag != null)
                    {
                        if(dbTag.Comixes != null && !dbTag.Comixes.Any(c=>c.Id == comix.Id))
                        {
                            dbTag.Count++;
                            dbTag.Comixes.Add(comix);
                            db.Entry<Tag>(dbTag).State = EntityState.Modified;
                        }                       
                    }
                    else
                    {
                       // tag.Comixes.Add(comix);
                        db.Tags.Add(tag);
                    }
                    db.Comixes.FirstOrDefault(c => c.Id == comix.Id).Tags.Add(tag);
                }               
                db.SaveChanges();
            }                
        }

        private void DeleteOldTags(Comix comix, DataBaseConnection db)
        {            
            List<Tag> oldTags = (List<Tag>)db.Comixes.Include(t => t.Tags).Include(c => c.Tags.Select(t => t.Comixes)).FirstOrDefault(c => c.Id == comix.Id).Tags;
            for(int i=0; i< oldTags.Count; i++) {
                oldTags[i].Count--;
                Tag tag = comix.Tags.FirstOrDefault(t => t.Id == oldTags[i].Id);
                tag = null;
                if (oldTags[i].Count == 0)
                {
                    db.Entry(oldTags[i]).State = System.Data.Entity.EntityState.Deleted;
                    i--;
                }                                      
             
            }
            db.SaveChanges();
=======
        {
            CloudinaryDotNet.Account account = new CloudinaryDotNet.Account("ubakip-ru", "558288263223776", "IqzfFUQdOiwxYab-wi0a_ppyO-A");
            CloudinaryDotNet.Cloudinary cloudinary = new CloudinaryDotNet.Cloudinary(account);
            CloudinaryDotNet.Actions.ImageUploadParams uploadParams = new CloudinaryDotNet.Actions.ImageUploadParams()
            {
                File = new CloudinaryDotNet.Actions.FileDescription(data)
            };
            CloudinaryDotNet.Actions.ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);
            string url = cloudinary.Api.UrlImgUp.BuildUrl(String.Format("{0}.{1}", uploadResult.PublicId, uploadResult.Format));
            return url;
>>>>>>> 305fb4c09f1f69fab00900089d3717f1754f112f
>>>>>>> 28e1f9d133626d412c6e278820e45e02574cba7d
        }

        private void SavePageToDatabase(Page page)
        {
            using (var db = new DataBaseConnection())
            {
                if (db.Pages.Any(o => o.Id == page.Id))
                    UpdatePage(page, db); 
                else
                    db.Pages.Add(page);
                db.SaveChanges();
            }
        }

        private void UpdatePage(Page newPage, DataBaseConnection db)
        {
            var oldPage = db.Pages
                      .Include(x => x.Clouds)
                      .Include(x => x.ImageCell)                     
                      .Single(c => c.Id == newPage.Id);
            db.Entry(oldPage).CurrentValues.SetValues(newPage);                  
        }

        private void SaveClouds(Page page)
        {
            DeleteEmptyClouds((List<Cloud>)page.Clouds);
            SaveCloudsToDatabase(page);
        }

        private void DeleteEmptyClouds(List<Cloud> clouds)
        {
            List<Cloud> cloudsToRemove = new List<Cloud>();
            for (int i = 0; i < clouds.Count; i++)
                if (clouds[i].text == null)
                {
                    cloudsToRemove.Add(clouds[i]);
                    clouds.RemoveAt(i);
                    i--;
                }
            RemoveCloudsFromDatabase(cloudsToRemove);
        }

        private void RemoveCloudsFromDatabase(List<Cloud> clouds)
        {
            using (var db = new DataBaseConnection())
            {                
                for (int i=0;i< clouds.Count; i++)               
                {
                    int cloudId = clouds[i].id;
                    Cloud cloudToRemove = db.Clouds.FirstOrDefault(c => c.id == cloudId);
                    if (cloudToRemove != null)
                    {
                        db.Clouds.Remove(cloudToRemove);
                        db.SaveChanges();
                        clouds.RemoveAt(0);
                        i--;
                    }
                }              
            }
        }

        private void RemoveImagesFromDatabase(List<ImageCell> images)
        {
            using (var db = new DataBaseConnection())
            {
                for (int i = 0; i < images.Count; i++)
                {
                    int imageId = images[i].id;
                    ImageCell imageToRemove = db.ImageCells.FirstOrDefault(c => c.id == imageId);
                    if (imageToRemove != null)
                    {
                        db.ImageCells.Remove(imageToRemove);
                        db.SaveChanges();
                        images.RemoveAt(0);
                        i--;
                    }
                }
            }
        }

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

        private void SaveCloudsToDatabase(Page page)
        {
            using (var db = new DataBaseConnection())
            {
                foreach (var cld in page.Clouds)
                    if (db.Clouds.Any(o => o.id == cld.id))
                        db.Entry<Cloud>(cld).State = EntityState.Modified;
                    else
                    {
                        cld.Page = db.Pages.FirstOrDefault(p => p.Id == page.Id);
                        db.Clouds.Add(cld);
                    }
                db.SaveChanges();
            }
        }
        
        private void SaveImages(Page page)
        {
            using (var db = new DataBaseConnection())
            {
                foreach (var img in page.ImageCell)
                {
                    if (db.ImageCells.Any(o => o.id == img.id))
                        db.Entry<ImageCell>(img).State = EntityState.Modified;
                    else
                    {
                        img.Page = db.Pages.FirstOrDefault(p=>p.Id==page.Id);
                        if(img.cellId!=null)
                        db.ImageCells.Add(img);
                    }
                }
                db.SaveChanges();
            }
        }

        private void SaveComixToDatabase(Comix comix)
        {
            using (var db = new DataBaseConnection())
            {
                if (db.Comixes.Any(o => o.Id == comix.Id))
                    UpdateComix(comix, db);
                else
                    db.Comixes.Add(comix);
                db.SaveChanges();
            }
            SaveTags(comix);
        }

        private void UpdateComix(Comix newComix, DataBaseConnection db)
        {           
            var oldComix = db.Comixes
                      .Include(x => x.Tags)
                      .Include(x => x.Pages)                   
                      .Include(x => x.CoverPage)
                      .Single(c => c.Id == newComix.Id);
            List<Page> pages = new List<Page>();
            foreach (var p in newComix.Pages)
                pages.Add(db.Pages.FirstOrDefault(pa => pa.Id == p.Id));
            oldComix.CoverPage = db.Pages.Where(p => p.Id == newComix.CoverPage.Id).FirstOrDefault();
            db.Entry(oldComix).CurrentValues.SetValues(newComix);             
        }
        
    }
}