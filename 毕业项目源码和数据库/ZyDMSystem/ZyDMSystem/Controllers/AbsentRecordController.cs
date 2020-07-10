using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using ZyDMSystem.Models;

namespace ZyDMSystem.Controllers
{
    public class AbsentRecordController : Controller
    {
        // GET: AbsentRecord
        ZyDMSystemEntities db = new ZyDMSystemEntities();

        //学生缺寝列表
        public ActionResult Index(int? page = null)
        {
            List<AbsentRecord> aList = db.AbsentRecord.OrderByDescending(a=>a.Date).ToList();
            //第几页
            int pageNumber = page ?? 1;
            //每页显示10条
            int pageSize = 10;
            IPagedList<AbsentRecord> absentList = aList.ToPagedList<AbsentRecord>(pageNumber, pageSize);
            return View(absentList);
        }
        [HttpPost]
        public ActionResult Index(string Name, int? page = null)
        {
            List<AbsentRecord> aList = db.AbsentRecord.Where(s => (s.Student.Name.Contains(Name)) || (s.Student.Name == Name)).ToList();
            //第几页
            int pageNumber = page ?? 1;
            //每页显示10条
            int pageSize = 10;
            IPagedList<AbsentRecord> absentList = aList.ToPagedList<AbsentRecord>(pageNumber, pageSize);
            return View(absentList);
        }
        //宿管查询缺寝列表
        public ActionResult DormitoryStuAbsentRecord(int id,int? page=null)
        {
            List<AbsentRecord> aList = db.AbsentRecord.Where(s =>s.Student.Dorm.DormitoryID==id).ToList();
            //第几页
            int pageNumber = page ?? 1;
            //每页显示10条
            int pageSize = 10;
            IPagedList<AbsentRecord> absentList = aList.ToPagedList<AbsentRecord>(pageNumber, pageSize);
            ViewBag.dormitoryID = id;
            return View(absentList);
        }
        [HttpPost]
        public ActionResult DormitoryStuAbsentRecord(int ID,string Name, int? page = null)
        {
            List<AbsentRecord> aList = db.AbsentRecord.Where(s => s.Student.Dorm.DormitoryID == ID&&(s.Student.Name.Contains(Name)||s.Student.Name==Name)).ToList();
            //第几页
            int pageNumber = page ?? 1;
            //每页显示10条
            int pageSize = 10;
            IPagedList<AbsentRecord> absentList = aList.ToPagedList<AbsentRecord>(pageNumber, pageSize);
            ViewBag.dormitoryID = ID;
            return View(absentList);
        }
        //学生缺寝
        public ActionResult AbsentRecordStu(int id)
        {
            ViewBag.dormitoryID = id;
            return View(db.Dorm.Where(d=>d.DormitoryID==id).ToList());
        }
        [HttpPost]
        public ActionResult AbsentRecordStu(AbsentRecord absent,int DormitoryID)
        {
            var stu = db.Student.Find(absent.StuID);
            if (stu.State == "已迁出") 
            {
                string errorMsg = "<script>alert('该学生已迁出！');location.href='/AbsentRecord/AbsentRecordStu/" + DormitoryID + "';</script>";
                return Content(errorMsg);
            }
            else
            {
                var date = absent.Date.ToShortDateString();
                var absentList = db.AbsentRecord.Where(a => a.StuID == absent.StuID).ToList();
                List<AbsentRecord> list = new List<AbsentRecord>();
                if (absentList.Count > 0)
                {
                    foreach (var item in absentList)
                    {
                        if (item.Date.ToShortDateString() == date)
                        {
                            list.Add(item);
                        }
                    }
                    if (list.Count > 0)
                    {
                        string errorMsg = "<script>alert('该学生今日已缺寝！');location.href='/AbsentRecord/AbsentRecordStu/" + DormitoryID + "';</script>";
                        return Content(errorMsg);
                    }
                    else
                    {
                        db.AbsentRecord.Add(absent);
                        db.SaveChanges();
                        string successMsg = "<script>alert('操作成功！');location.href='/AbsentRecord/DormitoryStuAbsentRecord/" + DormitoryID + "';</script>";
                        return Content(successMsg);
                    }
                }
                else
                {
                    db.AbsentRecord.Add(absent);
                    db.SaveChanges();
                    string successMsg = "<script>alert('操作成功！');location.href='/AbsentRecord/DormitoryStuAbsentRecord/" + DormitoryID + "';</script>";
                    return Content(successMsg);
                }
            }
        }
        //取得学生列表
        [HttpPost]
        public ActionResult GetStuList(string dormID)
        {
            int dID = Convert.ToInt32(dormID);
            var dList = from m in db.Student
                        where m.DormID == dID
                        select new
                        {
                            StuID = m.ID,
                            Name = m.Name
                        };
            return Json(dList, JsonRequestBehavior.AllowGet);
        }
        //学生 我的缺寝记录
        public ActionResult MyRecord(int id)
        {
            return View(db.AbsentRecord.Where(a=>a.StuID==id).ToList());
        }
    }
}