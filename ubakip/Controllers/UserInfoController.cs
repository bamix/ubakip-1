using System;
using System.Linq;
using System.Web.Mvc;
using ubakip.Models;
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
        public ActionResult Index()
        {
            Comment comment = new Comment()
            {
                Text = "123",
                Id = 1,
                DateCreated = DateTime.Now,
               // FromUserId = new Users() { Login = "bamix", Photo = GetThumbanil("http://res.cloudinary.com/ubakip-ru/image/upload/v1463227450/3DClFMPdBSk_eb8izm.jpg") }
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
                    About = "kek",
                    Rating = 3.4f
                },
                User = new Users() { }
            };

            /*User's Info*/
            using (var db = new DataBaseConnection())
            {
                var id = db.Users.Where(u => u.Login == User.Identity.Name).FirstOrDefault().Id;
                var photo = db.Users.Where(u => u.Id == id).FirstOrDefault().Photo;
                var firstname = db.Users.Where(u => u.Id == id).FirstOrDefault().FirstName;
                var lastname = db.Users.Where(u => u.Id == id).FirstOrDefault().LastName;

                author.User.Id = id;

                /*Names and login*/
                if (firstname != null && lastname != null)
                {
                    author.User.FirstName = firstname;
                    author.User.LastName = lastname;
                    author.User.Login = User.Identity.Name;
                }
                else
                    author.User.FirstName = User.Identity.Name;

                /*Photo*/
                if (photo == null)
                    author.User.Photo = "http://res.cloudinary.com/ubakip-ru/image/upload/v1463169215/nouser.jpg"; // Blank Picture
                else
                    author.User.Photo = photo;
            }

            for (int i = 0; i < 10; i++)
            {
                author.Medals.Add(medal);
                author.Comments.Add(comment);
            }

            return View(author);
        }

        [HttpPost]
        public JsonResult SendComment(string text, int toUserId)
        {
            using (var db = new DataBaseConnection())
            {
                if (text != null && text.Length > 4)
                {
                    //  var comment = db.Comments.Create();
                    Comment comment = new Comment();
                    comment.DateCreated = DateTime.Now;
                    comment.Text = text;
                    // TODO add fromUser and toUserId 
                    // db.Comments.Add(comment);
                    // db.SaveChanges();
                    comment.FromUser = db.Users.Where(u => u.Login == User.Identity.Name).FirstOrDefault();
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
        public ActionResult _CommentsPartial(Comment model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new DataBaseConnection())
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

        [HttpPost]
        public string Upload(string data)
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
        }

        public ActionResult Edit()
        {
            return View();
        }
    }
}