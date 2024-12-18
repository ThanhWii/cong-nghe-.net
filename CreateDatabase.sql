USE [master]
GO
/****** Object:  Database [QLPhongTro]    Script Date: 12/7/2024 05:59:23 PM ******/
CREATE DATABASE [QLPhongTro]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QLPhongTro_Data', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\QLPhongTro.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'QLPhongTro_Log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\QLPhongTro.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [QLPhongTro] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [QLPhongTro].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [QLPhongTro] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QLPhongTro] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QLPhongTro] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QLPhongTro] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QLPhongTro] SET ARITHABORT OFF 
GO
ALTER DATABASE [QLPhongTro] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [QLPhongTro] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QLPhongTro] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QLPhongTro] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QLPhongTro] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QLPhongTro] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QLPhongTro] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QLPhongTro] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QLPhongTro] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QLPhongTro] SET  DISABLE_BROKER 
GO
ALTER DATABASE [QLPhongTro] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QLPhongTro] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QLPhongTro] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [QLPhongTro] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [QLPhongTro] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QLPhongTro] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [QLPhongTro] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [QLPhongTro] SET RECOVERY FULL 
GO
ALTER DATABASE [QLPhongTro] SET  MULTI_USER 
GO
ALTER DATABASE [QLPhongTro] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QLPhongTro] SET DB_CHAINING OFF 
GO
ALTER DATABASE [QLPhongTro] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [QLPhongTro] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [QLPhongTro] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [QLPhongTro] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [QLPhongTro] SET QUERY_STORE = OFF
GO
USE [QLPhongTro]
GO
/****** Object:  Table [dbo].[ChiTietHoaDon]    Script Date: 12/7/2024 05:59:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietHoaDon](
	[MaChiTiet] [int] IDENTITY(1,1) NOT NULL,
	[MaHoaDon] [int] NULL,
	[MaPhong] [int] NULL,
	[TienPhong] [decimal](25, 2) NULL,
	[TienDien] [decimal](25, 2) NULL,
	[TienNuoc] [decimal](25, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaChiTiet] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dien]    Script Date: 12/7/2024 05:59:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dien](
	[MaDien] [int] IDENTITY(1,1) NOT NULL,
	[NgayDocSo] [date] NOT NULL,
	[ChiSoCu] [decimal](10, 3) NOT NULL,
	[ChiSoMoi] [decimal](10, 3) NULL,
	[GiaTien] [decimal](10, 3) NOT NULL,
	[MaPhong] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaDien] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HoaDon]    Script Date: 12/7/2024 05:59:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HoaDon](
	[MaHoaDon] [int] IDENTITY(1,1) NOT NULL,
	[NgayLap] [date] NOT NULL,
	[ThanhTien] [decimal](25, 2) NULL,
	[MaPhong] [int] NULL,
	[MaKH] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaHoaDon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KhachThue]    Script Date: 12/7/2024 05:59:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KhachThue](
	[MaKhachThue] [int] IDENTITY(1,1) NOT NULL,
	[HoTenDem] [nvarchar](50) NOT NULL,
	[Ten] [nvarchar](50) NOT NULL,
	[CMND] [nvarchar](100) NULL,
	[DienThoai] [nvarchar](20) NULL,
	[NgayVaoO] [date] NOT NULL,
	[NgayTraPhong] [date] NULL,
	[MaPhong] [int] NULL,
	[Mail] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaKhachThue] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LoaiPhong]    Script Date: 12/7/2024 05:59:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LoaiPhong](
	[MaLP] [int] IDENTITY(1,1) NOT NULL,
	[TenLP] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_LoaiPhong] PRIMARY KEY CLUSTERED 
(
	[MaLP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_LoaiPhong] UNIQUE NONCLUSTERED 
(
	[TenLP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Nuoc]    Script Date: 12/7/2024 05:59:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Nuoc](
	[MaNuoc] [int] IDENTITY(1,1) NOT NULL,
	[NgayDocSo] [date] NOT NULL,
	[ChiSoCu] [decimal](10, 3) NOT NULL,
	[ChiSoMoi] [decimal](10, 3) NULL,
	[GiaTien] [decimal](10, 3) NOT NULL,
	[MaPhong] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaNuoc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Phong]    Script Date: 12/7/2024 05:59:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Phong](
	[MaPhong] [int] IDENTITY(1,1) NOT NULL,
	[SoPhong] [nvarchar](50) NOT NULL,
	[LoaiPhong] [nvarchar](50) NOT NULL,
	[GiaThueThang] [decimal](25, 2) NULL,
	[DaThue] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaPhong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaiKhoan]    Script Date: 12/7/2024 05:59:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaiKhoan](
	[MaTaiKhoan] [int] IDENTITY(1,1) NOT NULL,
	[SoDienThoai] [nvarchar](20) NOT NULL,
	[MatKhau] [nvarchar](100) NOT NULL,
	[ChucVu] [nvarchar](50) NOT NULL,
	[TrangThai] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaTaiKhoan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThongKe]    Script Date: 12/7/2024 05:59:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThongKe](
	[Thang] [date] NOT NULL,
	[TongTienThue] [decimal](18, 2) NOT NULL,
	[TongTienNuoc] [decimal](18, 2) NOT NULL,
	[TongTienDien] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Thang] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ChiTietHoaDon]  WITH CHECK ADD FOREIGN KEY([MaHoaDon])
REFERENCES [dbo].[HoaDon] ([MaHoaDon])
GO
ALTER TABLE [dbo].[ChiTietHoaDon]  WITH CHECK ADD FOREIGN KEY([MaPhong])
REFERENCES [dbo].[Phong] ([MaPhong])
GO
ALTER TABLE [dbo].[Dien]  WITH CHECK ADD FOREIGN KEY([MaPhong])
REFERENCES [dbo].[Phong] ([MaPhong])
GO
ALTER TABLE [dbo].[HoaDon]  WITH CHECK ADD FOREIGN KEY([MaPhong])
REFERENCES [dbo].[Phong] ([MaPhong])
GO
ALTER TABLE [dbo].[HoaDon]  WITH CHECK ADD  CONSTRAINT [FK_HoaDon_KhachThue] FOREIGN KEY([MaKH])
REFERENCES [dbo].[KhachThue] ([MaKhachThue])
GO
ALTER TABLE [dbo].[HoaDon] CHECK CONSTRAINT [FK_HoaDon_KhachThue]
GO
ALTER TABLE [dbo].[KhachThue]  WITH CHECK ADD FOREIGN KEY([MaPhong])
REFERENCES [dbo].[Phong] ([MaPhong])
GO
ALTER TABLE [dbo].[Nuoc]  WITH CHECK ADD FOREIGN KEY([MaPhong])
REFERENCES [dbo].[Phong] ([MaPhong])
GO
/****** Object:  StoredProcedure [dbo].[UpdateThongKe]    Script Date: 12/7/2024 05:59:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateThongKe]
AS
BEGIN
    SET NOCOUNT ON;

    WITH DienData AS (
        SELECT 
            DATEFROMPARTS(YEAR(NgayDocSo), MONTH(NgayDocSo), 1) AS Thang,
            SUM((ChiSoMoi - ChiSoCu) * GiaTien) AS TongTienDien
        FROM Dien
        GROUP BY YEAR(NgayDocSo), MONTH(NgayDocSo)
    ),
    NuocData AS (
        SELECT 
            DATEFROMPARTS(YEAR(NgayDocSo), MONTH(NgayDocSo), 1) AS Thang,
            SUM((ChiSoMoi - ChiSoCu) * GiaTien) AS TongTienNuoc
        FROM Nuoc
        GROUP BY YEAR(NgayDocSo), MONTH(NgayDocSo)
    ),
    ThueData AS (
        SELECT 
            DATEFROMPARTS(YEAR(NgayVaoO), MONTH(NgayVaoO), 1) AS Thang,
            SUM(DATEDIFF(MONTH, NgayVaoO, ISNULL(NgayTraPhong, GETDATE())) * GiaThueThang) AS TongTienThue
        FROM KhachThue
        INNER JOIN Phong ON KhachThue.MaPhong = Phong.MaPhong
        WHERE Phong.DaThue = 1
        GROUP BY YEAR(NgayVaoO), MONTH(NgayVaoO)
    )
    MERGE ThongKe AS target
    USING (
        SELECT 
            COALESCE(DienData.Thang, NuocData.Thang, ThueData.Thang) AS Thang,
            ISNULL(ThueData.TongTienThue, 0) AS TongTienThue,
            ISNULL(NuocData.TongTienNuoc, 0) AS TongTienNuoc,
            ISNULL(DienData.TongTienDien, 0) AS TongTienDien
        FROM DienData
        FULL OUTER JOIN NuocData ON DienData.Thang = NuocData.Thang
        FULL OUTER JOIN ThueData ON COALESCE(DienData.Thang, NuocData.Thang) = ThueData.Thang
    ) AS source
    ON target.Thang = source.Thang
    WHEN MATCHED THEN
        UPDATE SET 
            TongTienThue = source.TongTienThue,
            TongTienNuoc = source.TongTienNuoc,
            TongTienDien = source.TongTienDien
    WHEN NOT MATCHED THEN
        INSERT (Thang, TongTienThue, TongTienNuoc, TongTienDien)
        VALUES (source.Thang, source.TongTienThue, source.TongTienNuoc, source.TongTienDien);
END;
GO
USE [master]
GO
ALTER DATABASE [QLPhongTro] SET  READ_WRITE 
GO
