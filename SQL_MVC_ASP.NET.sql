Create DATABASE ShopQuanAo
USE ShopQuanAo

GO


CREATE TABLE ACOUNT 
(
	ID INT NOT NULL PRIMARY KEY Identity(1,1),
	EMAIL VARCHAR(250) NOT NULL,
	MAT_KHAU VARCHAR(250),
	LINK_ANH CHAR(250),
	HO_TEN NVARCHAR(50) NOT NULL,
	PHONE CHAR(12) NOT NULL,
	NGAY_DANG_KY DATETIME  DEFAULT GETDATE(),
	DIA_CHI NVARCHAR (250) NOT NULL,
	TRANG_THAI BIT DEFAULT 0,
	IS_REMOVE bit DEFAULT 0
)


GO

CREATE TABLE LOAI_SAN_PHAM
 (
	ID_LOAI_SP INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	TEN_LOAI_SP NVARCHAR(250) NOT NULL,
	SLUG VARCHAR(250) NOT NULL,
	TRANG_THAI BIT DEFAULT 1,
	ID_CHA INT DEFAULT 0,
	NGAY_TAO DATETIME DEFAULT GETDATE(),
	IS_REMOVE bit DEFAULT 0
 )

 GO
 
 CREATE TABLE SAN_PHAM(
	MA_SP CHAR(20) NOT NULL PRIMARY KEY,
	ID_LSP INT NOT NULL,
	TEN_SP NVARCHAR(250) NOT NULL,
	SLUG NVARCHAR(250) NOT NULL,
	MO_TA NVARCHAR(255) ,
	MO_TA_CHI_TIET NTEXT,
	LINK_ANH_CHINH VARCHAR(250),
	LIST_ANH_KEM TEXT,
	SO_LUONG_TONG INT DEFAULT 0,
	GIA_NHAP DECIMAL(15,4) NOT NULL ,
	GIA_BAN DECIMAL(15,4) NOT NULL ,
	GIAM_GIA INT DEFAULT 0,
	DON_VI_TINH NVARCHAR(30),
	LUOT_XEM INT DEFAULT 0,
	TRANG_THAI BIT DEFAULT 0,
	NOI_BAT BIT DEFAULT 0,
	NGAY_TAO DATETIME DEFAULT GETDATE(),
	IS_REMOVE bit DEFAULT 0
)
GO
 
 CREATE TABLE COLOR
 (
	ID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	TEN_MAU NVARCHAR(250) NOT NULL,
	SLUG VARCHAR(250),
	MA_MAU VARCHAR(250) ,
	MA_SP CHAR(20),
	IMAGES VARCHAR(100), 
	TRANG_THAI  BIT DEFAULT 1, 
 )

 GO 
 CREATE TABLE SIZE(
	ID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	TEN_SIZE VARCHAR(50) ,
	SLUG VARCHAR (50),
	MO_TA NVARCHAR(250),
	TRANG_THAI BIT DEFAULT 1,
	IS_REMOVE bit DEFAULT 0
 )
 GO

 CREATE TABLE SAN_PHAM_CHI_TIET(
	ID BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
	ID_SIZE INT NOT NULL,
	ID_COLOR INT NOT NULL ,
	MA_SP CHAR(20) NOT NULL ,
	SLUG NVARCHAR(80) NOT NULL,
	SO_LUONG INT ,
	TRANG_THAI BIT DEFAULT 0, 
	NGAY_TAO DATETIME DEFAULT GETDATE(),
	IS_REMOVE bit DEFAULT 0
 ) 

GO

 CREATE TABLE BAI_VIET
 (
	MA_BV INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	TIEU_DE NVARCHAR(250) NOT NULL,
	MO_TA NVARCHAR(255) ,
	SLUG VARCHAR (250) NOT NULL UNIQUE,
	IMAGES VARCHAR(250),
	NOI_DUNG NTEXT,
	NOI_BAT BIT DEFAULT 0,
	TRANG_THAI BIT DEFAULT 0,
	NGAY_DANG DATETIME DEFAULT GETDATE(),
	IS_REMOVE bit DEFAULT 0
 )

 GO
  CREATE TABLE SLIDE
 (
	ID INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	TIEU_DE NVARCHAR(250),
	LINK VARCHAR(250) NOT NULL,
	IMAGES VARCHAR(250),
	STT TINYINT DEFAULT 0,
	TRANG_THAI BIT DEFAULT 0,
	NOI_BAT BIT DEFAULT 0,
	NGAY_DANG DATETIME DEFAULT GETDATE(),
	IS_REMOVE bit DEFAULT 0
 )
 GO


 CREATE TABLE STATUS_HOA_DON
 (
	ID INT PRIMARY KEY IDENTITY(1,1),
	STATUS_ORDER NVARCHAR(20) NOT NULL
 )
 GO
 INSERT INTO STATUS_HOA_DON(STATUS_ORDER) VALUES
 (N'Đặt Hàng'),
 (N'Giao Hàng'),
 (N'Hoàn Thành'),
 (N'Hủy Đơn Hàng');
 GO

 CREATE TABLE HOA_DON 
 (
	MA_HD INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	TEN_NHAN_HANG NVARCHAR(50) ,
	NGAY_MUA DATETIME DEFAULT GETDATE(),
	DIA_CHI_NHAN NVARCHAR(250) ,
	SDT_NHAN CHAR(15) ,
	TONG_TIEN DECIMAL(15,4)DEFAULT 0 ,
	TRANG_THAI INT DEFAULT 1,
	GHI_CHU NVARCHAR(250) ,
	IS_REMOVE bit DEFAULT 0
)
 go




 CREATE TABLE CHI_TIET_HOA_DON 
 (
	ID INT NOT NULL PRIMARY KEY IDENTITY(1,1) ,
	ID_HD INT not null,
	ID_SP_CT INT not null ,
	SP_SIZE_MAU NVARCHAR(80) ,
	DON_VI_TINH NVARCHAR(30) ,
	SL_MUA INT default 0,
	GIA_BAN DECIMAL(15,4) default 0 ,
	IS_REMOVE bit DEFAULT 0
 )
GO


--- TRIGGER tính tổng số lượng sản phẩm theo tổng số lượng san-pham-chi-tiet hiện có 
	/* cập nhật số lượng hàng trong kho sau khi thêm sô lượng vào bảng chi tiết*/
	CREATE TRIGGER TRG_INSERT_SO_LUONG_CHI_TIET_SP ON SAN_PHAM_CHI_TIET AFTER INSERT AS 
	BEGIN
		UPDATE SAN_PHAM SET SO_LUONG_TONG = SO_LUONG_TONG + (SELECT SO_LUONG FROM inserted WHERE inserted.MA_SP = SAN_PHAM.MA_SP) 
		FROM SAN_PHAM  JOIN inserted  ON SAN_PHAM.MA_SP = inserted.MA_SP
	
	END
	GO
	/* cập nhật hàng trong kho sau khi cập nhật đặt hàng */
	CREATE TRIGGER TRG_UPDATE_SO_LUONG_CHI_TIET_SP on SAN_PHAM_CHI_TIET after update AS
	BEGIN
		UPDATE SAN_PHAM SET SO_LUONG_TONG = SO_LUONG_TONG + (SELECT SO_LUONG FROM inserted WHERE inserted.MA_SP = SAN_PHAM.MA_SP) -
		(SELECT SO_LUONG FROM deleted WHERE deleted.MA_SP = SAN_PHAM.MA_SP)
		FROM SAN_PHAM  JOIN deleted  ON SAN_PHAM.MA_SP = deleted.MA_SP
	end
	GO
	/* cập nhật hàng trong kho sau khi hủy đặt hàng */
	CREATE TRIGGER TRG_DELETE_SO_LUONG_CHI_TIET_SP ON SAN_PHAM_CHI_TIET FOR DELETE AS 
	BEGIN
		UPDATE SAN_PHAM SET SO_LUONG_TONG = SO_LUONG_TONG - (SELECT SO_LUONG FROM deleted WHERE deleted.MA_SP = SAN_PHAM.MA_SP) 
		FROM SAN_PHAM  JOIN deleted  ON SAN_PHAM.MA_SP = deleted.MA_SP
	END
go
--- end

--- Tạo Proc xóa Sản Phẩm
