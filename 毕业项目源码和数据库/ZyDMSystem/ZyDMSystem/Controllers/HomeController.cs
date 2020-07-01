using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZyDMSystem.Models;

namespace ZyDMSystem.Controllers
{
    public class HomeController : Controller
    {
        ZyDMSystemEntities db = new ZyDMSystemEntities();
        public ActionResult Index()
        {
            return View();
        }
        //退出
        public ActionResult Quit()
        {
            Session.Clear();
            return RedirectToAction("Login","Login");
        }
        //我的资料
        public ActionResult MyInfo(int? id)
        {
            object user=null;
            if (Session["admin"] != null)
            {
                user = db.Admin.Find(id);
            }
            if (Session["dormAdmin"] != null)
            {
                user = db.DormAdmin.Find(id);
            }
            if (Session["stu"] != null)
            {
                user = db.Student.Find(id);
            }
            return View(user);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}