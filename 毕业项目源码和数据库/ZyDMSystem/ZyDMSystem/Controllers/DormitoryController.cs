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
        // GET: Dormitory
        //楼宇列表
        public ActionResult Index(int? page = null)
        {
            List<Dormitory> dList = db.Dormitory.OrderBy(p => p.DormitoryID).ToList();
            //第几页
            int pageNumber = page ?? 1;
            //每页显示5条
            int pageSize = 5;
            IPagedList<Dormitory> dormList = dList.ToPagedList<Dormitory>(pageNumber, pageSize);
            return View(dormList);
        }
        [HttpPost]
        public ActionResult Index(string Name, int? page = null)
        {
            List<Dormitory> dList = db.Dormitory.Where(s => (s.Name.Contains(Name)) || (s.Name == Name)).OrderBy(p => p.DormitoryID).ToList();
            //第几页
            int pageNumber = page ?? 1;
            //每页显示5条
            int pageSize = 5;
            IPagedList<Dormitory> dormList = dList.ToPagedList<Dormitory>(pageNumber, pageSize);
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
                var adminList=db.DormAdmin.Where(a=>a.DormitoryID==DormitoryID).ToList();
                foreach (var admin in adminList)
                {
                    admin.DormitoryID = null;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        //添加楼宇
        [HttpPost]
        public ActionResult AddDorm(Dormitory dormitory)
        {
            var dormi = db.Dormitory.Where(a => a.Name == dormitory.Name).ToList();
            if (dormi.Count > 0)
            {
                return Content("<script>alert('该楼宇已存在！');location.href='/Dormitory/Index';</script>");
            }
            else
            {
                db.Dormitory.Add(dormitory);
                db.SaveChanges();
                return Content("<script>alert('添加成功！');location.href='/Dormitory/Index';</script>");
            }
        }

        //楼宇信息
        public ActionResult DormitoryDetail(int? id)
        {
            var dormitory = db.Dormitory.Find(id);
            return Json(dormitory);
        }
        [HttpPost]
        public ActionResult EditDormitory(Dormitory dormitory)
        {
            db.Entry(dormitory).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Content("<script>alert('编辑成功！');history.go(-1);</script>");
        }
    }
}