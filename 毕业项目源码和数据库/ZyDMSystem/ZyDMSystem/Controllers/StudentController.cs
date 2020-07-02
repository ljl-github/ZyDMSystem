using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZyDMSystem.Models;

namespace ZyDMSystem.Controllers
{
    public class StudentController : Controller
    {
        ZyDMSystemEntities db = new ZyDMSystemEntities();
        // GET: Student
        //学生列表
        public ActionResult Index(int? page = null)
        {
            List<Student> sList = db.Student.OrderByDescending(p => p.ClassID).ToList();
            //第几页
            int pageNumber = page ?? 1;
            //每页显示5条
            int pageSize = 5;
            IPagedList<Student> stuList = sList.ToPagedList<Student>(pageNumber, pageSize);
            return View(stuList);
        }
        [HttpPost]
        public ActionResult Index(string Name,int? page = null)
        {
            List<Student> sList = db.Student.Where(s => (s.Name.Contains(Name))||(s.Name==Name)).OrderByDescending(p => p.ClassID).ToList();
            //第几页
            int pageNumber = page ?? 1;
            //每页显示5条
            int pageSize = 5;
            IPagedList<Student> stuList = sList.ToPagedList<Student>(pageNumber, pageSize);
            return View(stuList);
        }
        //添加学生
        public ActionResult AddStu()
        {
            return View(db.Class.ToList());
        }
        [HttpPost]
        public ActionResult AddStu(HttpPostedFileBase Photo, Student student)
        {
            //获取图片文件名 截取判断后缀名
            string fileName = Path.GetFileName(Photo.FileName);
            string fileFormat = fileName.Substring(fileName.LastIndexOf('.') + 1);
            if (fileFormat != "jpg" && fileFormat != "jpeg" && fileFormat != "png")
            {
                ModelState.AddModelError("errorMsg", "图片格式不正确！");
                return View(db.Class.ToList());
            }
            else
            {
                //保存图片
                Photo.SaveAs(Server.MapPath("~/Content/image/") + fileName);
                student.Photo = fileName;
                db.Student.Add(student);
                db.SaveChanges();
                return Content("<script>alert('添加成功！');location.href='/Student/AddStu';</script>");
            }
        }

        //检查学号是否已存在
        public int CheckAccount(string Account)
        {
            var stu = db.Student.SingleOrDefault(s => s.Account == Account);
            if (stu != null)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        //编辑学生信息
        public ActionResult EditStu(int? id)
        {
            ViewBag.Class = db.Class.ToList();
            return View(db.Student.Find(id));
        }
        [HttpPost]
        public ActionResult EditStu(HttpPostedFileBase Photo, Student student)
        {
            var stu = db.Student.Find(student.ID);
            if (Photo != null)
            {
                //获取图片文件名 
                string fileName = Path.GetFileName(Photo.FileName);
                stu.Photo = fileName;
                stu.Name = student.Name;
                stu.Sex = student.Sex;
                stu.Pwd = student.Pwd;
                stu.Phone = student.Phone;
                stu.Address = student.Address;
                stu.ClassID = student.ClassID;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                student.Photo = stu.Photo;
                stu.Name = student.Name;
                stu.Sex = student.Sex;
                stu.Pwd = student.Pwd;
                stu.Phone = student.Phone;
                stu.Address = student.Address;
                stu.ClassID = student.ClassID;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        //替换图片
        [HttpPost]
        public ActionResult ReplaceImg()
        {
            //取得文件
            var file = Request.Files[0];
            //取得文件名
            string fileName = Path.GetFileName(file.FileName);
            //截取后缀名
            string fileFormat = Path.GetExtension(fileName);
            if (fileFormat != ".jpg" && fileFormat != ".jpeg" && fileFormat != ".png" && fileFormat != ".JPG" && fileFormat != ".JPEG")
            {
                return JavaScript("alert('图片格式不正确,请您重新选择！')");
            }
            else
            {
                //保存文件到本地
                file.SaveAs(Server.MapPath("~/Content/image/") + fileName);
                //将图片路径返回
                return Content("/Content/image/"+fileName);
            }
        }

        //删除学生
        public ActionResult DeleteStu(int? id)
        {
            var student = db.Student.Find(id);
            if (student.State == "已入住")
            {
                return Content("<script>alert('只能删除未入住或已迁出的学生！');location.href='/Student/Index';</script>");
            }
            else
            {
                db.Student.Remove(student);
                db.SaveChanges();
                return Content("<script>alert('删除成功！');location.href='/Student/Index';</script>");
            }
        }
    }
}