using System;
using System.Web;
using System.Web.Mvc;
using ubakip.Models;

namespace ubakip.Controllers
{
    public class UserInfoController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            Comments comment = new Comments()
            {
                Text = "123",
                Id = 1,
                DateCreated = DateTime.Now,
                FromUser = new Users() { Name = "bamix" , Photo = "https://pp.vk.me/c630516/v630516851/17d41/3DClFMPdBSk.jpg" }
            };

            Medal medal = new Medal()
            {
                Description = "dsfkjsdklf",
                Photo = "/Content/Images/medal.png"
            };
            UserProfile author = new UserProfile()
            {
                UserInfo = new UserInfo()
                {
                    FirstName = "Alex",
                    LastName = "Ignatyev",
                    About = "kek",
                    Rating = 3.4f
                },
                User = new Users()
                {
                    Id = 123,
                    Name = "testName",
                    Photo = "https://pp.vk.me/c630516/v630516851/17d41/3DClFMPdBSk.jpg"
                }
            };

            author.Medals.Add(medal);
            author.Medals.Add(medal);
            author.Medals.Add(medal);
            author.Medals.Add(medal);
            author.Medals.Add(medal);
            author.Medals.Add(medal);
            author.Medals.Add(medal);
            author.Medals.Add(medal);
            author.Medals.Add(medal);
            author.Medals.Add(medal);
            author.Comments.Add(comment);
            author.Comments.Add(comment);
            author.Comments.Add(comment);
            author.Comments.Add(comment);
            author.Comments.Add(comment);
            author.Comments.Add(comment);
            return View(author);
        }

        [HttpPost]
        public JsonResult SendComment(string text, int toUserId)
        {          
          using (var db = new MainDbContext())
                {
                    if (text != null && text.Length > 4)
                    {
                    //  var comment = db.Comments.Create();
                    Comments comment = new Comments();
                        comment.DateCreated = DateTime.Now;
                        comment.Text = text;
                    // TODO add fromUser and toUserId 
                    // db.Comments.Add(comment);
                    // db.SaveChanges();
                    comment.FromUser = new Users() { Name = "testname2", Photo = "http://podrobnosti.ua/media/pictures/2016/1/16/thumbs/740x415/di-kaprio-javljaetsja-na-opredelennuju-dolju-russkim_rect_73964f9ad23fe44bdccc31d53472a745.jpg" };
                    Random rnd = new Random();
                    comment.Id = rnd.Next(2, 100);                  
                   return Json(comment);                           
                    }
                    else
                    {
                        return null;
                    }
                }          
        }

        [HttpPost]
        public ActionResult _CommentsPartial(Comments model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new MainDbContext())
                {
                    if (model.Text != null && model.Text.Length > 4)
                    {
                        var comment = db.Comments.Create();
                        comment.DateCreated = DateTime.Now;
                        comment.Text = model.Text;
                        db.Comments.Add(comment);
                        db.SaveChanges();
                        return RedirectToAction("Index", "UserInfo");
                    }
                    else
                    {
                        return RedirectToAction("Index", "UserInfo");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Something goes wrong");
            }
            return View(model);
        }
    }
}