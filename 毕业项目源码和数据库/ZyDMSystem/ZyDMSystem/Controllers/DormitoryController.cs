using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZyDMSystem.Models;

namespace ZyDMSystem.Controllers
{
    public class DormitoryController : Controller
    {
        ZyDMSystemEntities db = new ZyDMSystemEntities();
        public static string name;
        // GET: Dormitory
        //楼宇列表
        public ActionResult Index(string Name, int? page = null)
        {
            IPagedList<Dormitory> dormList = null;
            if ((Name != "" && Name != null) || name != null)
            {
                if (name == null)
                {
                    name = Name;
                }
                var dList = db.Dormitory.Where(s => ((s.Name == Name || s.Name == name))).ToList();
                //第几页
                int pageNumber = page ?? 1;
                //每页显示5条
                int pageSize = 5;
                ViewBag.Name = name;
                dormList = dList.ToPagedList<Dormitory>(pageNumber, pageSize);
            }
            else
            {
                ViewBag.Name = name;
                var dList = db.Dormitory.Where(s => s.Name.Contains("")).ToList();
                //第几页
                int pageNumber = page ?? 1;
                //每页显示5条
                int pageSize = 5;
                dormList = dList.ToPagedList<Dormitory>(pageNumber, pageSize);
            }
            return View(dormList);
        }
        //设置楼宇管理员
        public ActionResult SetDormAdmin(int? id) 
        {
            //所有楼宇管理员
            ViewBag.dAdmin = db.DormAdmin.ToList();
            //该楼宇管理员
            ViewBag.dormAdmin = db.DormAdmin.Where(a=>a.DormitoryID==id).ToList();
            return View(db.Dormitory.Find(id));
        }
        [HttpPost]
        public ActionResult SetDormAdmin(int[] DormAdminID,int DormitoryID)
        {
            if (DormAdminID != null)
            {
                foreach (var admin in DormAdminID)
                {
                    var dAdmin = db.DormAdmin.Find(admin);
                    dAdmin.DormitoryID = DormitoryID;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var admin in db.DormAdmin.ToList())
                {
                    admin.DormitoryID = null;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        //添加楼宇
        [HttpPost]
        //public int AddDorm(string Name)
        //{
        //    var dormi = db.Dormitory.Where(a => a.Name == Name).ToList();
        //    if (dormi.Count > 0)
        //    {
        //        return 0;
        //    }
        //    else
        //    {
        //        var dormitory = new Dormitory() { Name = Name };
        //        db.Dormitory.Add(dormitory);
        //        db.SaveChanges();
        //        return 1;
        //    }
        //}
        public ActionResult AddDorm(Dormitory dormitory)
        {
            var dormi = db.Dormitory.Where(a => a.Name == dormitory.Name).ToList();
            if (dormi.Count > 0)
            {
                return Content("<script>alert('该楼宇已存在！');history.go(-1)</script>");
            }
            else
            {
                db.Dormitory.Add(dormitory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}