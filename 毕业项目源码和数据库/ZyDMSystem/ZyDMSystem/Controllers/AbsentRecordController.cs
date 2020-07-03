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
            //每页显示5条
            int pageSize = 5;
            IPagedList<AbsentRecord> absentList = aList.ToPagedList<AbsentRecord>(pageNumber, pageSize);
            return View(absentList);
        }
        //[HttpPost]
        //public ActionResult Index(string Name,DateTime? Date,int? page = null)
        //{
        //    //创建一个AbsentRecord集合存数据
        //    List<AbsentRecord> abList=new List<AbsentRecord>();
        //    //取得AbsentRecord所有集合
        //    List<AbsentRecord> aList = db.AbsentRecord.ToList();
        //    List<AbsentRecord> absList = new List<AbsentRecord>();
        //    if (Date != null)
        //    { 
        //        //将传过来的时间转为yyyy-MM-dd短时间格式
        //        var date = DateTime.Parse(Date.ToString()).ToShortDateString();
        //        //循环判断时间并将符合的数据加入abList集合
        //        foreach (var item in aList)
        //        {
        //            if (item.Date.ToShortDateString()== date)
        //            {
        //                abList.Add(item);
        //            }
        //        }
        //        absList=abList.Where(a => a.Student.Name.Contains(Name)).ToList();
        //    }
        //    else
        //    {
        //        absList = db.AbsentRecord.Where(a => a.Student.Name.Contains(Name)).ToList();
        //    }
        //    //第几页
        //    int pageNumber = page ?? 1;
        //    //每页显示5条
        //    int pageSize = 5;
        //    IPagedList<AbsentRecord> absentList = absList.ToPagedList<AbsentRecord>(pageNumber, pageSize);
        //    return View(absentList);
        //}
    }
}