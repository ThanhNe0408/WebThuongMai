using BanDoOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanDoOnline.Areas.Admin.Controllers
{
    public class DanhMuc1Controller : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        // GET: Admin/DanhMuc1
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult DsDanhMuc()
        {
            try
            {
                var dsDM = (from dm in db.DANHMUCs select new { MaDM = dm.MaDM, TenDM = dm.TenDM }).ToList();
                return Json(new { code = 200, dsDM = dsDM, msg = "Lấy danh sách danh mục thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách danh mục thất bại" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Details(int maDM)
        {
            try
            {
                var dm = (from s in db.DANHMUCs
                          where (s.MaDM == maDM)
                          select new
                          {
                              MaDM = s.MaDM,
                              TenDM = s.TenDM
                          }).SingleOrDefault();

                return Json(new { code = 200, dm = dm, msg = "Lấy thông tin danh mục thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin danh mục thất bại." + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddDanhMuc(string strTenDM)
        {
            try
            {
                var dm = new DANHMUC();
                dm.TenDM = strTenDM;
                db.DANHMUCs.InsertOnSubmit(dm);
                db.SubmitChanges();
                return Json(new { code = 200, msg = "Thêm danh mục thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm danh mục thất bại. Lỗi" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Update(int maDM, string strTenDM)
        {
            try
            {
                var dm = db.DANHMUCs.SingleOrDefault(c => c.MaDM == maDM);
                dm.TenDM = strTenDM;
                db.SubmitChanges();
                return Json(new { code = 200, msg = "Sửa danh mục thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Sửa danh mục thất bại. Lỗi" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Xoa(int maDM)
        {
            try
            {
                var dm = db.DANHMUCs.SingleOrDefault(c => c.MaDM == maDM);
                db.DANHMUCs.DeleteOnSubmit(dm);
                db.SubmitChanges();
                return Json(new { code = 200, msg = "Xóa danh mục thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa danh mục thất bại. Lỗi" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [ChildActionOnly]
        public PartialViewResult _DoanhMucPartial()
        {
            var dsDM = db.DANHMUCs.Select(dm => new { MaDM = dm.MaDM, TenDM = dm.TenDM }).ToList();
            return PartialView("_DoanhMucPartial", dsDM);
        }

    }
}
