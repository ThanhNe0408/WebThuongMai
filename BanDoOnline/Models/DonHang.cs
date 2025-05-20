using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BanDoOnline.Models
{
    public class DonHang
    {
        public int MaDonHang { get; set; }
        public bool DaThanhToan { get; set; }
        public int TinhTrangGiaoHang { get; set; }
        public DateTime? NgayDat { get; set; }
        public DateTime? NgayGiao { get; set; }
        public int MaKH { get; set; }
        // Thêm các trường thông tin khách hàng nếu cầnS
        // Thêm các trường thông tin khách hàng nếu cầnS
    }
}