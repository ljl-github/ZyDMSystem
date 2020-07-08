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

        //个人中心
        public ActionResult Personal(int id)
        {
            ViewBag.ID = id;
            return View();
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

        //修改资料
        [HttpPost]
        public int UpdateInfo(int id,string Phone,string Address)
        {
            if (Session["admin"] != null)
            {
                var admin = db.Admin.Find(id);
                admin.Phone = Phone;
                admin.Address = Address;
                db.SaveChanges();
                return 1;
            }
            if (Session["dormAdmin"] != null)
            {
                var dormAdmin = db.DormAdmin.Find(id);
                dormAdmin.Phone = Phone;
                dormAdmin.Address = Address;
                db.SaveChanges();
                return 1;
            }
            if (Session["stu"] != null)
            {
                var stu = db.Student.Find(id);
                stu.Phone = Phone;
                stu.Address = Address;
                db.SaveChanges();
                return 1;
            }
            return 0;
        }
        //修改密码
        public ActionResult ReplacePwd(int id)
        {
            object user = null;
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
        [HttpPost]
        public int ReplacePwd(int ID, string Pwd, string newPwd)
        {
            
            if (Session["admin"] != null)
            {
                var admin = db.Admin.Find(ID);
                if (Pwd != admin.Pwd)
                {
                    return 0;
                }
                else
                {
                    admin.Pwd = newPwd;
                    db.SaveChanges();
                    return 1;
                }
            }
            if (Session["dormAdmin"] != null)
            {
                var dormAdmin = db.DormAdmin.Find(ID);
                if (Pwd != dormAdmin.Pwd)
                {
                    return 0;
                }
                else
                {
                    dormAdmin.Pwd = newPwd;
                    db.SaveChanges();
                    return 1;
                }
            }
            if (Session["stu"] != null)
            {
                var stu = db.Student.Find(ID);
                if (Pwd != stu.Pwd)
                {
                    return 0;
                }
                else
                {
                    stu.Pwd = newPwd;
                    db.SaveChanges();
                    return 1;
                }
            }
            else
            {
                return 0;
            }
        }


        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}