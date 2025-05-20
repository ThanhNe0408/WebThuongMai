using BanDoOnline.Models;
using System;
using System.Data.Linq;
using System.Linq;
using System.Web.Mvc;

namespace BanDoOnline.Areas.Admin.Controllers
{
    public class ChiTietTonKhoController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        [HttpGet]
        public ActionResult Index(int maDanhMuc = 0)
        {
            ViewBag.DanhMucList = new SelectList(db.DANHMUCs, "MaDM", "TenDM");

            // Load danh sách sản phẩm theo danh mục được chọn
            var dsSanPham = db.SANPHAMs
                .Where(sp => maDanhMuc == 0 || sp.MaDM == maDanhMuc) // Lọc theo danh mục nếu có
                .OrderBy(sp => sp.TenSP)
                .ToList();

            ViewBag.SanPhamList = new SelectList(dsSanPham, "MaSP", "TenSP");

            return View();
        }

        [HttpGet]
        public JsonResult DsChiTietTonKho(int maDanhMuc = 0, int maSanPham = 0, int page = 1, int pageSize = 10)
        {
            try
            {
                // Assuming that relationships are already defined in your LINQ to SQL model
                DataLoadOptions options = new DataLoadOptions();
                options.LoadWith<CHITIETTONKHO>(ct => ct.SANPHAM);
                options.LoadWith<CHITIETTONKHO>(ct => ct.COLOR);
                options.LoadWith<CHITIETTONKHO>(ct => ct.SIZE);

                db.LoadOptions = options;

                var query = db.CHITIETTONKHOs
                    .OrderBy(ct => ct.MaSP)
                    .Where(ct => (maDanhMuc == 0 || ct.SANPHAM.MaDM == maDanhMuc) && (maSanPham == 0 || ct.MaSP == maSanPham));

                var totalRecords = query.Count();
                var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                var dsChiTiet = query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(ct => new
                    {
                        MaSP = ct.MaSP,
                        MaSize = ct.MaSize,
                        MaColor = ct.MaColor,
                        TenSP = ct.SANPHAM.TenSP,
                        TenColor = ct.COLOR.TenColor,
                        TenSize = ct.SIZE.TenSize,
                        SoLuongTrongKho = ct.SoLuongTrongKho
                    })
                    .ToList();

                return Json(new
                {
                    code = 200,
                    dsChiTiet = dsChiTiet,
                    totalPages = totalPages,
                    currentPage = page,
                    msg = "Lấy danh sách chi tiết tồn kho thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách chi tiết tồn kho thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ChiTiet(int maSP, int maSize, int maColor)
        {
            try
            {
                var chitiet = db.CHITIETTONKHOs
                    .Where(ct => ct.MaSP == maSP && ct.MaSize == maSize && ct.MaColor == maColor)
                    .Select(ct => new
                    {
                        MaSP = ct.MaSP,
                        MaSize = ct.MaSize,
                        MaColor = ct.MaColor,
                        TenSP = ct.SANPHAM.TenSP,
                        TenColor = ct.COLOR.TenColor,
                        TenSize = ct.SIZE.TenSize,
                        SoLuongTrongKho = ct.SoLuongTrongKho
                    })
                    .SingleOrDefault();

                if (chitiet != null)
                {
                    return Json(new { code = 200, chitiet = chitiet, msg = "Lấy thông tin chi tiết tồn kho thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 404, msg = "Không tìm thấy chi tiết tồn kho" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin chi tiết tồn kho thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetSizeAndColorLists()
        {
            try
            {
                var sizes = db.SIZEs.Select(s => new { MaSize = s.MaSize, TenSize = s.TenSize }).ToList();
                var colors = db.COLORs.Select(c => new { MaColor = c.MaColor, TenColor = c.TenColor }).ToList();
                var danhMucList = db.DANHMUCs.Select(dm => new { MaDM = dm.MaDM, TenDM = dm.TenDM }).ToList();

                return Json(new { code = 200, sizes = sizes, colors = colors, danhMucList = danhMucList, msg = "Lấy danh sách size, màu, và danh mục thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách size, màu, và danh mục thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult ThemChiTietTonKho(int maSP, int maSize, int maColor, int soLuong)
        {
            try
            {
                var chitiet = new CHITIETTONKHO
                {
                    MaSP = maSP,
                    MaSize = maSize,
                    MaColor = maColor,
                    SoLuongTrongKho = soLuong
                };

                db.CHITIETTONKHOs.InsertOnSubmit(chitiet);
                db.SubmitChanges();

                return Json(new { code = 200, msg = "Thêm chi tiết tồn kho thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm chi tiết tồn kho thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CapNhatChiTietTonKho(int maSP, int maSize, int maColor, int soLuong)
        {
            try
            {
                var chitiet = db.CHITIETTONKHOs
                    .Where(ct => ct.MaSP == maSP && ct.MaSize == maSize && ct.MaColor == maColor)
                    .SingleOrDefault();

                if (chitiet != null)
                {
                    chitiet.SoLuongTrongKho = soLuong;
                    db.SubmitChanges();

                    return Json(new { code = 200, msg = "Cập nhật chi tiết tồn kho thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 404, msg = "Không tìm thấy chi tiết tồn kho để cập nhật" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Cập nhật chi tiết tồn kho thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult XoaChiTietTonKho(int maSP, int maSize, int maColor)
        {
            try
            {
                var chitiet = db.CHITIETTONKHOs
                    .Where(ct => ct.MaSP == maSP && ct.MaSize == maSize && ct.MaColor == maColor)
                    .SingleOrDefault();

                if (chitiet != null)
                {
                    db.CHITIETTONKHOs.DeleteOnSubmit(chitiet);
                    db.SubmitChanges();

                    return Json(new { code = 200, msg = "Xóa chi tiết tồn kho thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 404, msg = "Không tìm thấy chi tiết tồn kho để xóa" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa chi tiết tồn kho thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetSanPhamByDanhMuc(int maDanhMuc)
        {
            try
            {
                var sanPhamList = db.SANPHAMs
                    .Where(sp => (maDanhMuc == 0 || sp.MaDM == maDanhMuc) && sp.CHITIETTONKHOs.Any(ct => ct.SoLuongTrongKho > 0))
                    .Select(sp => new
                    {
                        MaSP = sp.MaSP,
                        TenSP = sp.TenSP
                    })
                    .ToList();

                return Json(new { code = 200, products = sanPhamList, msg = "Lấy danh sách sản phẩm theo danh mục thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách sản phẩm theo danh mục thất bại: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
