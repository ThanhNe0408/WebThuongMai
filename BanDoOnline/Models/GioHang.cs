using System;
using System.Collections.Generic;
using System.Linq;

namespace BanDoOnline.Models
{
    public class GioHang
    {
        private readonly DataClasses1DataContext db = new DataClasses1DataContext();

        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string HinhAnh { get; set; }
        public double GiaBan { get; set; }
        public int SoLuong { get; set; }
        public double ThanhTien { get; set; }
        public int MaSizeChon { get; set; }
        public string TenSizeChon { get; set; }
        public int MaColorChon { get; set; }
        public string TenColorChon { get; set; }
        public decimal DonGia
        {
            get { return Convert.ToDecimal(GiaBan * SoLuong); }
        }

        public int MaSize { get; set; }
        public int MaColor { get; set; }
        public IEnumerable<SIZE> Sizes { get; set; }
        public IEnumerable<COLOR> Colors { get; set; }

        public GioHang(int maSanPham, int maSize, int maColor, int quantity = 1)
        {
            MaSanPham = maSanPham;
            MaSize = maSize;
            MaColor = maColor;

            // Lấy thông tin Size và Color từ cơ sở dữ liệu nếu có tồn kho
            var tonKho = db.CHITIETTONKHOs.SingleOrDefault(x => x.MaSP == maSanPham && x.MaSize == maSize && x.MaColor == maColor);
            if (tonKho != null)
            {
                Sizes = db.SIZEs.ToList();
                Colors = db.COLORs.ToList();
            }

            // Lấy thông tin sản phẩm từ cơ sở dữ liệu
            var sanPham = db.SANPHAMs.SingleOrDefault(n => n.MaSP == MaSanPham);

            if (sanPham != null)
            {
                TenSanPham = sanPham.TenSP;
                HinhAnh = sanPham.AnhBia;
                GiaBan = Convert.ToDouble(sanPham.GiaBan);
                SoLuong = quantity;
                ThanhTien = GiaBan * SoLuong;
            }
            else
            {
                throw new InvalidOperationException("Không tìm thấy sản phẩm với mã số cung cấp.");
            }
        }


    }
}
