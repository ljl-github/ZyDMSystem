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
            //每页显示5条
            int pageSize = 5;
            IPagedList<Dorm> absList = dList.ToPagedList<Dorm>(pageNumber, pageSize);
            return View(absList);
        }
    }
}