	CREATE DATABASE BanDoOnline
GO
USE BanDoOnline
GO

CREATE TABLE SANPHAM
(
	MaSP INT ,
	TenSP NVARCHAR(100) NOT NULL,
	MoTa NTEXT,
	AnhBia VARCHAR(50),
	NgayCapNhat SMALLDATETIME,
	SoLuongBan INT CHECK(SoLuongBan>0),
	GiaBan MONEY CHECK (GiaBan>=0),
	MaDM INT,
	CONSTRAINT PK_S PRIMARY KEY(MaSP)
)
GO
CREATE TABLE DANHMUC
(
	MaDM INT ,
	TenDM NVARCHAR(50) NOT NULL,
	CONSTRAINT PK_CD PRIMARY KEY(MaDM)
)
GO
CREATE TABLE KHACHHANG
(
	MaKH INT ,
	HoTen NVARCHAR(50) NOT NULL,
	TaiKhoan VARCHAR(15) UNIQUE,
	MatKhau VARCHAR(15) NOT NULL,
	Email VARCHAR(50) UNIQUE,
	DiaChi NVARCHAR(50),
	DienThoai VARCHAR(10),
	NgaySinh SMALLDATETIME,
	CONSTRAINT PK_Kh PRIMARY KEY(MaKH)
)
-- Tìm và xóa ràng buộc mặc định cho cột FailedLoginAttempts

ALTER TABLE KHACHHANG
ADD LuotDN INT,
    TrangThai BIT;



ALTER TABLE KHACHHANG
ALTER COLUMN MatKhau VARCHAR(MAX);
-- Bước 1: Xóa ràng buộc UNIQUE hiện tại trên cột TaiKhoan
ALTER TABLE KHACHHANG
DROP CONSTRAINT UQ__KHACHHAN__D5B8C7F095E19738;

-- Bước 2: Thay đổi kích thước của cột TaiKhoan thành VARCHAR(MAX)
ALTER TABLE KHACHHANG
ALTER COLUMN TaiKhoan VARCHAR(MAX);

ALTER TABLE KHACHHANG
DROP COLUMN TaiKhoan;

ALTER TABLE KHACHHANG
ADD TaiKhoan VARCHAR(999) UNIQUE;

GO
CREATE TABLE DONDATHANG
(
	MaDonHang INT ,
	DaThanhToan BIT DEFAULT(0),
	TinhTrangGiaoHang INT DEFAULT(1),
	NgayDat SMALLDATETIME,
	NgayGiao SMALLDATETIME,
	MaKH INT,
	CONSTRAINT PK_DDH PRIMARY KEY(MaDonHang)
)
GO
CREATE TABLE CHITIETDATHANG
(
	MaDonHang INT,
	MaSP INT,
	SoLuong INT CHECK(SoLuong>0),
	DonGia DECIMAL(9,2) CHECK(DonGia>=0),
	CONSTRAINT PK_CTDH PRIMARY KEY(MaDonHang,MaSP)
)
GO




ALTER TABLE SANPHAM ADD CONSTRAINT FK_S_CD FOREIGN KEY (MaDM) REFERENCES DANHMUC(MaDM)
ALTER TABLE CHITIETDATHANG ADD CONSTRAINT FK_CTDH_DDH FOREIGN KEY (MaDonHang) REFERENCES DONDATHANG(MaDonHang)
ALTER TABLE CHITIETDATHANG ADD CONSTRAINT FK_CTDH_S FOREIGN KEY (MaSP) REFERENCES SANPHAM(MaSP)


--DANH MỤC
INSERT [dbo].[DANHMUC] ([MaDM], [TenDM]) VALUES (1, N'Giày câu lông')
INSERT [dbo].[DANHMUC] ([MaDM], [TenDM]) VALUES (2, N'Giày bóng đá')
INSERT [dbo].[DANHMUC] ([MaDM], [TenDM]) VALUES (3, N'Giày chạy bộ')
INSERT [dbo].[DANHMUC] ([MaDM], [TenDM]) VALUES (4, N'Quần áo bóng đá')
INSERT [dbo].[DANHMUC] ([MaDM], [TenDM]) VALUES (5, N'Quần áo cầu lông nữ')
INSERT [dbo].[DANHMUC] ([MaDM], [TenDM]) VALUES (6, N'Quần áo cầu lông nam')
INSERT [dbo].[DANHMUC] ([MaDM], [TenDM]) VALUES (7, N'Phụ kiện cầu lông')
INSERT [dbo].[DANHMUC] ([MaDM], [TenDM]) VALUES (8, N'Túi cầu lông')
INSERT [dbo].[DANHMUC] ([MaDM], [TenDM]) VALUES (9, N'Phụ kiện bóng đá')



--SẢN PHẨM
INSERT [dbo].[SANPHAM] ([MaSP], [TenSP], [GiaBan], [Mota], [AnhBia], [MaDM], [Ngaycapnhat], [SoLuongBan])
VALUES (1, N'Áo đá bóng Barcelona sân khách 2023 - 2024', 120000, N'
- Chất liệu: vải thun lạnh 100% polyester, mềm, mịn và siệu nhẹ.
- Cấu trúc vải hai chiều với các đặc tính thấm hút tốt và có giản linh hoạt tạo sự thoải mái thoáng mát cho các cầu thủ khi tập luyện và thi đấu ở cường độ cao.
- Công nghệ in chuyển cao cấp cùng mực in nhập khẩu Châu Âu cho màu áo cực kỳ sắc nét, thẩm mỹ, áo luôn tươi mới, bền đẹp và không gây hại da.
- Logo câu lạc bộ được dệt bằng máy dệt cao cấp, sắc nét và không bị sổ chỉ.
- Áo đá bóng tay dài giúp bạn bảo vệ da khi mặc đá bóng ngoài trời nắng và giữ ấm khi có khí trời lạnh.
- Form áo chuẩn thể thao phù hợp với nhiều người, dễ mặc chung với nhiều loại đồ.
- Size áo: S, M, L, XL, XXL', N'aobd1.jpg', 4, '04/11/2023', 120)

INSERT [dbo].[SANPHAM] ([MaSP], [TenSP], [GiaBan], [Mota], [AnhBia], [MaDM], [Ngaycapnhat], [SoLuongBan])
VALUES (2, N'ÁO ĐÁ BÓNG KEEP FLY FLOW MÀU CAM', 130000, N'
Áo đá bóng Barcelona sân khách mùa giải 2023 - 2024
Bộ quần áo đá bóng không logo Keep Fly Flow chính hãng
- Chất liệu vải: Vải thun lạnh Polyester cao cấp, mềm mỏng, nhẹ, độ co giãn tốt, thấm hút mồ hôi và khô nhanh, giúp cơ thể luôn thoải mái kể cả khi chơi thể thao ở cường độ cao.
- Công nghệ chống bết da cơ thể không khó chịu khi đổ mồ hôi.
- Logo thêu sắc nét, các họa tiết trên áo đều in chuyển tinh xảo, không bị bong tróc bay màu và không gây hại da
- Sản xuất trên dây chuyền công nghệ tiên tiến, các đường kim chỉn chu, sắc nét, cho sản phẩm luôn ỗn định, độ bền cao.
- Thiết kế Form suông dễ mặc.
- Size S - M - L - XL - XXL
- Sản xuất tại Việt Nam.', N'aobd2.jpg', 4, '11/11/2023', 90)

INSERT [dbo].[SANPHAM] ([MaSP], [TenSP], [GiaBan], [Mota], [AnhBia], [MaDM], [Ngaycapnhat], [SoLuongBan])
VALUES (3, N'ÁO ĐÁ BÓNG KEEP FLY FLOW MÀU TÍM TRẮNG', 130000, N'
Bộ quần áo đá bóng không logo Keep Fly Flow chính hãng
- Chất liệu vải: Vải thun lạnh Polyester cao cấp, mềm mỏng, nhẹ, độ co giãn tốt, thấm hút mồ hôi và khô nhanh, giúp cơ thể luôn thoải mái kể cả khi chơi thể thao ở cường độ cao.
- Công nghệ chống bết da cơ thể không khó chịu khi đổ mồ hôi.
- Logo thêu sắc nét, các họa tiết trên áo đều in chuyển tinh xảo, không bị bong tróc bay màu và không gây hại da
- Sản xuất trên dây chuyền công nghệ tiên tiến, các đường kim chỉn chu, sắc nét, cho sản phẩm luôn ỗn định, độ bền cao.
- Thiết kế Form suông dễ mặc.
- Size S - M - L - XL - XXL
- Sản xuất tại Việt Nam.', N'aobd3.jpg', 4, '05/11/2023', 110)

INSERT [dbo].[SANPHAM] ([MaSP], [TenSP], [GiaBan], [Mota], [AnhBia], [MaDM], [Ngaycapnhat], [SoLuongBan])
VALUES (4, N'ÁO CẦU LÔNG NỮ KAMITO SKY MÀU TRẮNG', 130000, N'
Bộ quần áo đá bóng không logo Keep Fly Flow chính hãng
- Chất liệu vải: Vải thun lạnh Polyester cao cấp, mềm mỏng, nhẹ, độ co giãn tốt, thấm hút mồ hôi và khô nhanh, giúp cơ thể luôn thoải mái kể cả khi chơi thể thao ở cường độ cao.
- Công nghệ chống bết da cơ thể không khó chịu khi đổ mồ hôi.
- Logo thêu sắc nét, các họa tiết trên áo đều in chuyển tinh xảo, không bị bong tróc bay màu và không gây hại da
- Sản xuất trên dây chuyền công nghệ tiên tiến, các đường kim chỉn chu, sắc nét, cho sản phẩm luôn ỗn định, độ bền cao.
- Thiết kế Form suông dễ mặc.
- Size S - M - L - XL - XXL
- Sản xuất tại Việt Nam.', N'aobd4.jpg', 4, '07/11/2023', 100)

INSERT [dbo].[SANPHAM] ([MaSP], [TenSP], [GiaBan], [Mota], [AnhBia], [MaDM], [Ngaycapnhat], [SoLuongBan])
VALUES (5, N'ÁO CẦU LÔNG NỮ PRONING 3697 MÀU ĐỎ', 100000, N'
Giới thiệu Áo cầu lông nữ Kamito Sky chính hãng
- Chất liệu vải: 95% Polyester và 5% Spandex 
- Đặc tính vải: Mềm mịn, nhẹ, thoáng, thoát hơi nhanh, đàn hồi tốt.
- Công nghệ Moisture-Wicking sợi vải thấm hút mồ hôi tốt, khô nhanh cùng với khả năng chống bám bụi tốt.
- Form áo regular fit vừa vặn với mọi cơ thể, thoải mái trong từng cử động.
- Công nghệ chống tia cực tím cho da hạn chế bị kích ứng dưới anh nắng mặt trời.
- Công nghệ In chuyển nhiệt hiện đại cho họa tiết áo sắc nét, bên màu, không hề gây kích ứng cho da.
- Áo cầu lông nữ Kamito Sky mang đến cho bạn phong cách nhẹ nhàng nhưng hiện đại và luôn nổi bật.
- Được Viện Dệt may Việt Nam chứng nhận không chứa các chất gây hại cho sức khỏe khi vượt qua các thử nghiệm trong lồng giặt công suất cao, đảm bảo sự bền bỉ của từng sợi vải.', N'aocaulongnu1.jpg', 5, '07/11/2023', 100)

INSERT [dbo].[SANPHAM] ([MaSP], [TenSP], [GiaBan], [Mota], [AnhBia], [MaDM], [Ngaycapnhat], [SoLuongBan])
VALUES (6, N'ÁO ĐÁ BÓNG KEEP FLY FLOW MÀU TRẮNG XANH NGỌC', 200000, N'
Áo cầu lông nữ Proning 3697 chính hãng
- Chất liệu: Vải thun Polyester mè cao cấp.
- Đặc tính vải: Mềm mịn, nhẹ, thoáng, thoát hơi nhanh, đàn hồi tốt.
- Sợi vải thấm hút mồ hôi tốt, khô nhanh cùng với khả năng chống bám bụi tốt.
- Độ đàn hồi cao, giữ form, không bị giản xệ theo thời gian.
- Công nghệ chống tia cực tím cho da hạn chế bị kích ứng dưới anh nắng mặt trời.
- Công nghệ In chuyển nhiệt hiện đại cho họa tiết áo sắc nét, bên màu, không hề gây kích ứng cho da.
- Áo thể thao Proning 3697 mang đến cho bạn phong cách trẻ trung hiện đại, năng lượng dồi dào và luôn nổi bật với đám đông.', N'aocaulongnu2.jpg', 5, '09/11/2023', 120)

INSERT [dbo].[SANPHAM] ([MaSP], [TenSP], [GiaBan], [Mota], [AnhBia], [MaDM], [Ngaycapnhat], [SoLuongBan])
VALUES (7, N'ÁO CẦU LÔNG NỮ PRONING 3697 MÀU XANH BÍCH', 180000, N'
Áo cầu lông nữ Proning 3697 chính hãng
- Chất liệu: Vải thun Polyester mè cao cấp.
- Đặc tính vải: Mềm mịn, nhẹ, thoáng, thoát hơi nhanh, đàn hồi tốt.
- Sợi vải thấm hút mồ hôi tốt, khô nhanh cùng với khả năng chống bám bụi tốt.
- Độ đàn hồi cao, giữ form, không bị giản xệ theo thời gian.
- Công nghệ chống tia cực tím cho da hạn chế bị kích ứng dưới anh nắng mặt trời.
- Công nghệ In chuyển nhiệt hiện đại cho họa tiết áo sắc nét, bên màu, không hề gây kích ứng cho da.
- Áo thể thao Proning 3697 mang đến cho bạn phong cách trẻ trung hiện đại, năng lượng dồi dào và luôn nổi bật với đám đông.', N'aocaulongnu3.jpg', 5, '08/11/2023', 120)








--KHÁCH HÀNG
INSERT [dbo].[KHACHHANG] ([MaKH], [Hoten], [Diachi], [Dienthoai], [TaiKhoan], [Matkhau], [Ngaysinh], [Email]) VALUES (1, N'Ngô Minh Anh', N'3 Bùi Hữu Nghĩa', N'0389895755', N'anhlo', N'anh', '11/24/2003', N'anh@gmail.com')
INSERT [dbo].[KHACHHANG] ([MaKH], [Hoten], [Diachi], [Dienthoai], [TaiKhoan], [Matkhau], [Ngaysinh], [Email]) VALUES (2, N'Nguyễn Tiến Luân', N'Quận 6', N'Chua có', N'thang', N'123456', '03/10/1981', N'ntluan@hcmuns.edu.vn')
INSERT [dbo].[KHACHHANG] ([MaKH], [Hoten], [Diachi], [Dienthoai], [TaiKhoan], [Matkhau], [Ngaysinh], [Email]) VALUES (3, N'Đặng Quốc Hòa', N'Sư Vạn Hạnh', N'Chua có', N'dqhoa', N'hoa', '04/10/1980', N'dqhoa@hcmuns.edu.vn')
INSERT [dbo].[KHACHHANG] ([MaKH], [Hoten], [Diachi], [Dienthoai], [TaiKhoan], [Matkhau], [Ngaysinh], [Email]) VALUES (4, N'Ngô Ngọc Ngân', N'Khu chung cư', N'0918544699', N'nnngan', N'ngan', '02/19/1982', N'nnngan@hcmuns.edu.vn')
INSERT [dbo].[KHACHHANG] ([MaKH], [Hoten], [Diachi], [Dienthoai], [TaiKhoan], [Matkhau], [Ngaysinh], [Email]) VALUES (5, N'Đỗ Quỳnh Hương', N'Cống Quỳnh', N'0908123456', N'dqhuong', N'huong', '06/13/1985', N'dqhuong@hcmuns.edu.vn')
INSERT [dbo].[KHACHHANG] ([MaKH], [Hoten], [Diachi], [Dienthoai], [TaiKhoan], [Matkhau], [Ngaysinh], [Email]) VALUES (6, N'Trần Thị Thu Trang', N'Nơ Trang Long', N'Chua có', N'ttttrang', N'trang', '12/10/1990', N'ttrang@yahoo.com')
INSERT [dbo].[KHACHHANG] ([MaKH], [Hoten], [Diachi], [Dienthoai], [TaiKhoan], [Matkhau], [Ngaysinh], [Email]) VALUES (7, N'Nguyễn Thiên Thanh', N'Hai Bà Trưng', N'0908320111', N'ntthanh', N'thanh', '12/12/1986', N'ntthanh@t3h.hcmuns.edu.vn')
INSERT [dbo].[KHACHHANG] ([MaKH], [Hoten], [Diachi], [Dienthoai], [TaiKhoan], [Matkhau], [Ngaysinh], [Email]) VALUES (8, N'Trần Thị Hải Yến', N'Trần Hưng Đạo', N'8111111   ', N'tthyen', N'yen', '02/07/1988', N'tthyan@vol.vnn.vn')
INSERT [dbo].[KHACHHANG] ([MaKH], [Hoten], [Diachi], [Dienthoai], [TaiKhoan], [Matkhau], [Ngaysinh], [Email]) VALUES (9, N'Nguyễn Thị Thanh Mai', N'Trần Quang Diệu', N'81111222', N'nttmai', N'mai', '06/10/1992', NULL)
INSERT [dbo].[KHACHHANG] ([MaKH], [Hoten], [Diachi], [Dienthoai], [TaiKhoan], [Matkhau], [Ngaysinh], [Email]) VALUES (10, N'Nguyễn Thành Danh', N'Cộng Hòa', N'8103751', N'ntdanh', N'danh', '07/10/1978', N'ntdanh@yahoo.com')
INSERT [dbo].[KHACHHANG] ([MaKH], [Hoten], [Diachi], [Dienthoai], [TaiKhoan], [Matkhau], [Ngaysinh], [Email]) VALUES (11, N'Phạm Thị Nga', N'Q6 - Tp.HCM', N'0831564512', N'ptnga', N'nga', '03/20/1986', N'ptnhung@hcmuns.edu.vn')
INSERT [dbo].[KHACHHANG] ([MaKH], [Hoten], [Diachi], [Dienthoai], [TaiKhoan], [Matkhau], [Ngaysinh], [Email]) VALUES (12, N'Lê Doanh Doanh', N'2Bis Hùng Vương', N'07077865', N'lddoanh', N'doanh', '02/11/1982', N'lddoanh@yahoo.com')
INSERT [dbo].[KHACHHANG] ([MaKH], [Hoten], [Diachi], [Dienthoai], [TaiKhoan], [Matkhau], [Ngaysinh], [Email]) VALUES (13, N'Đòan Ngọc Minh Tâm', N'2 Đinh Tiên Hòang', N'0909092222', N'dnmtam', N'tam', '10/21/1982', N'ndmtam@yahoo.com')
INSERT [dbo].[KHACHHANG] ([MaKH], [Hoten], [Diachi], [Dienthoai], [TaiKhoan], [Matkhau], [Ngaysinh], [Email]) VALUES (14, N'Trần Khải Nhi', N'3 Bùi Hữu Nghĩa', N'0909095555', N'tknhi', N'nhi', '11/24/1982', N'tknhi@yahoo.com')
INSERT [dbo].[KHACHHANG] ([MaKH], [Hoten], [Diachi], [Dienthoai], [TaiKhoan], [Matkhau], [Ngaysinh], [Email]) VALUES (15, N'Nguyễn Hữu Thành', N'3 Huỳnh Văn Lũy', N'0387589597', N'chothanh', N'thanh', '08/24/2003', N'thanh@gmail.com')
INSERT [dbo].[KHACHHANG] ([MaKH], [Hoten], [Diachi], [Dienthoai], [TaiKhoan], [Matkhau], [Ngaysinh], [Email]) VALUES (16, N'Huỳnh Hữu Hoanh', N'3 Lê Hồng Phong', N'0373502953', N'hoanhdz', N'12345', '09/21/2003', N'huynhhuuhoanh21092003@gmail.com')


--ĐƠN ĐẶT HÀNG
INSERT [dbo].[DONDATHANG] ([MaDonHang], [MaKH], [TinhTrangGiaoHang], [Ngaydat], [Ngaygiao]) VALUES (1, 1, 4, '05/10/2021','05/12/2021')
INSERT [dbo].[DONDATHANG] ([MaDonHang], [MaKH], [TinhTrangGiaoHang], [Ngaydat], [Ngaygiao]) VALUES (2, 1, 2, '05/07/2021','05/11/2021')


--CHI TIẾT ĐẶT HÀNG
INSERT [dbo].[CHITIETDATHANG] ([MaDonHang], MaSP, [SoLuong], [DonGia]) VALUES (1, 1, 1, 25000)
INSERT [dbo].[CHITIETDATHANG] ([MaDonHang], MaSP, [SoLuong], [DonGia]) VALUES (1, 2, 2, 25000)
INSERT [dbo].[CHITIETDATHANG] ([MaDonHang], MaSP, [SoLuong], [DonGia]) VALUES (2, 3, 1, 26000)
INSERT [dbo].[CHITIETDATHANG] ([MaDonHang], MaSP, [SoLuong], [DonGia]) VALUES (2, 4, 3, 18000)



SELECT * FROM SANPHAM
	SELECT * FROM CHITIETDATHANG
	SELECT * FROM DANHMUC
	SELECT * FROM KHACHHANG
	SELECT * FROM DONDATHANG
	SELECT * FROM DANHGIA
	SELECT * FROM CHITIETTONKHO
	SELECT * FROM SIZE
	SELECT * FROM COLORS
	SELECT * FROM TKADMIN