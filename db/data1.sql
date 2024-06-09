set dateformat dmy

INSERT INTO Roles VALUES ('Admin', N'Full chức năng')
INSERT INTO Roles VALUES ('Customer', N'Xem, mua hàng, theo dõi đơn hàng')

INSERT INTO Accounts VALUES ('quocvinh9502', '123', 1, 1, NULL, NULL)
INSERT INTO Accounts VALUES ('vannhan1604', '123', 1, 1, NULL, NULL)
INSERT INTO Accounts VALUES('thanhnhan2308', '456', 1, 1, NULL, NULL)
INSERT INTO Accounts VALUES('manhtien2901', '000', 1, 2, NULL, NULL)
INSERT INTO Accounts VALUES('hatien', '111', 1, 2, NULL, NULL)

insert into brand values('Apple', NULL)
insert into brand values('Razor', NULL)
insert into brand values('Lenovo', NULL)
insert into brand values('Dell', NULL)
insert into brand values('LG', NULL)
insert into brand values('HP', NULL)
insert into brand values('Asus', NULL)
insert into brand values('MSI', NULL)
insert into brand values('GIGABYTE', NULL)

INSERT INTO Categories VALUES (N'Laptop', NULL, NULL, 1, 1, 1)
INSERT INTO Categories VALUES (N'Màn hình', NULL, NULL, 1, 2, 1)
INSERT INTO Categories VALUES (N'Arm màn hình', NULL, NULL, 1, 3, 1)
INSERT INTO Categories VALUES (N'Âm thanh', NULL, NULL, 1, 4, 1)
INSERT INTO Categories VALUES (N'Bàn phím', NULL, NULL, 1, 5, 1)
INSERT INTO Categories VALUES (N'Ghế công thái học', NULL, NULL, 1, 6, 1)
INSERT INTO Categories VALUES (N'Máy chơi game', NULL, NULL, 1, 7, 1)
INSERT INTO Categories VALUES (N'Phụ kiện Apple', NULL, NULL, 1, 8, 1)
INSERT INTO Categories VALUES (N'Chuột', NULL, NULL, 1, 9, 1)
INSERT INTO Categories VALUES (N'Bàn nâng hạ', NULL, NULL, 1, 10, 1)
INSERT INTO Categories VALUES (N'Balo, Túi', NULL, NULL, 1, 11, 1)
INSERT INTO Categories VALUES (N'Cổng chuyển', NULL, NULL, 1, 12, 1)
INSERT INTO Categories VALUES (N'Đế tản nhiệt', NULL, NULL, 1, 13, 1)
INSERT INTO Categories VALUES (N'Ổ cứng', NULL, NULL, 1, 14, 1)
INSERT INTO Categories VALUES (N'RAM', NULL, NULL, 1, 15, 1)
INSERT INTO Categories VALUES (N'Phần mềm', NULL, NULL, 1, 16, 1)
INSERT INTO Categories VALUES (N'Case máy tính', NULL, NULL, 1, 17, 1)
INSERT INTO Categories VALUES (N'PC', NULL, NULL, 1, 18, 1)
INSERT INTO Categories VALUES (N'CPU', NULL, NULL, 1, 19, 1)

INSERT INTO PRODUCTS VALUES(1, 3, N'Lenovo Ideapad 5 Pro 16 AMD (82L50095VN)', 'Ryzen 5', N'AMD Ryzen 5 5600H, 6 nhân, 12 luồng', '8GB', 'SSD', '512GB', N'Card rời', 'Nvidia Geforce', 'AMD Radeon Graphics, GeForce® GTX 1650', 'Window', '16.1", 2560 x 1600 px, IPS, 100%% sRGB, 120 Hz', 16.1, '1.9 kg', 24990000, 'lenovo-ideapad-5-pro-16.png', NULL, NULL, 1, 1, 1, 'IdeaPad5APro1604CF', 26) 

INSERT INTO Customers VALUES (N'Phước Phạm', '22/04/2002', NULL, N'Bến Tre', 'ngiap@gmail.com', '0623360961', NULL, 1, 1)
INSERT INTO Customers VALUES (N'Phụng Ngọc', '23/08/2000', NULL, N'Đà Nẵng', 'doanchi123@gmail.com', '0926285936', NULL, 1, 2)
INSERT INTO Customers VALUES (N'Đăng Hữu', '12/01/2005', NULL, N'Hồ Chí Minh', 'vinh.lieup@gmail.com', '0384409926', NULL, 1, 3)


INSERT INTO Staffs VALUES (N'Bích Phương', '19/02/1999', NULL, N'Cần Thơ', 'tquach@gmail.com', '0840895499', 4, NULL, 1)
INSERT INTO Staffs VALUES (N'Lâm Quang', '20/06/2003', NULL, N'Vũng Tàu', 'vong.anh@gmail.com', '0902410532', 5, NULL, 1)

INSERT INTO TransactStatus VALUES (N'Chờ xác nhận', NULL)
INSERT INTO TransactStatus VALUES (N'Chờ lấy hàng', NULL)
INSERT INTO TransactStatus VALUES (N'Đang giao', NULL)
INSERT INTO TransactStatus VALUES (N'Giao thành công', NULL)
INSERT INTO TransactStatus VALUES (N'Hoàn tiền', NULL)

INSERT INTO Payments VALUES (N'Thanh toán đơn hàng 1', NULL)

INSERT INTO Orders VALUES (1, '6/11/2022', 1, 0, 0, NULL, NULL, NULL)
INSERT INTO Orders VALUES (2, '5/11/2022', 2, 0, 0, NULL, NULL, NULL)
INSERT INTO Orders VALUES (3, '3/11/2022', 4, 0, 1, '3/11/2022', 1, NULL)

INSERT INTO OrderDetails VALUES (1, 1, 1, 1, 0, 66990000, NULL)
--INSERT INTO OrderDetails VALUES (2, 2, 2, 1, 0, 4990000, NULL)
--INSERT INTO OrderDetails VALUES (3, 3, 3, 2, 600000, 2980000, NULL)	

INSERT INTO Posts VALUES (N'Cách lấy lại mật khẩu', NULL, NULL, 4, NULL, N'Bích Phương', '#matkhau', 1, 0, 1000000)
INSERT INTO Posts VALUES (N'Cách áp dụng mã khuyến mãi', NULL, NULL, 5, NULL, N'Lâm Quang', '#makhuyenmai', 1, 1, 5000000)


