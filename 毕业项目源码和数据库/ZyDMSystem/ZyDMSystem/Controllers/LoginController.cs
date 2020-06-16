using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZyDMSystem.Models;

namespace ZyDMSystem.Controllers
{
    public class LoginController : Controller
    {
        ZyDMSystemEntities db = new ZyDMSystemEntities();

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(int Role,string Account,string Pwd)
        {
            if (Role == 0)
            {
                var admin = db.Admin.SingleOrDefault(a=>a.Account==Account&&a.Pwd==Pwd);
                if (admin != null)
                {
                    Session["admin"] = admin;
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ModelState.AddModelError("errorMsg","账号或密码错误，请重新输入！");
                    return View();
                }
            }
            else if(Role==1)
            {
                var DormAdmin = db.DormAdmin.SingleOrDefault(a => a.Account == Account && a.Pwd == Pwd);
                if (DormAdmin != null)
                {
                    Session["DormAdmin"] = DormAdmin;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("errorMsg", "账号或密码错误，请重新输入！");
                    return View();
                }
            }
            else
            {
                var stu = db.Student.SingleOrDefault(a => a.Account == Account && a.Pwd == Pwd);
                if (stu != null)
                {
                    Session["stu"] = stu;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("errorMsg", "账号或密码错误，请重新输入！");
                    return View();
                }
            }
        }
        public ActionResult Register()
        {
            return View();
        }
    }
}