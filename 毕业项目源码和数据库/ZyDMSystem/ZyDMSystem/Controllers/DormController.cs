using PagedList;
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
                return Content("<script>alert('该宿舍居住人数已满,请重新选择！');history.go(-1);</script>");
            }
            else
            {
                stu.DormID = DormID;
                stu.State = "已入住";
                //生成一条入住记录
                CheckIn checkIn = new CheckIn()
                {
                    StuID = ID,
                    DormID = DormID,
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
            Dorm s = new Dorm();
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
    }
}