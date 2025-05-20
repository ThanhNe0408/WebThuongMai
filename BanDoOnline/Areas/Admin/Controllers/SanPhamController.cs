using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using System.Globalization;
using BanDoOnline.Models;

namespace BanDoOnline.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        private DataClasses1DataContext db = new DataClasses1DataContext();

        public ActionResult Index(int? page)
        {
            int iPageNumber = (page ?? 1);
            int iPageSize = 7;

            // Sửa truy vấn LINQ để phù hợp với tên cột trong cơ sở dữ liệu
            return View(db.SANPHAMs.ToList().OrderBy(n => n.MaSP).ToPagedList(iPageNumber, iPageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaDM = new SelectList(db.DANHMUCs.ToList().OrderBy(n => n.TenDM), "MaDM", "TenDM");
            
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SANPHAM sanpham, FormCollection f, HttpPostedFileBase fFileUpload)
        {
            ViewBag.MaDM = new SelectList(db.DANHMUCs.ToList().OrderBy(n => n.TenDM), "MaDM", "TenDM");

            if (fFileUpload == null)
            {
                ViewBag.ThongBao = "Hãy chọn ảnh bìa";
                ViewBag.TenSP = f["sTenSP"];
                ViewBag.MoTa = f["sMoTa"];
                ViewBag.SoLuong = int.Parse(f["iSoLuong"]);
                ViewBag.GiaBan = decimal.Parse(f["mGiaBan"]);
                ViewBag.MaDM = new SelectList(db.DANHMUCs.ToList().OrderBy(n => n.TenDM), "MaDM", "TenDM", int.Parse(f["MaDM"]));

                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Theme/img/img/"), sFileName);
                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    sanpham.TenSP = f["sTenSP"];

                    // Loại bỏ thẻ <p></p> từ mô tả
                    sanpham.MoTa = StripHtmlTags(f["sMoTa"]);

                    sanpham.AnhBia = sFileName;
                    string format = "MM/dd/yyyy";
                    DateTime temp;
                    if (DateTime.TryParseExact(f["NgayCapNhat"], format, CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out temp))
                    {
                        sanpham.NgayCapNhat = temp;
                    }
                    else
                    {
                        // Xử lý trường hợp chuỗi ngày tháng không hợp lệ
                        sanpham.NgayCapNhat = DateTime.Now;
                    }
                    sanpham.SoLuongBan = int.Parse(f["iSoLuong"]);
                    sanpham.GiaBan = decimal.Parse(f["mGiaBan"]);
                    sanpham.MaDM = int.Parse(f["MaDM"]);

                    db.SANPHAMs.InsertOnSubmit(sanpham);
                    db.SubmitChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
        }

        // Hàm để loại bỏ các thẻ HTML từ chuỗi
        private string StripHtmlTags(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return html;
            }

            // Sử dụng biểu thức chính quy để loại bỏ thẻ <p></p>
            return System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", string.Empty);
        }

        public ActionResult DeTails(int id)
        {
            var sanpham = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }

  
public ActionResult TimKiem(string searchString, int? page)
    {
        // Lấy tất cả sản phẩm từ cơ sở dữ liệu
        var sanpham = from sp in db.SANPHAMs select sp;

        // Kiểm tra xem chuỗi tìm kiếm có rỗng hay không
        if (!String.IsNullOrEmpty(searchString))
        {
            // Chuyển chuỗi tìm kiếm về dạng chữ thường
            searchString = searchString.ToLower();

            // Lọc các sản phẩm có tên chứa chuỗi tìm kiếm
            sanpham = sanpham.Where(s => s.TenSP.ToLower().Contains(searchString));
        }

        // Số sản phẩm trên mỗi trang
        int pageSize = 4; // Đây là số sản phẩm bạn muốn hiển thị trên mỗi trang

        // Số trang, mặc định là trang 1 nếu không có page được truyền vào
        int pageNumber = (page ?? 1);

        // Áp dụng phân trang vào danh sách sản phẩm
        var pagedSanPham = sanpham.ToPagedList(pageNumber, pageSize);

        // Trả về view với danh sách sản phẩm sau khi tìm kiếm và phân trang
        return View("TimKiem", pagedSanPham);
    }
        public ActionResult Delete(int id)
        {
            var sanpham = db.SANPHAMs.SingleOrDefault(s => s.MaSP == id);
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var sanpham = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = db.CHITIETDATHANGs.Where(ct => ct.MaSP == id);
            if (ctdh.Count() > 0)
            {
                ViewBag.ThongBao = " Sản phẩm này đang có trong bảng chi tiết đặt hàng <br>"
                    + "Nếu muốn xóa thì phải xóa hết mã sản phẩm này trong bảng chi tiết đặt hàng";
                return View(sanpham);
            }
          
            db.SANPHAMs.DeleteOnSubmit(sanpham);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var sanpham = db.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }

            ViewBag.MaDM = new SelectList(db.DANHMUCs.OrderBy(n => n.TenDM), "MaDM", "TenDM", sanpham.MaDM);

            return View(sanpham);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(SANPHAM model, HttpPostedFileBase fFileUpload)
        {
            if (ModelState.IsValid)
            {
                var sanpham = db.SANPHAMs.SingleOrDefault(n => n.MaSP == model.MaSP);

                if (sanpham == null)
                {
                    return HttpNotFound();
                }

                if (fFileUpload != null)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Theme/img/img/"), sFileName);

                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    sanpham.AnhBia = sFileName;
                }

                // Loại bỏ thẻ <p></p> từ mô tả
                sanpham.MoTa = StripfHtmlTags(model.MoTa);

                sanpham.TenSP = model.TenSP;
                sanpham.NgayCapNhat = model.NgayCapNhat;
                sanpham.SoLuongBan = model.SoLuongBan;
                sanpham.GiaBan = model.GiaBan;
                sanpham.MaDM = model.MaDM;

                // Sử dụng SaveChanges thay vì SubmitChanges
                db.SubmitChanges();

                return RedirectToAction("Index");
            }

            ViewBag.MaDM = new SelectList(db.DANHMUCs.OrderBy(n => n.TenDM), "MaDM", "TenDM", model.MaDM);

            return View(model);
        }

        // Hàm để loại bỏ các thẻ HTML từ chuỗi
        private string StripfHtmlTags(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return html;
            }

            // Sử dụng biểu thức chính quy để loại bỏ thẻ <p></p>
            return System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", string.Empty);
        }




    }
}
