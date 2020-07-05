using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZyDMSystem.Models;

namespace ZyDMSystem.Controllers
{
    public class DormAdminController : Controller
    {
        // GET: DormAdmin
        ZyDMSystemEntities db = new ZyDMSystemEntities();
        //楼宇管理员列表
        public ActionResult Index(int? page = null)
        {
            List<DormAdmin> dList = db.DormAdmin.ToList();
            //第几页
            int pageNumber = page ?? 1;
            //每页显示10条
            int pageSize = 10;
            IPagedList<DormAdmin> dAdminList = dList.ToPagedList<DormAdmin>(pageNumber, pageSize);
            return View(dAdminList);
        }
        [HttpPost]
        public ActionResult Index(string Name, int? page = null)
        {
            List<DormAdmin> dList = db.DormAdmin.Where(s => (s.Name.Contains(Name)) || (s.Name == Name)).ToList();
            //第几页
            int pageNumber = page ?? 1;
            //每页显示10条
            int pageSize = 10;
            IPagedList<DormAdmin> dAdminList = dList.ToPagedList<DormAdmin>(pageNumber, pageSize);
            return View(dAdminList);
        }
        //编辑
        public ActionResult EditDormAdmin(int? id)
        {
            ViewBag.dormitory = db.Dormitory.ToList();
            return View(db.DormAdmin.Find(id));
        }
        [HttpPost]
        public ActionResult EditDormAdmin(DormAdmin dormAdmin)
        {
            db.Entry(dormAdmin).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //添加
        public ActionResult AddDormAdmin()
        {
            return View(db.Dormitory.ToList());
        }
        [HttpPost]
        public ActionResult AddDormAdmin(DormAdmin dormAdmin)
        {
            if (ModelState.IsValid)
            {
                db.DormAdmin.Add(dormAdmin);
                db.SaveChanges();
                return Content("<script>alert('添加成功！');location.href='/DormAdmin/Index';</script>");
            }
            else
            {
                return Content("<script>alert('添加失败！');location.href='/DormAdmin/Index';</script>");
            }
        }
        //检查工号
        public int CheckAccount(string Account)
        {
            var stu = db.DormAdmin.SingleOrDefault(s => s.Account == Account);
            if (stu != null)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        //删除楼宇管理员
        public ActionResult DeleteDormAdmin(int? id)
        {
            var dAdmin = db.DormAdmin.Find(id);
            db.DormAdmin.Remove(dAdmin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}