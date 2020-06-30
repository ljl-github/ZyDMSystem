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
        public static string name;
        // GET: Dormitory
        //楼宇列表
        public ActionResult Index(string Name, int? page = null)
        {
            IPagedList<Dormitory> dormList = null;
            if ((Name != "" && Name != null) || name != null)
            {
                if (name == null)
                {
                    name = Name;
                }
                var dList = db.Dormitory.Where(s => ((s.Name == Name || s.Name == name))).ToList();
                //第几页
                int pageNumber = page ?? 1;
                //每页显示5条
                int pageSize = 5;
                ViewBag.Name = name;
                dormList = dList.ToPagedList<Dormitory>(pageNumber, pageSize);
            }
            else
            {
                ViewBag.Name = name;
                var dList = db.Dormitory.Where(s => s.Name.Contains("")).ToList();
                //第几页
                int pageNumber = page ?? 1;
                //每页显示5条
                int pageSize = 5;
                dormList = dList.ToPagedList<Dormitory>(pageNumber, pageSize);
            }
            return View(dormList);
        }
    }
}