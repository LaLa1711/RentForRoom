USE [QLNhaTro]
GO
/****** Object:  Table [dbo].[tbAlbum]    Script Date: 1/4/2025 11:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbAlbum](
	[IDAlbum] [int] IDENTITY(1,1) NOT NULL,
	[IDPhong] [int] NULL,
	[TieuDe] [nvarchar](150) NULL,
	[HinhAnh] [varchar](200) NULL,
	[Hide] [bit] NULL,
 CONSTRAINT [PK_tbAlbum] PRIMARY KEY CLUSTERED 
(
	[IDAlbum] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbBanner]    Script Date: 1/4/2025 11:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbBanner](
	[IDBanner] [int] IDENTITY(1,1) NOT NULL,
	[TieuDe] [nvarchar](150) NULL,
	[NoiDung] [nvarchar](300) NULL,
	[HinhBanner] [varchar](200) NULL,
	[Hide] [bit] NULL,
 CONSTRAINT [PK_tbBanner] PRIMARY KEY CLUSTERED 
(
	[IDBanner] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbChiTietPhong]    Script Date: 1/4/2025 11:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbChiTietPhong](
	[IDPhong] [int] IDENTITY(1,1) NOT NULL,
	[MaTaiKhoan] [int] NULL,
	[TieuDe] [nvarchar](150) NULL,
	[DiaChi] [nvarchar](150) NULL,
	[IDPhuong] [int] NULL,
	[IDQuan] [int] NULL,
	[IDTP] [int] NULL,
	[GiaThue] [float] NULL,
	[MoTa] [nvarchar](300) NULL,
	[LinkBai] [varchar](300) NULL,
	[GioGiac] [nvarchar](150) NULL,
	[LinkMap] [varchar](300) NULL,
	[TienDien] [float] NULL,
	[TienNuoc] [float] NULL,
	[TienDichVụ] [float] NULL,
	[CuaSoBanCong] [bit] NULL,
	[WC] [bit] NULL,
	[KeBep] [bit] NULL,
	[MayLanh] [int] NULL,
	[Giuong] [int] NULL,
	[TuDo] [int] NULL,
	[Nem] [int] NULL,
	[TuLanh] [int] NULL,
	[BanGhe] [int] NULL,
	[MayGiat] [bit] NULL,
	[Sofa] [int] NULL,
	[SoPhongNgu] [int] NULL,
	[Thang] [bit] NULL,
	[PhuongTien] [nvarchar](150) NULL,
	[ThuCung] [bit] NULL,
	[TienCoc] [bit] NULL,
	[Hide] [bit] NULL,
	[NoiBat] [bit] NULL,
	[TrangThaiXuLy] [bit] NULL,
 CONSTRAINT [PK_tbChiTietPhong] PRIMARY KEY CLUSTERED 
(
	[IDPhong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbDatLich]    Script Date: 1/4/2025 11:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbDatLich](
	[IDDatLich] [int] IDENTITY(1,1) NOT NULL,
	[IDPhong] [int] NULL,
	[SoNguoiO] [int] NULL,
	[SoLuongXe] [int] NULL,
	[NgayXemPhong] [datetime] NULL,
	[NgayChuyenVao] [date] NULL,
	[GhiChu] [nvarchar](max) NULL,
	[ThuCung] [bit] NULL,
	[HoTen] [nvarchar](150) NULL,
	[SDT] [varchar](11) NULL,
	[Email] [nvarchar](150) NULL,
	[Hide] [bit] NULL,
 CONSTRAINT [PK_tbDatLich] PRIMARY KEY CLUSTERED 
(
	[IDDatLich] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbGioiThieu]    Script Date: 1/4/2025 11:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbGioiThieu](
	[IDGioiThieu] [int] IDENTITY(1,1) NOT NULL,
	[TieuDe] [nvarchar](150) NULL,
	[NoiDung] [nvarchar](max) NULL,
	[HinhAnh] [varchar](200) NULL,
	[TieuDeDanhSach] [nvarchar](150) NULL,
	[NoiDungDanhSach] [nvarchar](150) NULL,
	[Hide] [bit] NULL,
 CONSTRAINT [PK_tbGioiThieu] PRIMARY KEY CLUSTERED 
(
	[IDGioiThieu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbQuanHuyen]    Script Date: 1/4/2025 11:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbQuanHuyen](
	[IDTP] [int] NULL,
	[TenQuan] [nvarchar](150) NULL,
	[IDQuan] [int] NOT NULL,
	[Hide] [bit] NULL,
 CONSTRAINT [PK_tbQuanHuye] PRIMARY KEY CLUSTERED 
(
	[IDQuan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbQuyen]    Script Date: 1/4/2025 11:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbQuyen](
	[IDQuyen] [int] IDENTITY(1,1) NOT NULL,
	[TenQuyen] [nvarchar](150) NULL,
	[Hide] [bit] NULL,
 CONSTRAINT [PK_tbRole_1] PRIMARY KEY CLUSTERED 
(
	[IDQuyen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbTinhThanhPho]    Script Date: 1/4/2025 11:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbTinhThanhPho](
	[IDTP] [int] NOT NULL,
	[TenTP] [nvarchar](150) NULL,
	[Hide] [bit] NULL,
 CONSTRAINT [PK_tbTinhThanhPho] PRIMARY KEY CLUSTERED 
(
	[IDTP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbUser]    Script Date: 1/4/2025 11:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbUser](
	[IDUser] [int] IDENTITY(1,1) NOT NULL,
	[MaTaiKhoan] [int] NULL,
	[Role] [int] NULL,
	[SDT] [nvarchar](150) NULL,
	[MatKhau] [varchar](50) NULL,
	[HoTen] [nvarchar](150) NULL,
	[Gmail] [nvarchar](150) NULL,
	[HinhAnh] [varchar](200) NULL,
	[Hide] [bit] NULL,
 CONSTRAINT [PK_tbUser] PRIMARY KEY CLUSTERED 
(
	[IDUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbXaPhuong]    Script Date: 1/4/2025 11:49:43 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbXaPhuong](
	[IDQuan] [int] NULL,
	[TenPhuong] [nvarchar](150) NULL,
	[IDPhuong] [int] NOT NULL,
	[Hide] [bit] NULL,
 CONSTRAINT [PK_tbXaPhuong] PRIMARY KEY CLUSTERED 
(
	[IDPhuong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
