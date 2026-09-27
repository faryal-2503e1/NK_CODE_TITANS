USE [master]

GO

/****** Object:  Database [FanHub21]    Script Date: 9/27/2026 4:07:56 PM ******/

CREATE DATABASE [FanHub21]

 CONTAINMENT = NONE

 ON  PRIMARY 

( NAME = N'FanHub21', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL17.SQLEXPRESS\MSSQL\DATA\FanHub21.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )

 LOG ON 

( NAME = N'FanHub21_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL17.SQLEXPRESS\MSSQL\DATA\FanHub21_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )

 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF

GO

ALTER DATABASE [FanHub21] SET COMPATIBILITY_LEVEL = 170

GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))

begin

EXEC [FanHub21].[dbo].[sp_fulltext_database] @action = 'enable'

end

GO

ALTER DATABASE [FanHub21] SET ANSI_NULL_DEFAULT OFF 

GO

ALTER DATABASE [FanHub21] SET ANSI_NULLS OFF 

GO

ALTER DATABASE [FanHub21] SET ANSI_PADDING OFF 

GO

ALTER DATABASE [FanHub21] SET ANSI_WARNINGS OFF 

GO

ALTER DATABASE [FanHub21] SET ARITHABORT OFF 

GO

ALTER DATABASE [FanHub21] SET AUTO_CLOSE ON 

GO

ALTER DATABASE [FanHub21] SET AUTO_SHRINK OFF 

GO

ALTER DATABASE [FanHub21] SET AUTO_UPDATE_STATISTICS ON 

GO

ALTER DATABASE [FanHub21] SET CURSOR_CLOSE_ON_COMMIT OFF 

GO

ALTER DATABASE [FanHub21] SET CURSOR_DEFAULT  GLOBAL 

GO

ALTER DATABASE [FanHub21] SET CONCAT_NULL_YIELDS_NULL OFF 

GO

ALTER DATABASE [FanHub21] SET NUMERIC_ROUNDABORT OFF 

GO

ALTER DATABASE [FanHub21] SET QUOTED_IDENTIFIER OFF 

GO

ALTER DATABASE [FanHub21] SET RECURSIVE_TRIGGERS OFF 

GO

ALTER DATABASE [FanHub21] SET  ENABLE_BROKER 

GO

ALTER DATABASE [FanHub21] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 

GO

ALTER DATABASE [FanHub21] SET DATE_CORRELATION_OPTIMIZATION OFF 

GO

ALTER DATABASE [FanHub21] SET TRUSTWORTHY OFF 

GO

ALTER DATABASE [FanHub21] SET ALLOW_SNAPSHOT_ISOLATION OFF 

GO

ALTER DATABASE [FanHub21] SET PARAMETERIZATION SIMPLE 

GO

ALTER DATABASE [FanHub21] SET READ_COMMITTED_SNAPSHOT ON 

GO

ALTER DATABASE [FanHub21] SET HONOR_BROKER_PRIORITY OFF 

GO

ALTER DATABASE [FanHub21] SET RECOVERY SIMPLE 

GO

ALTER DATABASE [FanHub21] SET  MULTI_USER 

GO

ALTER DATABASE [FanHub21] SET PAGE_VERIFY CHECKSUM  

GO

ALTER DATABASE [FanHub21] SET DB_CHAINING OFF 

GO

ALTER DATABASE [FanHub21] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 

GO

ALTER DATABASE [FanHub21] SET TARGET_RECOVERY_TIME = 60 SECONDS 

GO

ALTER DATABASE [FanHub21] SET DELAYED_DURABILITY = DISABLED 

GO

ALTER DATABASE [FanHub21] SET OPTIMIZED_LOCKING = OFF 

GO

ALTER DATABASE [FanHub21] SET ACCELERATED_DATABASE_RECOVERY = OFF  

GO

ALTER DATABASE [FanHub21] SET QUERY_STORE = ON

GO

ALTER DATABASE [FanHub21] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)

GO

USE [FanHub21]

GO

/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 9/27/2026 4:07:57 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[__EFMigrationsHistory](

	[MigrationId] [nvarchar](150) NOT NULL,

	[ProductVersion] [nvarchar](32) NOT NULL,

 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 

(

	[MigrationId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ActivityLogs]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[ActivityLogs](

	[LogId] [int] IDENTITY(1,1) NOT NULL,

	[UserId] [nvarchar](max) NOT NULL,

	[ActivityType] [nvarchar](100) NOT NULL,

	[Details] [nvarchar](500) NOT NULL,

	[Timestamp] [datetime2](7) NOT NULL,

 CONSTRAINT [PK_ActivityLogs] PRIMARY KEY CLUSTERED 

(

	[LogId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[AspNetRoleClaims](

	[Id] [int] IDENTITY(1,1) NOT NULL,

	[RoleId] [nvarchar](450) NOT NULL,

	[ClaimType] [nvarchar](max) NULL,

	[ClaimValue] [nvarchar](max) NULL,

 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 

(

	[Id] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[AspNetRoles](

	[Id] [nvarchar](450) NOT NULL,

	[Name] [nvarchar](256) NULL,

	[NormalizedName] [nvarchar](256) NULL,

	[ConcurrencyStamp] [nvarchar](max) NULL,

 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 

(

	[Id] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[AspNetUserClaims](

	[Id] [int] IDENTITY(1,1) NOT NULL,

	[UserId] [nvarchar](450) NOT NULL,

	[ClaimType] [nvarchar](max) NULL,

	[ClaimValue] [nvarchar](max) NULL,

 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 

(

	[Id] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[AspNetUserLogins](

	[LoginProvider] [nvarchar](128) NOT NULL,

	[ProviderKey] [nvarchar](128) NOT NULL,

	[ProviderDisplayName] [nvarchar](max) NULL,

	[UserId] [nvarchar](450) NOT NULL,

 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 

(

	[LoginProvider] ASC,

	[ProviderKey] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[AspNetUserRoles](

	[UserId] [nvarchar](450) NOT NULL,

	[RoleId] [nvarchar](450) NOT NULL,

 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 

(

	[UserId] ASC,

	[RoleId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[AspNetUsers](

	[Id] [nvarchar](450) NOT NULL,

	[FullName] [nvarchar](max) NULL,

	[Address] [nvarchar](max) NULL,

	[ProfilePictureUrl] [nvarchar](max) NULL,

	[UserName] [nvarchar](256) NULL,

	[NormalizedUserName] [nvarchar](256) NULL,

	[Email] [nvarchar](256) NULL,

	[NormalizedEmail] [nvarchar](256) NULL,

	[EmailConfirmed] [bit] NOT NULL,

	[PasswordHash] [nvarchar](max) NULL,

	[SecurityStamp] [nvarchar](max) NULL,

	[ConcurrencyStamp] [nvarchar](max) NULL,

	[PhoneNumber] [nvarchar](max) NULL,

	[PhoneNumberConfirmed] [bit] NOT NULL,

	[TwoFactorEnabled] [bit] NOT NULL,

	[LockoutEnd] [datetimeoffset](7) NULL,

	[LockoutEnabled] [bit] NOT NULL,

	[AccessFailedCount] [int] NOT NULL,

 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 

(

	[Id] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[AspNetUserTokens](

	[UserId] [nvarchar](450) NOT NULL,

	[LoginProvider] [nvarchar](128) NOT NULL,

	[Name] [nvarchar](128) NOT NULL,

	[Value] [nvarchar](max) NULL,

 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 

(

	[UserId] ASC,

	[LoginProvider] ASC,

	[Name] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Bookmarks]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Bookmarks](

	[BookmarkId] [int] IDENTITY(1,1) NOT NULL,

	[UserId] [nvarchar](max) NOT NULL,

	[ItemType] [nvarchar](50) NOT NULL,

	[ContentId] [int] NULL,

	[CharacterId] [int] NULL,

	[MediaItemId] [int] NULL,

	[MerchandiseItemId] [int] NULL,

	[NoteText] [nvarchar](500) NOT NULL,

	[CreatedAt] [datetime2](7) NOT NULL,

	[ApplicationUserId] [nvarchar](450) NULL,

 CONSTRAINT [PK_Bookmarks] PRIMARY KEY CLUSTERED 

(

	[BookmarkId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Categories]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Categories](

	[CategoryId] [int] IDENTITY(1,1) NOT NULL,

	[Name] [nvarchar](100) NOT NULL,

	[Description] [nvarchar](500) NULL,

 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 

(

	[CategoryId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[CharacterProfiles]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[CharacterProfiles](

	[CharacterId] [int] IDENTITY(1,1) NOT NULL,

	[CategoryId] [int] NOT NULL,

	[Name] [nvarchar](150) NOT NULL,

	[Bio] [nvarchar](max) NULL,

	[ImageUrl] [nvarchar](500) NULL,

 CONSTRAINT [PK_CharacterProfiles] PRIMARY KEY CLUSTERED 

(

	[CharacterId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ChatbotFaqs]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[ChatbotFaqs](

	[FaqId] [int] IDENTITY(1,1) NOT NULL,

	[Question] [nvarchar](300) NOT NULL,

	[Answer] [nvarchar](max) NOT NULL,

 CONSTRAINT [PK_ChatbotFaqs] PRIMARY KEY CLUSTERED 

(

	[FaqId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ChatbotKnowledgeBaseEntries]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[ChatbotKnowledgeBaseEntries](

	[EntryId] [int] IDENTITY(1,1) NOT NULL,

	[Topic] [nvarchar](200) NOT NULL,

	[Content] [nvarchar](max) NOT NULL,

 CONSTRAINT [PK_ChatbotKnowledgeBaseEntries] PRIMARY KEY CLUSTERED 

(

	[EntryId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ChatbotQueries]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[ChatbotQueries](

	[QueryId] [int] IDENTITY(1,1) NOT NULL,

	[UserId] [nvarchar](max) NOT NULL,

	[Message] [nvarchar](max) NOT NULL,

	[Response] [nvarchar](max) NOT NULL,

	[CreatedAt] [datetime2](7) NOT NULL,

	[ApplicationUserId] [nvarchar](450) NULL,

 CONSTRAINT [PK_ChatbotQueries] PRIMARY KEY CLUSTERED 

(

	[QueryId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Contents]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Contents](

	[ContentId] [int] IDENTITY(1,1) NOT NULL,

	[CategoryId] [int] NOT NULL,

	[Title] [nvarchar](200) NOT NULL,

	[Type] [nvarchar](50) NOT NULL,

	[Description] [nvarchar](max) NULL,

	[ReleaseDate] [datetime2](7) NOT NULL,

	[PopularityScore] [int] NOT NULL,

 CONSTRAINT [PK_Contents] PRIMARY KEY CLUSTERED 

(

	[ContentId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ContentTags]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[ContentTags](

	[ContentId] [int] NOT NULL,

	[TagId] [int] NOT NULL,

 CONSTRAINT [PK_ContentTags] PRIMARY KEY CLUSTERED 

(

	[ContentId] ASC,

	[TagId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[ContentViews]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[ContentViews](

	[ViewId] [int] IDENTITY(1,1) NOT NULL,

	[ContentId] [int] NOT NULL,

	[UserId] [nvarchar](max) NOT NULL,

	[ViewedAt] [datetime2](7) NOT NULL,

 CONSTRAINT [PK_ContentViews] PRIMARY KEY CLUSTERED 

(

	[ViewId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EventCategories]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[EventCategories](

	[EventId] [int] NOT NULL,

	[CategoryId] [int] NOT NULL,

 CONSTRAINT [PK_EventCategories] PRIMARY KEY CLUSTERED 

(

	[EventId] ASC,

	[CategoryId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EventChatMessages]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[EventChatMessages](

	[MessageId] [int] IDENTITY(1,1) NOT NULL,

	[EventId] [int] NOT NULL,

	[UserId] [nvarchar](450) NOT NULL,

	[UserName] [nvarchar](100) NOT NULL,

	[Message] [nvarchar](500) NOT NULL,

	[SentAt] [datetime2](7) NOT NULL,

PRIMARY KEY CLUSTERED 

(

	[MessageId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Events]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Events](

	[EventId] [int] IDENTITY(1,1) NOT NULL,

	[Title] [nvarchar](200) NOT NULL,

	[Description] [nvarchar](max) NULL,

	[EventDate] [datetime2](7) NOT NULL,

	[City] [nvarchar](100) NULL,

	[LocationCoordinates] [nvarchar](300) NULL,

 CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED 

(

	[EventId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[EventTicketLinks]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[EventTicketLinks](

	[TicketLinkId] [int] IDENTITY(1,1) NOT NULL,

	[EventId] [int] NOT NULL,

	[PlatformName] [nvarchar](100) NOT NULL,

	[Url] [nvarchar](500) NOT NULL,

 CONSTRAINT [PK_EventTicketLinks] PRIMARY KEY CLUSTERED 

(

	[TicketLinkId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FanSubmissions]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[FanSubmissions](

	[SubmissionId] [int] IDENTITY(1,1) NOT NULL,

	[UserId] [nvarchar](max) NOT NULL,

	[Title] [nvarchar](200) NOT NULL,

	[Content] [nvarchar](max) NOT NULL,

	[MediaUrl] [nvarchar](500) NULL,

	[Status] [nvarchar](50) NOT NULL,

	[SubmittedAt] [datetime2](7) NOT NULL,

	[ApplicationUserId] [nvarchar](450) NULL,

 CONSTRAINT [PK_FanSubmissions] PRIMARY KEY CLUSTERED 

(

	[SubmissionId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[FeaturedArticles]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[FeaturedArticles](

	[ArticleId] [int] IDENTITY(1,1) NOT NULL,

	[Title] [nvarchar](250) NOT NULL,

	[Content] [nvarchar](max) NOT NULL,

	[ImageUrl] [nvarchar](500) NULL,

	[PublishedAt] [datetime2](7) NOT NULL,

 CONSTRAINT [PK_FeaturedArticles] PRIMARY KEY CLUSTERED 

(

	[ArticleId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Feedbacks]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Feedbacks](

	[FeedbackId] [int] IDENTITY(1,1) NOT NULL,

	[UserId] [nvarchar](max) NOT NULL,

	[Type] [nvarchar](50) NOT NULL,

	[Message] [nvarchar](max) NOT NULL,

	[Status] [nvarchar](50) NOT NULL,

	[CreatedAt] [datetime2](7) NOT NULL,

	[ApplicationUserId] [nvarchar](450) NULL,

 CONSTRAINT [PK_Feedbacks] PRIMARY KEY CLUSTERED 

(

	[FeedbackId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MediaItems]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[MediaItems](

	[MediaId] [int] IDENTITY(1,1) NOT NULL,

	[Title] [nvarchar](200) NOT NULL,

	[FileUrl] [nvarchar](500) NULL,

	[MediaType] [nvarchar](50) NULL,

	[UploadedAt] [datetime2](7) NOT NULL,

	[CategoryId] [int] NULL,

 CONSTRAINT [PK_MediaItems] PRIMARY KEY CLUSTERED 

(

	[MediaId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MediaRatings]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[MediaRatings](

	[RatingId] [int] IDENTITY(1,1) NOT NULL,

	[MediaId] [int] NOT NULL,

	[UserId] [nvarchar](450) NOT NULL,

	[Rating] [int] NOT NULL,

	[CreatedAt] [datetime2](7) NOT NULL,

	[ApplicationUserId] [nvarchar](450) NULL,

 CONSTRAINT [PK_MediaRatings] PRIMARY KEY CLUSTERED 

(

	[RatingId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MediaTags]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[MediaTags](

	[MediaId] [int] NOT NULL,

	[TagId] [int] NOT NULL,

 CONSTRAINT [PK_MediaTags] PRIMARY KEY CLUSTERED 

(

	[MediaId] ASC,

	[TagId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MerchandiseItems]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[MerchandiseItems](

	[ItemId] [int] IDENTITY(1,1) NOT NULL,

	[CategoryId] [int] NOT NULL,

	[Name] [nvarchar](150) NOT NULL,

	[ImageUrl] [nvarchar](500) NULL,

	[Tag] [nvarchar](50) NULL,

	[IsUpcoming] [bit] NOT NULL,

 CONSTRAINT [PK_MerchandiseItems] PRIMARY KEY CLUSTERED 

(

	[ItemId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[MerchandiseViews]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[MerchandiseViews](

	[ViewId] [int] IDENTITY(1,1) NOT NULL,

	[ItemId] [int] NOT NULL,

	[UserId] [nvarchar](max) NOT NULL,

	[ViewedAt] [datetime2](7) NOT NULL,

 CONSTRAINT [PK_MerchandiseViews] PRIMARY KEY CLUSTERED 

(

	[ViewId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Notes]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Notes](

	[NoteId] [int] IDENTITY(1,1) NOT NULL,

	[UserId] [nvarchar](max) NOT NULL,

	[Title] [nvarchar](200) NOT NULL,

	[Content] [nvarchar](max) NOT NULL,

	[CreatedAt] [datetime2](7) NOT NULL,

	[ApplicationUserId] [nvarchar](450) NULL,

 CONSTRAINT [PK_Notes] PRIMARY KEY CLUSTERED 

(

	[NoteId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PollOptions]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[PollOptions](

	[OptionId] [int] IDENTITY(1,1) NOT NULL,

	[PollId] [int] NOT NULL,

	[OptionText] [nvarchar](150) NOT NULL,

PRIMARY KEY CLUSTERED 

(

	[OptionId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Polls]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Polls](

	[PollId] [int] IDENTITY(1,1) NOT NULL,

	[CategoryId] [int] NULL,

	[Title] [nvarchar](200) NOT NULL,

	[IsActive] [bit] NOT NULL,

	[CreatedAt] [datetime2](7) NOT NULL,

PRIMARY KEY CLUSTERED 

(

	[PollId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[PollVotes]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[PollVotes](

	[VoteId] [int] IDENTITY(1,1) NOT NULL,

	[PollId] [int] NOT NULL,

	[OptionId] [int] NOT NULL,

	[UserId] [nvarchar](450) NOT NULL,

	[VotedAt] [datetime2](7) NOT NULL,

PRIMARY KEY CLUSTERED 

(

	[VoteId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[QuizAttempts]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[QuizAttempts](

	[AttemptId] [int] IDENTITY(1,1) NOT NULL,

	[QuizId] [int] NOT NULL,

	[UserId] [nvarchar](450) NOT NULL,

	[Score] [int] NOT NULL,

	[TotalQuestions] [int] NOT NULL,

	[PercentageScore] [float] NOT NULL,

	[AttemptedAt] [datetime2](7) NOT NULL,

PRIMARY KEY CLUSTERED 

(

	[AttemptId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[QuizQuestions]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[QuizQuestions](

	[QuestionId] [int] IDENTITY(1,1) NOT NULL,

	[QuizId] [int] NOT NULL,

	[QuestionText] [nvarchar](500) NOT NULL,

	[OptionA] [nvarchar](200) NOT NULL,

	[OptionB] [nvarchar](200) NOT NULL,

	[OptionC] [nvarchar](200) NOT NULL,

	[OptionD] [nvarchar](200) NOT NULL,

	[CorrectOption] [nvarchar](1) NOT NULL,

PRIMARY KEY CLUSTERED 

(

	[QuestionId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Quizzes]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Quizzes](

	[QuizId] [int] IDENTITY(1,1) NOT NULL,

	[CategoryId] [int] NOT NULL,

	[Title] [nvarchar](150) NOT NULL,

	[Description] [nvarchar](500) NULL,

	[CreatedAt] [datetime2](7) NOT NULL,

PRIMARY KEY CLUSTERED 

(

	[QuizId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Shares]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Shares](

	[ShareId] [int] IDENTITY(1,1) NOT NULL,

	[UserId] [nvarchar](max) NOT NULL,

	[Platform] [nvarchar](50) NOT NULL,

	[SharedUrl] [nvarchar](500) NOT NULL,

	[SharedAt] [datetime2](7) NOT NULL,

	[ApplicationUserId] [nvarchar](450) NULL,

 CONSTRAINT [PK_Shares] PRIMARY KEY CLUSTERED 

(

	[ShareId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[Tags]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[Tags](

	[TagId] [int] IDENTITY(1,1) NOT NULL,

	[Name] [nvarchar](50) NOT NULL,

 CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED 

(

	[TagId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY]

GO

/****** Object:  Table [dbo].[UserActivities]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[UserActivities](

	[ActivityId] [int] IDENTITY(1,1) NOT NULL,

	[UserId] [nvarchar](max) NOT NULL,

	[ActionDescription] [nvarchar](200) NOT NULL,

	[Timestamp] [datetime2](7) NOT NULL,

	[ApplicationUserId] [nvarchar](450) NULL,

 CONSTRAINT [PK_UserActivities] PRIMARY KEY CLUSTERED 

(

	[ActivityId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[UserFavoriteCategories]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[UserFavoriteCategories](

	[FavoriteId] [int] IDENTITY(1,1) NOT NULL,

	[UserId] [nvarchar](max) NOT NULL,

	[CategoryId] [int] NOT NULL,

	[ApplicationUserId] [nvarchar](450) NULL,

 CONSTRAINT [PK_UserFavoriteCategories] PRIMARY KEY CLUSTERED 

(

	[FavoriteId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

/****** Object:  Table [dbo].[UserPreferences]    Script Date: 9/27/2026 4:07:58 PM ******/

SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO

CREATE TABLE [dbo].[UserPreferences](

	[PreferenceId] [int] IDENTITY(1,1) NOT NULL,

	[UserId] [nvarchar](max) NOT NULL,

	[ThemeMode] [nvarchar](50) NOT NULL,

	[FontSize] [nvarchar](20) NOT NULL,

	[EmailNotifications] [bit] NOT NULL,

	[ApplicationUserId] [nvarchar](450) NULL,

 CONSTRAINT [PK_UserPreferences] PRIMARY KEY CLUSTERED 

(

	[PreferenceId] ASC

)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]

) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260924095837_MergedModels', N'8.0.23')

INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260926142055_AddQuizSystem', N'8.0.23')

GO

INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'8bb28dc4-4106-4cab-9913-e4b7ba83893c', N'Customer', N'CUSTOMER', NULL)

INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'c13b9ab6-45b5-4552-b3be-f6eebcec4c9e', N'Admin', N'ADMIN', NULL)

GO

INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'42d7a79f-0725-42cf-bd1f-85fdc4c6e054', N'8bb28dc4-4106-4cab-9913-e4b7ba83893c')

INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'71baae65-44ac-4ad3-9b4c-390a6e31eb53', N'8bb28dc4-4106-4cab-9913-e4b7ba83893c')

INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'88ad1b1f-360f-4266-afc8-c952aefc2141', N'8bb28dc4-4106-4cab-9913-e4b7ba83893c')

INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'd1f26ac5-631f-4fd9-8f35-378cd68c8944', N'c13b9ab6-45b5-4552-b3be-f6eebcec4c9e')

GO

INSERT [dbo].[AspNetUsers] ([Id], [FullName], [Address], [ProfilePictureUrl], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'42d7a79f-0725-42cf-bd1f-85fdc4c6e054', NULL, NULL, NULL, N'irmah@gmail.com', N'IRMAH@GMAIL.COM', N'irmah@gmail.com', N'IRMAH@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAEAD2Mm9G32BDbrGZ3xJzFV42t72Dlqp9yDJFsvwW+Ds7SWbqGeEGUbMmar8yEKfm8A==', N'UA4QNHLGVSOZAGWKRAKP322BNMQVT7XN', N'9f685e37-311e-44ee-af82-7d38a7f9b57a', NULL, 0, 0, NULL, 1, 0)

INSERT [dbo].[AspNetUsers] ([Id], [FullName], [Address], [ProfilePictureUrl], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'71baae65-44ac-4ad3-9b4c-390a6e31eb53', NULL, NULL, NULL, N'nadeemirmah@gmail.com', N'NADEEMIRMAH@GMAIL.COM', N'nadeemirmah@gmail.com', N'NADEEMIRMAH@GMAIL.COM', 1, N'AQAAAAIAAYagAAAAEFjAD4svoImynHwEPDOOVuNqytS0lkHvpDtKdPqGUslxKT7KWqVJVtX1S5OqGzMH/g==', N'SCODAIORVJTM4UHTZQ3GUVTVMZW33W4Z', N'9c34d747-b923-4a83-a9f3-69edbead9089', NULL, 0, 0, NULL, 1, 0)

INSERT [dbo].[AspNetUsers] ([Id], [FullName], [Address], [ProfilePictureUrl], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'88ad1b1f-360f-4266-afc8-c952aefc2141', NULL, NULL, NULL, N'azeemmoiz522@gmail.com', N'AZEEMMOIZ522@GMAIL.COM', N'azeemmoiz522@gmail.com', N'AZEEMMOIZ522@GMAIL.COM', 0, N'AQAAAAIAAYagAAAAEBwuxeuSFzols27Ztm4Ug/M7MTTgB/9uFpyyY0q4FZLmOs5/Pg440feGej/N4NUP8w==', N'KZ6OKIWZ6ZRDMTLWXAZ6E47J5NMGKA3G', N'b7863b1a-1021-49ca-8531-f370e14dffea', NULL, 0, 0, NULL, 1, 0)

INSERT [dbo].[AspNetUsers] ([Id], [FullName], [Address], [ProfilePictureUrl], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'd1f26ac5-631f-4fd9-8f35-378cd68c8944', N'Irmah khan', NULL, NULL, N'admin@demo.com', N'ADMIN@DEMO.COM', N'admin@demo.com', N'ADMIN@DEMO.COM', 1, N'AQAAAAIAAYagAAAAEMROHoi6FMgMOuYlflslYrm6zs8N4QU/GeTl/w/kSyrFP+BFVMGMVMYCfgX8auSAbQ==', N'FIKR776RU7DS64AQJHORW6CLOJJJGFRT', N'b74d19d6-f081-4baf-af19-cba2b5d33dae', NULL, 0, 0, NULL, 1, 0)

GO

SET IDENTITY_INSERT [dbo].[Bookmarks] ON 



INSERT [dbo].[Bookmarks] ([BookmarkId], [UserId], [ItemType], [ContentId], [CharacterId], [MediaItemId], [MerchandiseItemId], [NoteText], [CreatedAt], [ApplicationUserId]) VALUES (2, N'42d7a79f-0725-42cf-bd1f-85fdc4c6e054', N'Content', 5, NULL, NULL, NULL, N'', CAST(N'2026-09-26T19:35:54.4064402' AS DateTime2), NULL)

SET IDENTITY_INSERT [dbo].[Bookmarks] OFF

GO

SET IDENTITY_INSERT [dbo].[Categories] ON 



INSERT [dbo].[Categories] ([CategoryId], [Name], [Description]) VALUES (1, N'Anime', N'Anime series, films and the characters behind them.?')

INSERT [dbo].[Categories] ([CategoryId], [Name], [Description]) VALUES (2, N'Gaming', N'Video games, esports and gaming culture.')

INSERT [dbo].[Categories] ([CategoryId], [Name], [Description]) VALUES (3, N'Movies', N'Feature films, premieres and trailers.')

INSERT [dbo].[Categories] ([CategoryId], [Name], [Description]) VALUES (4, N'TV Shows', N'Television series, seasons and episode news.')

INSERT [dbo].[Categories] ([CategoryId], [Name], [Description]) VALUES (5, N'K-Pop', N'Korean pop artists, releases and fan culture.')

INSERT [dbo].[Categories] ([CategoryId], [Name], [Description]) VALUES (6, N'Comics', N'Comic books, graphic novels and creators.')

INSERT [dbo].[Categories] ([CategoryId], [Name], [Description]) VALUES (7, N'Manga', N'Manga series, chapters and volume releases.')

INSERT [dbo].[Categories] ([CategoryId], [Name], [Description]) VALUES (8, N'Cosplay', N'Cosplay showcases, meetups and craft guides.')

SET IDENTITY_INSERT [dbo].[Categories] OFF

GO

SET IDENTITY_INSERT [dbo].[CharacterProfiles] ON 



INSERT [dbo].[CharacterProfiles] ([CharacterId], [CategoryId], [Name], [Bio], [ImageUrl]) VALUES (2, 1, N'Kaito Tanaka', N'A strong-willed and determined young hero with a signature spiky hairstyle, striving to protect his world from dark forces.', N'/uploads/characters/7a78a389aa5b4edca118c202924325a7.jpg')

INSERT [dbo].[CharacterProfiles] ([CharacterId], [CategoryId], [Name], [Bio], [ImageUrl]) VALUES (3, 2, N'Ren Akabane', N'A calm, highly strategic, and powerful combat expert who constantly pushes his limits in the virtual arena.', N'/uploads/characters/62811533d5a146f8acc2b29d0cf9022a.jpg')

INSERT [dbo].[CharacterProfiles] ([CharacterId], [CategoryId], [Name], [Bio], [ImageUrl]) VALUES (4, 3, N'Sora Hoshino', N'A loyal and brilliant friend who serves as the tactical and tech expert, always keeping the team grounded.', N'/uploads/characters/50e89e1aaa864631ae2ea4cac82599e2.jpg')

INSERT [dbo].[CharacterProfiles] ([CharacterId], [CategoryId], [Name], [Bio], [ImageUrl]) VALUES (5, 7, N'Master Jin', N'A wise veteran with a mysterious past and unmatched high-tier combat skills, guiding the next generation of fighters.', N'/uploads/characters/9160e2c0193c4f1e84b38a75fb108d1c.jpg')

INSERT [dbo].[CharacterProfiles] ([CharacterId], [CategoryId], [Name], [Bio], [ImageUrl]) VALUES (6, 4, N'Malakor', N'A formidable villain driven by a complex backstory and deep philosophical motives, believing his actions will bring true balance.', N'/uploads/characters/8d901d11b3654074b95a9f69b76e83de.jpg')

SET IDENTITY_INSERT [dbo].[CharacterProfiles] OFF

GO

SET IDENTITY_INSERT [dbo].[ChatbotFaqs] ON 



INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (1, N'what is this website', N'FanHubPlus is a community hub for fans to explore content, characters, events, and merchandise, and connect with other fans who share the same interests.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (2, N'how do i bookmark something', N'Open any content, character, media or merchandise page and click the Bookmark button. You need to be logged in to save bookmarks.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (3, N'how do i submit fan art', N'Go to Fan Submissions from your dashboard and click "Submit Your Work". Every submission is reviewed before it''s published.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (4, N'how do i create an account', N'Click Register at the top of the page and fill in your email and password to get started.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (5, N'how do i contact support', N'You can reach out through the Contact page, or send us feedback from your dashboard''s Feedback section.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (6, N'how do i change my profile picture', N'Go to your Profile, click Edit Profile, and upload a new picture from the Profile Picture section.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (7, N'How do I create an account on Fan Hub Plus?', N'Click Register on the top navigation bar, enter your name, email, and password, then confirm your email using the link we send you. Once verified, you can log in and start exploring.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (8, N'I forgot my password. How do I reset it?', N'On the login page, click ''Forgot password?'', enter the email address you registered with, and we''ll send you a secure link to set a new password.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (9, N'Do I need an account to browse content?', N'No. Visitors can browse categories, content, character profiles, and merchandise without logging in. You only need an account to bookmark items, leave ratings, take notes, or submit fan content.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (10, N'What fandom categories does the platform cover?', N'Anime, Gaming, Movies, TV Shows, K-Pop, Comics, Manga, and Cosplay. You can browse each category from the homepage or the category menu.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (11, N'How can I search or filter content?', N'Use the search bar to find content by title, or use the filters for category, genre, release year, content type, and popularity. You can also sort results by latest, most popular, or alphabetically.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (12, N'Can I update my profile picture and favorite fandoms?', N'Yes. Go to your Profile page, click Edit, and you can upload a profile picture, change your favorite fandoms, and update your display preferences at any time.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (13, N'What shows up on my personal dashboard?', N'Your dashboard shows a personalized greeting, your recent activity, your favorite fandoms, and the items you''ve bookmarked, all in one place.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (14, N'How do I bookmark an article, character, video, or merchandise item?', N'Open the item you''re interested in and click the bookmark icon. You can view and manage all your saved items later from your Dashboard or the Bookmarks page.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (15, N'Can I add personal notes to a bookmarked item?', N'Yes. Open a bookmarked item and use the Notes option to add, edit, or delete your own private notes about it.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (16, N'How do I rate or review a video, trailer, or audio clip?', N'On the media item''s page, use the star rating or thumbs-up/down control below the player to leave your feedback.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (17, N'Can I filter character profiles by fandom?', N'Yes. On the Character Profiles page, use the category filter to show only characters from a specific fandom, such as Anime or Gaming.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (18, N'How do I find upcoming anime, games, movies, or merchandise releases?', N'Check the Upcoming Releases section under Merchandise or Content. It lists anticipated anime, games, movies, shows, comics, and merchandise drops with tags like ''Pre-Order'' or ''Limited Edition''.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (19, N'Can I submit my own fan content or articles?', N'Yes. Go to Fan Submissions and click ''Submit Content''. Your submission will be reviewed by an admin before it appears publicly on the platform.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (20, N'How long does it take for my fan submission to be approved?', N'Submissions are reviewed by our admin team as quickly as possible. You''ll see the status (pending, approved, or rejected) on your My Submissions page.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (21, N'How do I find fan conventions, meetups, or screenings near me?', N'Open the Events page and either allow location access to see nearby events on the map, or use the city filter in the Event Calendar to browse events by location.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (22, N'Do event listings include ticket links?', N'Yes. Where available, each event page includes a link to the official ticket or registration platform.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (23, N'How do I report a bug or leave feedback?', N'Go to the Feedback page, choose a type (bug, suggestion, or query), describe the issue, and submit. Our admin team reviews all feedback submitted through this form.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (24, N'Can I enable dark mode or change the font size?', N'Yes. Use the accessibility controls in the site header to toggle dark mode or increase/decrease the font size for easier reading.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (25, N'Can I purchase merchandise directly from the website?', N'No. Fan Hub Plus only showcases merchandise for display and discovery. We don''t process orders or payments; the merchandise section is for browsing only.')

INSERT [dbo].[ChatbotFaqs] ([FaqId], [Question], [Answer]) VALUES (26, N'What can you (the chatbot) help me with?', N'I can answer questions about the platform, help you find content or categories based on what you''re interested in, and guide you through features like bookmarking, submissions, and events.')

SET IDENTITY_INSERT [dbo].[ChatbotFaqs] OFF

GO

SET IDENTITY_INSERT [dbo].[ChatbotKnowledgeBaseEntries] ON 



INSERT [dbo].[ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (1, N'Bookmarks', N'Bookmarks let registered users save any content, character, media item or merchandise item for later. Login is required.')

INSERT [dbo].[ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (2, N'Events Management', N'Explore all upcoming global anime conventions, cosplay championships, art exhibitions, and live music festivals with dates and locations.')

INSERT [dbo].[ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (3, N'Character Profiles', N'Discover detailed profiles of protagonists, rivals, mentors, and antagonists categorized across anime, gaming, manga, and movies.')

INSERT [dbo].[ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (4, N'Fan Submissions', N'Registered users can submit their own custom fan art, cosplay photos, and reviews through the user portal for admin review.')

INSERT [dbo].[ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (5, N'Merchandise Store', N'Browse through official collectibles, apparel, action figures, artbooks, and night lamps with secure ordering details.')

INSERT [dbo].[ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (6, N'Account Registration', N'New visitors can register with an email address and password. A confirmation link is sent by email, and the account becomes active once verified. No account is required for browsing.')

INSERT [dbo].[ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (7, N'Content Explorer and Filters', N'The Content Explorer pulls curated articles, profiles, and media from the database. Users can filter by category, genre, release year, popularity, or content type, and sort by latest, most popular, or alphabetical order.')

INSERT [dbo].[ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (8, N'Bookmarking and Notes', N'Registered users can bookmark any article, character profile, video, or merchandise item, and attach a personal note to it. Bookmarks are visible from the user''s dashboard.')

INSERT [dbo].[ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (9, N'Fan Submission Workflow', N'Registered users can submit fan-made content or articles. Every submission starts in a Pending state and only becomes publicly visible once an admin approves it from the Fan Submissions panel.')

INSERT [dbo].[ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (10, N'Event Discovery', N'Events (conventions, meetups, screenings) can be discovered either through map/GPS-based nearby search or through a city-filterable calendar. Listings may include an external ticket link.')

INSERT [dbo].[ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (11, N'Merchandise Showcase', N'Merchandise items are grouped by fandom and category, and tagged as Limited Edition, Pre-Order, or Collectible by the admin. The platform does not support purchasing, checkout, or payments.')

INSERT [dbo].[ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (12, N'Feedback Types', N'User feedback is categorized as Bug, Suggestion, or Query when submitted, which helps the admin team triage and respond appropriately.')

INSERT [dbo].[ChatbotKnowledgeBaseEntries] ([EntryId], [Topic], [Content]) VALUES (13, N'Accessibility Features', N'The platform supports a dark mode toggle and font-size adjustment to improve readability, along with breadcrumb navigation across category pages.')

SET IDENTITY_INSERT [dbo].[ChatbotKnowledgeBaseEntries] OFF

GO

SET IDENTITY_INSERT [dbo].[ChatbotQueries] ON 



INSERT [dbo].[ChatbotQueries] ([QueryId], [UserId], [Message], [Response], [CreatedAt], [ApplicationUserId]) VALUES (1, N'User1', N'How can I submit my custom cosplay gallery?', N'You can go to the Fan Submissions section from your user dashboard and upload your cosplay photos with a title and description.', CAST(N'2026-09-24T10:30:00.0000000' AS DateTime2), NULL)

INSERT [dbo].[ChatbotQueries] ([QueryId], [UserId], [Message], [Response], [CreatedAt], [ApplicationUserId]) VALUES (2, N'User2', N'Where can I find the upcoming anime event schedule?', N'Check out the Events page in the navigation bar to see all upcoming conventions, tournaments, and live streams.', CAST(N'2026-09-24T12:45:00.0000000' AS DateTime2), NULL)

INSERT [dbo].[ChatbotQueries] ([QueryId], [UserId], [Message], [Response], [CreatedAt], [ApplicationUserId]) VALUES (3, N'User3', N'Are merchandise 
<truncated 70564 bytes>

NOTE: The output was truncated because it was too long. Use a more targeted query or a smaller range to get the information you need.
