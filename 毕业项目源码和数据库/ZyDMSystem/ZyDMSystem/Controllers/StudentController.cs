using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZyDMSystem.BLL;
using ZyDMSystem.Models;

namespace ZyDMSystem.Controllers
{
    public class StudentController : Controller
    {
        ZyDMSystemEntities db = new ZyDMSystemEntities();
        // GET: Student
        //学生列表
        public ActionResult Index(int? page = null)
        {
            List<Student> sList = db.Student.OrderBy(p => p.ClassID).ToList();
            //第几页
            int pageNumber = page ?? 1;
            //每页显示10条
            int pageSize = 10;
            IPagedList<Student> stuList = sList.ToPagedList<Student>(pageNumber, pageSize);
            return View(stuList);
        }
        [HttpPost]
        public ActionResult Index(string Name,int? page = null)
        {
            List<Student> sList = db.Student.Where(s => (s.Name.Contains(Name))||(s.Name==Name)).OrderBy(p => p.ClassID).ToList();
            //第几页
            int pageNumber = page ?? 1;
            //每页显示10条
            int pageSize = 10;
            IPagedList<Student> stuList = sList.ToPagedList<Student>(pageNumber, pageSize);
            return View(stuList);
        }
        //宿管查询管理楼宇的学生
        public ActionResult DormitoryStu(int id,int? page = null)
        {
            List<Student> sList = db.Student.Where(s => s.Dorm.DormitoryID==id).OrderBy(p => p.ClassID).ToList();
            //第几页
            int pageNumber = page ?? 1;
            //每页显示10条
            int pageSize = 10;
            IPagedList<Student> stuList = sList.ToPagedList<Student>(pageNumber, pageSize);
            ViewBag.dormitoryID = id;
            return View(stuList);
        }
        [HttpPost]
        public ActionResult DormitoryStu(int ID,string Name, int? page = null)
        {
            List<Student> sList = db.Student.Where(s => s.Dorm.DormitoryID == ID&&(s.Name.Contains(Name)||s.Name==Name)).OrderBy(p => p.ClassID).ToList();
            //第几页
            int pageNumber = page ?? 1;
            //每页显示10条
            int pageSize = 10;
            IPagedList<Student> stuList = sList.ToPagedList<Student>(pageNumber, pageSize);
            ViewBag.dormitoryID = ID;
            return View(stuList);
        }
        //添加学生
        public ActionResult AddStu()
        {
            return View(db.Class.ToList());
        }
        [HttpPost]
        public ActionResult AddStu(Student student)
        {
            var stu = db.Student.Where(s=>s.Account==student.Account).SingleOrDefault();
            if (stu != null) 
            {
                return Content("<script>alert('添加失败！请检查您输入的信息是否有误！');history.go(-1);</script>");
            }
            else
            {
                db.Student.Add(student);
                db.SaveChanges();
                return Content("<script>alert('添加成功！');location.href='/Student/Index';</script>");
            }
        }

        //检查学号是否已存在
        public int CheckAccount(string Account)
        {
            var stu = db.Student.SingleOrDefault(s => s.Account == Account);
            if (stu != null)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        //编辑学生信息
        public ActionResult EditStu(int? id)
        {
            ViewBag.Class = db.Class.ToList();
            return View(db.Student.Find(id));
        }
        [HttpPost]
        public ActionResult EditStu(Student student)
        {
            db.Entry(student).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //删除学生
        public ActionResult DeleteStu(int? id)
        {
            var student = db.Student.Find(id);
            if (student.State == "已入住")
            {
                return Content("<script>alert('只能删除未入住或已迁出的学生！');location.href='/Student/Index';</script>");
            }
            else
            {
                db.Student.Remove(student);
                db.SaveChanges();
                return Content("<script>alert('删除成功！');location.href='/Student/Index';</script>");
            }
        }

        //批量导入
        public ActionResult ExcelToUpload()
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
                    msg = SaveExcelToDB.InsertDataToStuDB(excelTable);// 写入基础数据

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
        //导出学生信息excel
        public FileResult StuExcel()
        {
            //获取list数据
            var list = db.Student.Where(s => s.DormID != null).ToList();

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");

            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("姓名");
            row1.CreateCell(1).SetCellValue("性别");
            row1.CreateCell(2).SetCellValue("班级");
            row1.CreateCell(3).SetCellValue("学号");
            row1.CreateCell(4).SetCellValue("电话");
            row1.CreateCell(5).SetCellValue("住址");
            //将数据逐步写入sheet1各个行
            for (int i = 0; i < list.Count; i++)
            {
                string sex;
                if (list[i].Sex == true)
                {
                    sex = "男";
                }
                else
                {
                    sex = "女";
                }
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(list[i].Name);
                rowtemp.CreateCell(1).SetCellValue(sex);
                rowtemp.CreateCell(2).SetCellValue(list[i].Class.ClassName);
                rowtemp.CreateCell(3).SetCellValue(list[i].Account);
                rowtemp.CreateCell(4).SetCellValue(list[i].Phone);
                rowtemp.CreateCell(5).SetCellValue(list[i].Address);
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", "学生信息.xls");
        }
    }
}