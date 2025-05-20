using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanDoOnline.Models
{
    public class DanhGia
    {
        public int MaDanhGia { get; set; }
        public int? MaKH { get; set; }
        public int? MaSP { get; set; }
        public int DiemDanhGia { get; set; }
        public string NhanXet { get; set; }
        public DateTime NgayDanhGia { get; set; }
        public string HoTenNguoiDanhGia { get; set; }

        public DanhGia()
        {
            // Default constructor
        }

        public DanhGia(int? maKH, int maSP, int diemDanhGia, string nhanXet, DateTime ngayDanhGia, string hoTenNguoiDanhGia)
        {
            MaKH = maKH;
            MaSP = maSP;
            DiemDanhGia = diemDanhGia;
            NhanXet = nhanXet;
            NgayDanhGia = ngayDanhGia;
            HoTenNguoiDanhGia = hoTenNguoiDanhGia;
        }
    }
}