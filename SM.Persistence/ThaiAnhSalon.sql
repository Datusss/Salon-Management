USE [master]
GO
/****** Object:  Database [ThaiAnhSalon]    Script Date: 12/26/2015 7:52:24 AM ******/
CREATE DATABASE [ThaiAnhSalon]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ThaiAnhSalon', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER2014\MSSQL\DATA\ThaiAnhSalon.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ThaiAnhSalon_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER2014\MSSQL\DATA\ThaiAnhSalon_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ThaiAnhSalon] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ThaiAnhSalon].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ThaiAnhSalon] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ThaiAnhSalon] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ThaiAnhSalon] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ThaiAnhSalon] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ThaiAnhSalon] SET ARITHABORT OFF 
GO
ALTER DATABASE [ThaiAnhSalon] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ThaiAnhSalon] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ThaiAnhSalon] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ThaiAnhSalon] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ThaiAnhSalon] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ThaiAnhSalon] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ThaiAnhSalon] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ThaiAnhSalon] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ThaiAnhSalon] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ThaiAnhSalon] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ThaiAnhSalon] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ThaiAnhSalon] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ThaiAnhSalon] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ThaiAnhSalon] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ThaiAnhSalon] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ThaiAnhSalon] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ThaiAnhSalon] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ThaiAnhSalon] SET RECOVERY FULL 
GO
ALTER DATABASE [ThaiAnhSalon] SET  MULTI_USER 
GO
ALTER DATABASE [ThaiAnhSalon] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ThaiAnhSalon] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ThaiAnhSalon] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ThaiAnhSalon] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [ThaiAnhSalon] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'ThaiAnhSalon', N'ON'
GO
USE [ThaiAnhSalon]
GO
/****** Object:  Table [dbo].[Bill]    Script Date: 12/26/2015 7:52:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bill](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Notice] [ntext] NULL,
	[Price] [int] NULL,
	[Discount] [int] NULL,
	[DiscountRatio] [tinyint] NULL,
	[CustomerId] [bigint] NULL,
	[StaffId] [int] NULL,
	[SubStaffId] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
 CONSTRAINT [PK_Bill] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BillService]    Script Date: 12/26/2015 7:52:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillService](
	[ServiceId] [int] NOT NULL,
	[BillId] [bigint] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Customer]    Script Date: 12/26/2015 7:52:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerId] [bigint] IDENTITY(1,1) NOT NULL,
	[CustomerName] [nvarchar](255) NULL,
	[Gender] [bit] NULL,
	[Country] [nvarchar](255) NULL,
	[City] [nvarchar](255) NULL,
	[District] [nvarchar](255) NULL,
	[Address] [nvarchar](255) NULL,
	[Phone] [nvarchar](255) NULL,
	[BirthDay] [datetime] NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Position]    Script Date: 12/26/2015 7:52:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Position](
	[PositionId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
 CONSTRAINT [PK_Position] PRIMARY KEY CLUSTERED 
(
	[PositionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PriceBook]    Script Date: 12/26/2015 7:52:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PriceBook](
	[PriceBookId] [int] IDENTITY(1,1) NOT NULL,
	[ServiceId] [int] NULL,
	[ServiceTypeId] [int] NULL,
	[Price] [bigint] NULL,
 CONSTRAINT [PK_PriceBook] PRIMARY KEY CLUSTERED 
(
	[PriceBookId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ServiceGroup]    Script Date: 12/26/2015 7:52:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceGroup](
	[ServiceGroupId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Description] [nvarchar](500) NULL,
	[Rank] [int] NULL,
 CONSTRAINT [PK_ServiceGroup] PRIMARY KEY CLUSTERED 
(
	[ServiceGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ServiceName]    Script Date: 12/26/2015 7:52:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceName](
	[ServiceId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Description] [nvarchar](500) NULL,
	[ServiceGroupId] [int] NULL,
	[Rank] [int] NULL,
 CONSTRAINT [PK_ServiceType] PRIMARY KEY CLUSTERED 
(
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ServiceType]    Script Date: 12/26/2015 7:52:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceType](
	[ServiceTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Description] [nvarchar](500) NULL,
	[Rank] [int] NULL,
	[ServiceId] [int] NULL,
 CONSTRAINT [PK_ServiceType_1] PRIMARY KEY CLUSTERED 
(
	[ServiceTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Staff]    Script Date: 12/26/2015 7:52:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[StaffId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NULL,
	[BirthDay] [datetime] NULL,
	[Gender] [bit] NULL,
	[PositionId] [int] NULL,
 CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED 
(
	[StaffId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 12/26/2015 7:52:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](255) NULL,
	[Password] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[Phone] [nvarchar](255) NULL,
	[DisplayName] [nvarchar](255) NULL,
	[Gender] [bit] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD  CONSTRAINT [FK_Bill_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[Bill] CHECK CONSTRAINT [FK_Bill_Customer]
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD  CONSTRAINT [FK_Bill_Staff] FOREIGN KEY([StaffId])
REFERENCES [dbo].[Staff] ([StaffId])
GO
ALTER TABLE [dbo].[Bill] CHECK CONSTRAINT [FK_Bill_Staff]
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD  CONSTRAINT [FK_Bill_Staff1] FOREIGN KEY([SubStaffId])
REFERENCES [dbo].[Staff] ([StaffId])
GO
ALTER TABLE [dbo].[Bill] CHECK CONSTRAINT [FK_Bill_Staff1]
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD  CONSTRAINT [FK_Bill_User] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[Bill] CHECK CONSTRAINT [FK_Bill_User]
GO
ALTER TABLE [dbo].[BillService]  WITH CHECK ADD  CONSTRAINT [FK_BillService_Bill] FOREIGN KEY([BillId])
REFERENCES [dbo].[Bill] ([Id])
GO
ALTER TABLE [dbo].[BillService] CHECK CONSTRAINT [FK_BillService_Bill]
GO
ALTER TABLE [dbo].[BillService]  WITH CHECK ADD  CONSTRAINT [FK_BillService_PriceBook] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[PriceBook] ([PriceBookId])
GO
ALTER TABLE [dbo].[BillService] CHECK CONSTRAINT [FK_BillService_PriceBook]
GO
ALTER TABLE [dbo].[PriceBook]  WITH CHECK ADD  CONSTRAINT [FK_PriceBook_ServiceName] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[ServiceName] ([ServiceId])
GO
ALTER TABLE [dbo].[PriceBook] CHECK CONSTRAINT [FK_PriceBook_ServiceName]
GO
ALTER TABLE [dbo].[PriceBook]  WITH CHECK ADD  CONSTRAINT [FK_PriceBook_ServiceType] FOREIGN KEY([ServiceTypeId])
REFERENCES [dbo].[ServiceType] ([ServiceTypeId])
GO
ALTER TABLE [dbo].[PriceBook] CHECK CONSTRAINT [FK_PriceBook_ServiceType]
GO
ALTER TABLE [dbo].[ServiceName]  WITH CHECK ADD  CONSTRAINT [FK_Service_ServiceGroup] FOREIGN KEY([ServiceGroupId])
REFERENCES [dbo].[ServiceGroup] ([ServiceGroupId])
GO
ALTER TABLE [dbo].[ServiceName] CHECK CONSTRAINT [FK_Service_ServiceGroup]
GO
ALTER TABLE [dbo].[ServiceType]  WITH CHECK ADD  CONSTRAINT [FK_ServiceType_Service] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[ServiceName] ([ServiceId])
GO
ALTER TABLE [dbo].[ServiceType] CHECK CONSTRAINT [FK_ServiceType_Service]
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD  CONSTRAINT [FK_Staff_Position] FOREIGN KEY([PositionId])
REFERENCES [dbo].[Position] ([PositionId])
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [FK_Staff_Position]
GO
USE [master]
GO
ALTER DATABASE [ThaiAnhSalon] SET  READ_WRITE 
GO
