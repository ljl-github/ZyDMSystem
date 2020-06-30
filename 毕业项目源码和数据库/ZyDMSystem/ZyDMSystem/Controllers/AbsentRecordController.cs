using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZyDMSystem.Models;

namespace ZyDMSystem.Controllers
{
    public class AbsentRecordController : Controller
    {
        // GET: AbsentRecord
        ZyDMSystemEntities db = new ZyDMSystemEntities();

        public static string name;
        // GET: Student
        //学生缺寝列表
        public ActionResult Index(string Name,int? page = null)
        {
            IPagedList<AbsentRecord> absentList = null;
            if ((Name != "" && Name != null) || name != null)
            {
                if (name == null)
                {
                    name = Name;
                }
                var aList = db.AbsentRecord.Where(s => ((s.Student.Name == Name || s.Student.Name == name))).OrderBy(s => s.Date).ToList();
                //第几页
                int pageNumber = page ?? 1;
                //每页显示5条
                int pageSize = 1;
                ViewBag.Name = name;
                absentList = aList.ToPagedList<AbsentRecord>(pageNumber, pageSize);
            }
            else
            {
                ViewBag.Name = name;
                var aList = db.AbsentRecord.Where(s => s.Student.Name.Contains("")).OrderBy(s=>s.Date).ToList();
                //第几页
                int pageNumber = page ?? 1;
                //每页显示5条
                int pageSize = 1;
                absentList = aList.ToPagedList<AbsentRecord>(pageNumber, pageSize);
            }
            return View(absentList);
        }
    }
}