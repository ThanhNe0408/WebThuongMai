using BanDoOnline.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace BanDoOnline.Areas.Admin.Controllers
{
    public class DanhMucController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult DsDanhMuc()
        {
            try
            {
                var dsDM = (from dm in db.DANHMUCs
                            select new
                            {
                                MaDM = dm.MaDM,
                                TenDM = dm.TenDM
                            }).ToList();

                return Json(new { code = 200, dsDM = dsDM, msg = "Lấy danh sách danh mục thành công" },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách danh mục thất bại: " + ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Details(int maDM)
        {
            try
            {
                var dm = (from d in db.DANHMUCs
                          where (d.MaDM == maDM)
                          select new
                          {
                              MaDM = d.MaDM,
                              TenDM = d.TenDM
                          }).SingleOrDefault();

                return Json(new { code = 200, dm = dm, msg = "Lấy thông tin danh mục thành công" },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin danh mục thất bại: " + ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddDanhMuc(string strTenDM)
        {
            try
            {
                // Kiểm tra xem tên danh mục đã tồn tại chưa
                if (db.DANHMUCs.Any(d => d.TenDM.Equals(strTenDM, StringComparison.OrdinalIgnoreCase)))
                {
                    return Json(new { code = 400, msg = "Tên danh mục đã tồn tại. Vui lòng chọn tên khác." });
                }

                var dm = new DANHMUC();
                dm.TenDM = strTenDM;
                db.DANHMUCs.InsertOnSubmit(dm);
                db.SubmitChanges();

                // Lấy danh sách danh mục sau khi thêm để cập nhật trên view
                var dsDM = (from d in db.DANHMUCs
                            select new
                            {
                                MaDM = d.MaDM,
                                TenDM = d.TenDM
                            }).ToList();

                return Json(new { code = 200, dsDM = dsDM, msg = "Thêm danh mục thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm danh mục thất bại: " + ex.Message });
            }
        }


        [HttpPost]
        public JsonResult Update(int maDM, string strTenDM)
        {
            try
            {
                var dm = db.DANHMUCs.SingleOrDefault(d => d.MaDM == maDM);
                if (dm != null)
                {
                    dm.TenDM = strTenDM;
                    db.SubmitChanges();

                    return Json(new { code = 200, msg = "Sửa danh mục thành công" },
                        JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 404, msg = "Không tìm thấy danh mục" },
                        JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Sửa danh mục thất bại: " + ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(int maDM)
        {
            try
            {
                var dm = db.DANHMUCs.SingleOrDefault(d => d.MaDM == maDM);
                if (dm != null)
                {
                    db.DANHMUCs.DeleteOnSubmit(dm);
                    db.SubmitChanges();

                    // Lấy danh sách danh mục sau khi xóa để cập nhật trên view
                    var dsDM = (from d in db.DANHMUCs
                                select new
                                {
                                    MaDM = d.MaDM,
                                    TenDM = d.TenDM
                                }).ToList();

                    return Json(new { code = 200, dsDM = dsDM, msg = "Xóa danh mục thành công" },
                        JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 404, msg = "Không tìm thấy danh mục" },
                        JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa danh mục thất bại: " + ex.Message },
                    JsonRequestBehavior.AllowGet);
            }
        }
    }
}
