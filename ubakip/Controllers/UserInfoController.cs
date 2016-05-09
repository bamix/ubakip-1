using System;
using System.Web.Mvc;
using ubakip.Models;

namespace ubakip.Controllers
{
    public class UserInfoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult _CommentsPartial()
        {
            return View();
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
                        ViewBag.Message = "Error creating a comment. Read posting conditions.";
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