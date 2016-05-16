using System;
using System.Linq;
using System.Web.Mvc;
using ubakip.Models;
using System.Data.Entity;
using MultilingualSite.Filters;

namespace ubakip.Controllers
{
    [Culture]
    public class UserInfoController : Controller
    {
        public static string GetThumbanil(string pictureUrl)
        {
            return (pictureUrl.Substring(0, 49) + "c_fill,h_50,q_50,r_4,w_50/" + pictureUrl.Substring(49));
        }

        [HttpGet]
        public ActionResult Index(string name)
        {

            Medal medal = new Medal()
            {
                Description = "dsfkjsdklf",
                Photo = "/Content/Images/medal.png"
            };
            UserProfile author = new UserProfile()
            {
                UserInfo = new UserInfo() { Rating = 3.4f },
                User = new Users() { }
            };

            /*User's Info*/
            using (var db = new DataBaseConnection())
            {
                if (name == null) name = User.Identity.Name;
                var id = db.Users.Where(u => u.Login == name).FirstOrDefault().Id;
                var photo = db.Users.Where(u => u.Id == id).FirstOrDefault().Photo;
                var firstname = db.Users.Where(u => u.Id == id).FirstOrDefault().FirstName;
                var lastname = db.Users.Where(u => u.Id == id).FirstOrDefault().LastName;
                var userinfoes = db.UsersInfoes.Where(u => u.Id == id).FirstOrDefault();

                author.User.Id = id;

                /*Names and login*/
                if (firstname != null && lastname != null)
                {
                    author.User.FirstName = firstname;
                    author.User.LastName = lastname;
                    author.User.Login = User.Identity.Name;
                }
                else
                    author.User.FirstName = name;

                    author.User.Photo = photo;

                /*About*/
                if (userinfoes != null && userinfoes.About != null)
                    author.UserInfo.About = userinfoes.About;
                else
                    author.UserInfo.About = "About user";

                /*Comments*/
                author.Comments = (db.Comments.Include(c => c.FromUser)).OrderByDescending(d => d.DateCreated).ToList();
                foreach(var comment in author.Comments)
                {
                    var ph = db.Users.Where(u => u.Id == comment.FromUser.Id).FirstOrDefault().Photo;
                    if (ph != null)
                        comment.FromUser.Photo = GetThumbanil(ph);
                    else
                        comment.FromUser.Photo = GetThumbanil("http://res.cloudinary.com/ubakip-ru/image/upload/v1463169215/nouser.jpg");
                }

            }

            for (int i = 0; i < 10; i++)
                author.Medals.Add(medal);

            return View(author);
        }

        [HttpPost]
        public JsonResult SendComment(string text, int toUserId)
        {
            using (var db = new DataBaseConnection())
            {
                if (text != null && text.Length > 4)
                {
                    var comment = db.Comments.Create();
                    comment.DateCreated = DateTime.Now;
                    comment.Text = text;
                    comment.FromUser = db.Users.Where(u => u.Login == User.Identity.Name).FirstOrDefault();
                    comment.ToUser = toUserId;
                    db.Comments.Add(comment);
                    db.SaveChanges();
                    Random rnd = new Random();
                    comment.Id = rnd.Next(2, 100);
                    return Json(comment);
                }
                else
                    return null;
            }
        }

        [HttpGet]
        public ActionResult Edit()
        {
            return View();
        }
    }
}