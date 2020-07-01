using System;
using System.Collections.Generic;
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
        public ActionResult Index()
        {
            return View(db.DormAdmin.ToList());
        }
    }
}