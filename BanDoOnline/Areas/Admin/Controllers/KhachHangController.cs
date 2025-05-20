using BanDoOnline.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanDoOnline.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        // GET: Admin/KhachHang
        public ActionResult Index(int? page)
        {
            int iPageNum = (page ?? 1);
            int iPageSize = 5;
            return View(db.KHACHHANGs.ToList().OrderBy(n => n.MaKH).ToPagedList(iPageNum, iPageSize));
        }

        [HttpGet]
        public JsonResult GetKhachHangs()
        {
            try
            {
                var khachHangs = (from kh in db.KHACHHANGs
                                  select new
                                  {
                                      MaKH = kh.MaKH,
                                      HoTen = kh.HoTen,
                                      TaiKhoan = kh.TaiKhoan,
                                      MatKhau = kh.MatKhau,
                                      Email = kh.Email,
                                      NgaySinh = kh.NgaySinh,
                                      DienThoai = kh.DienThoai,
                                      DiaChi = kh.DiaChi
                                  }).ToList();

                return Json(new { code = 200, khachHangs = khachHangs, msg = "Lấy danh sách khách hàng thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách khách hàng thất bại" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetKhachHangDetails(int maKH)
        {
            try
            {
                var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == maKH);

                if (kh != null)
                {
                    var khachHangDetails = new
                    {
                        MaKH = kh.MaKH,
                        HoTen = kh.HoTen,
                        TaiKhoan = kh.TaiKhoan,
                        MatKhau = kh.MatKhau,
                        Email = kh.Email,
                        NgaySinh = kh.NgaySinh,
                        DienThoai = kh.DienThoai,
                        DiaChi = kh.DiaChi
                    };

                    return Json(new { code = 200, khachHangDetails = khachHangDetails, msg = "Lấy thông tin khách hàng thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 404, msg = "Không tìm thấy khách hàng" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin khách hàng thất bại" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddKhachHang(KHACHHANG kh)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.KHACHHANGs.InsertOnSubmit(kh);
                    db.SubmitChanges();
                    return Json(new { code = 200, msg = "Thêm khách hàng thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 400, msg = "Dữ liệu không hợp lệ" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm khách hàng thất bại. Lỗi" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateKhachHang(KHACHHANG kh)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingKh = db.KHACHHANGs.FirstOrDefault(n => n.MaKH == kh.MaKH);

                    if (existingKh != null)
                    {
                        existingKh.HoTen = kh.HoTen;
                        existingKh.TaiKhoan = kh.TaiKhoan;
                        existingKh.MatKhau = kh.MatKhau;
                        existingKh.Email = kh.Email;
                        existingKh.NgaySinh = kh.NgaySinh;
                        existingKh.DienThoai = kh.DienThoai;
                        existingKh.DiaChi = kh.DiaChi;

                        db.SubmitChanges();

                        return Json(new { code = 200, msg = "Cập nhật thông tin khách hàng thành công" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { code = 404, msg = "Không tìm thấy khách hàng" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { code = 400, msg = "Dữ liệu không hợp lệ" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Cập nhật thông tin khách hàng thất bại. Lỗi" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteKhachHang(int maKH)
        {
            try
            {
                var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == maKH);

                if (kh != null)
                {
                    db.KHACHHANGs.DeleteOnSubmit(kh);
                    db.SubmitChanges();

                    return Json(new { code = 200, msg = "Xóa khách hàng thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 404, msg = "Không tìm thấy khách hàng" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa khách hàng thất bại. Lỗi" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}