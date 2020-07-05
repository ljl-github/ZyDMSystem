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
            List<Student> sList = db.Student.OrderBy(p => p.ClassID).ToList();
            //第几页
            int pageNumber = page ?? 1;
            //每页显示10条
            int pageSize = 10;
            IPagedList<Student> stuList = sList.ToPagedList<Student>(pageNumber, pageSize);
            return View(stuList);
        }
        [HttpPost]
        public ActionResult Index(string Name,int? page = null)
        {
            List<Student> sList = db.Student.Where(s => (s.Name.Contains(Name))||(s.Name==Name)).OrderBy(p => p.ClassID).ToList();
            //第几页
            int pageNumber = page ?? 1;
            //每页显示10条
            int pageSize = 10;
            IPagedList<Student> stuList = sList.ToPagedList<Student>(pageNumber, pageSize);
            return View(stuList);
        }
        //添加学生
        public ActionResult AddStu()
        {
            return View(db.Class.ToList());
        }
        [HttpPost]
        public ActionResult AddStu(Student student)
        {
            db.Student.Add(student);
            db.SaveChanges();
            return Content("<script>alert('添加成功！');location.href='/Student/Index';</script>");
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
        public ActionResult EditStu(Student student)
        {
            db.Entry(student).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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