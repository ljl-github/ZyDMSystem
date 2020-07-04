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
            //每页显示5条
            int pageSize = 5;
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
        public ActionResult EditDormAdmin(HttpPostedFileBase Photo, DormAdmin dormAdmin)
        {
            var adm = db.DormAdmin.Find(dormAdmin.ID);
            if (Photo != null)
            {
                //获取图片文件名 
                string fileName = Path.GetFileName(Photo.FileName);
                adm.Photo = fileName;
                adm.Name = dormAdmin.Name;
                adm.Sex = dormAdmin.Sex;
                adm.Pwd = dormAdmin.Pwd;
                adm.Phone = dormAdmin.Phone;
                adm.Address = dormAdmin.Address;
                adm.DormitoryID = dormAdmin.DormitoryID;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                dormAdmin.Photo = adm.Photo;
                adm.Name = dormAdmin.Name;
                adm.Sex = dormAdmin.Sex;
                adm.Pwd = dormAdmin.Pwd;
                adm.Phone = dormAdmin.Phone;
                adm.Address = dormAdmin.Address;
                adm.DormitoryID = dormAdmin.DormitoryID;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        //添加
        public ActionResult AddDormAdmin()
        {
            return View(db.Dormitory.ToList());
        }
        [HttpPost]
        public ActionResult AddDormAdmin(HttpPostedFileBase Photo, DormAdmin dormAdmin)
        {
            //获取图片文件名 截取判断后缀名
            string fileName = Path.GetFileName(Photo.FileName);
            string fileFormat = fileName.Substring(fileName.LastIndexOf('.') + 1);
            if (fileFormat != "jpg" && fileFormat != "jpeg" && fileFormat != "png" && fileFormat != "JPG" && fileFormat != "JPEG")
            {
                ModelState.AddModelError("errorMsg", "图片格式不正确！");
                return View(db.Dormitory.ToList());
            }
            else
            {
                //保存图片
                Photo.SaveAs(Server.MapPath("~/Content/image/") + fileName);
                dormAdmin.Photo = fileName;
                db.DormAdmin.Add(dormAdmin);
                db.SaveChanges();
                return Content("<script>alert('添加成功！');location.href='/DormAdmin/Index';</script>");
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
    }
}