using System;
using System.Collections.Generic;
using System.IO;
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
            //每次进入登录界面是清空session
            Session.Clear();

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
                    Session["dormAdmin"] = DormAdmin;
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
                    if (stu.State == "已迁出") 
                    {
                        ModelState.AddModelError("errorMsg", "您已迁出！");
                        return View();
                    }
                    else
                    {
                        if (stu.DormID == null)
                        {
                            //string msg = "<script>alert('请进行入住登记！');location.href='/CheckIn/StuCheckIn/"+stu.ID+"';</script>";
                            //return Content(msg);
                            string msg = "<script>alert('您还未入住，请进行入住登记后再使用！');location.href='/Login/Login/';</script>";
                            return Content(msg);
                        }
                        else
                        {
                            Session["stu"] = stu;
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("errorMsg", "账号或密码错误，请重新输入！");
                    return View();
                }
            }
        }
        //public ActionResult Register()
        //{
        //    return View(db.Class.ToList());
        //}
        //[HttpPost]
        //public ActionResult Register(HttpPostedFileBase Photo,Student student)
        //{
        //    var stu = db.Student.SingleOrDefault(s=>s.Account==student.Account);
        //    if (stu != null)
        //    {
        //        ModelState.AddModelError("errorMsg", "该账号已被注册！");
        //        return View(db.Class.ToList());
        //    }
        //    else
        //    {
        //        //获取图片文件名 截取判断后缀名
        //        string fileName = Path.GetFileName(Photo.FileName);
        //        string fileFormat = fileName.Substring(fileName.LastIndexOf('.')+1);
        //        if (fileFormat != "jpg" && fileFormat != "jpeg" && fileFormat != "png")
        //        {
        //            ModelState.AddModelError("errorMsg", "图片格式不正确！");
        //            return View(db.Class.ToList());
        //        }
        //        else
        //        {
        //            //保存图片
        //            Photo.SaveAs(Server.MapPath("~/Content/image/")+fileName);
        //            student.Photo = fileName;
        //            db.Student.Add(student);
        //            db.SaveChanges();
        //            return Content("<script>alert('注册成功！');location.href='/Login/Login';</script>");
        //        }
        //    }
        //}
    }
}