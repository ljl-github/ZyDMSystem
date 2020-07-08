using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZyDMSystem.Models;
using ZyDMSystem.BLL;

namespace ZyDMSystem.Controllers
{
    public class DormitoryController : Controller
    {
        ZyDMSystemEntities db = new ZyDMSystemEntities();
        // GET: Dormitory
        //楼宇列表
        public ActionResult Index()
        {
            return View(db.Dormitory.ToList());
        }
        //设置楼宇管理员
        public ActionResult SetDormAdmin(int? id) 
        {
            //所有楼宇管理员
            ViewBag.dAdmin = db.DormAdmin.ToList();
            //该楼宇管理员
            ViewBag.dormAdmin = db.DormAdmin.Where(a=>a.DormitoryID==id).ToList();
            return View(db.Dormitory.Find(id));
        }
        [HttpPost]
        public ActionResult SetDormAdmin(int[] DormAdminID,int DormitoryID)
        {
            if (DormAdminID != null)
            {
                foreach (var admin in DormAdminID)
                {
                    var dAdmin = db.DormAdmin.Find(admin);
                    dAdmin.DormitoryID = DormitoryID;
                }
                db.SaveChanges();
                return Content("<script>alert('设置成功！');location.href='/Dormitory/Index';</script>");
            }
            else
            {
                var adminList=db.DormAdmin.Where(a=>a.DormitoryID==DormitoryID).ToList();
                foreach (var admin in adminList)
                {
                    admin.DormitoryID = null;
                }
                db.SaveChanges();
                return Content("<script>alert('设置成功！');location.href='/Dormitory/Index';</script>");
            }
        }
        //添加楼宇
        [HttpPost]
        public ActionResult AddDorm(Dormitory dormitory)
        {
            var dormi = db.Dormitory.Where(a => a.Name == dormitory.Name).ToList();
            if (dormi.Count > 0)
            {
                return Content("<script>alert('该楼宇已存在！');location.href='/Dormitory/Index';</script>");
            }
            else
            {
                db.Dormitory.Add(dormitory);
                db.SaveChanges();
                //添加成功则返回提示并跳转到添加宿舍界面
                string successMsg = "<script>alert('添加成功,请您为该楼宇添加宿舍！');location.href='/Dorm/ForAddDorm/" + dormitory.DormitoryID + "';</script>";
                return Content(successMsg);
            }
        }
        //楼宇信息
        public ActionResult DormitoryDetail(int? id)
        {
            var dormitory = db.Dormitory.Find(id);
            return Json(dormitory);
        }
        [HttpPost]
        public ActionResult EditDormitory(Dormitory dormitory)
        {
            db.Entry(dormitory).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Content("<script>alert('编辑成功！');history.go(-1);</script>");
        }
        //Excel批量导入宿舍
        [HttpPost]
        public ActionResult ExcelToUpload(int id)
        {  
            //用来存储excel表中读出来的数据
            DataTable excelTable = new DataTable();
            string msg = "";
            if (Request.Files.Count > 0) //request.files.count客户端传过来几个文件
            {
                try
                {
                    HttpPostedFileBase mypost = Request.Files[0];//取客户端传过来多个文件的第一个
                    string fileName = Path.GetFileName(mypost.FileName);//通过文件流取文件名称
                    string serverpath = Server.MapPath(
                    string.Format("~/{0}", "Excel")); //获取要存入的服务器上的地址
                    string path = Path.Combine(serverpath, fileName); //将定义的服务器路径和文件名结合，形成完整路径
                    mypost.SaveAs(path); //将文件保存
                    //msg = "文件上传成功！";
                    excelTable = ImportExcel.GetExcelDataTable(path);//获得表数据
                    msg = SaveExcelToDB.InsertDataToDB(excelTable, id);// 写入基础数据

                }
                catch (Exception ex)
                {
                    msg = "Excel导入失败，请检查匹配";
                }
            }
            else
            {
                msg = "请选择文件";
            }
            return Json(msg);
        }
    }
}