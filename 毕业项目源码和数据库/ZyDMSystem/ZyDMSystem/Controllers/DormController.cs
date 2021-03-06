﻿using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZyDMSystem.Models;

namespace ZyDMSystem.Controllers
{
    public class DormController : Controller
    {
        // GET: Dorm
        ZyDMSystemEntities db = new ZyDMSystemEntities();
        public ActionResult Index(int? page = null)
        {
            var dList = db.Dorm.ToList();
            //第几页
            int pageNumber = page ?? 1;
            //每页显示10条
            int pageSize = 10;
            IPagedList<Dorm> absList = dList.ToPagedList<Dorm>(pageNumber, pageSize);
            return View(absList);
        }

        //分配宿舍
        public ActionResult DistributeDorm(int? id) 
        {
            ViewBag.Class = db.Class.ToList();
            ViewBag.Dormitory = db.Dormitory.ToList();
            return View(db.Student.Find(id));
        }
        [HttpPost]
        public ActionResult DistributeDorm(int ID,int DormID)
        {
            //学生
            var stu = db.Student.Find(ID);
            //宿舍
            var dorm = db.Dorm.Find(DormID);
            //已居住人数
            var pNumber = db.Student.Where(s => s.DormID == DormID).Count();

            //判断人数是否已满
            if (pNumber >= dorm.PNumber)
            {
                string errorMsg = "<script>alert('该宿舍居住人数已满,请重新选择！');location.href='/Dorm/DistributeDorm/"+ID+ "';</script>";
                return Content(errorMsg);
            }
            else
            {
                stu.DormID = DormID;
                stu.State = "已入住";
                //生成一条入住记录
                CheckIn checkIn = new CheckIn()
                {
                    StuID = ID,
                    Date = DateTime.Now
                };
                db.CheckIn.Add(checkIn);
                db.SaveChanges();
                return Content("<script>alert('操作成功！');location.href='/Student/Index';</script>");
            }
            
        }
        //取得宿舍列表
        [HttpPost]
        public ActionResult GetDormList(string dormitoryID)
        {
            int dID = Convert.ToInt32(dormitoryID);
            var dList = from m in db.Dorm
                        where m.DormitoryID == dID
                        select new
                        {
                            DormID = m.DormID,
                            DormNumber = m.DormNumber
                        };
            return Json(dList, JsonRequestBehavior.AllowGet);
        }

        //为楼宇添加宿舍
        public ActionResult ForAddDorm(int? id)
        {
            return View(db.Dormitory.Find(id));
        }
        [HttpPost]
        public ActionResult AddDorm(Dorm dorm)
        {
            var dor = db.Dorm.Where(d => d.DormNumber == dorm.DormNumber&&d.DormitoryID==dorm.DormitoryID).ToList();
            if (dor.Count>0)
            {
                //添加失败则返回提示并跳转到上一页
                string errorMsg = "<script>alert('该宿舍已存在！');history.go(-1);</script>";
                return Content(errorMsg);
            }
            else
            {
                //添加成功则返回到楼宇列表
                db.Dorm.Add(dorm);
                db.SaveChanges();
                return Content("<script>alert('添加成功！');location.href='/Dormitory/Index';</script>");
            }
        }

        //删除宿舍
        public ActionResult DeleteDorm(int? id)
        {
            var dorm = db.Dorm.Find(id);
            if (dorm.Student.Count > 0)
            {
                return Content("<script>alert('该宿舍有学生居住，不可删除！');history.go(-1);</script>");
            }
            else
            {
                db.Dorm.Remove(dorm);
                db.SaveChanges();
                return Content("<script>alert('删除成功！');location.href='/Dormitory/Index';</script>");
            }
        }
    }
}