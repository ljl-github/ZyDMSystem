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
            List<AbsentRecord> aList = db.AbsentRecord.ToList();
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
    }
}