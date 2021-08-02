USE [master]
GO
/****** Object:  Database [JudgementApp]    Script Date: 02/08/2021 9:09:28 pm ******/
CREATE DATABASE [JudgementApp] 
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [JudgementApp].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [JudgementApp] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [JudgementApp] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [JudgementApp] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [JudgementApp] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [JudgementApp] SET ARITHABORT OFF 
GO
ALTER DATABASE [JudgementApp] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [JudgementApp] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [JudgementApp] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [JudgementApp] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [JudgementApp] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [JudgementApp] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [JudgementApp] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [JudgementApp] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [JudgementApp] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [JudgementApp] SET  DISABLE_BROKER 
GO
ALTER DATABASE [JudgementApp] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [JudgementApp] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [JudgementApp] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [JudgementApp] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [JudgementApp] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [JudgementApp] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [JudgementApp] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [JudgementApp] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [JudgementApp] SET  MULTI_USER 
GO
ALTER DATABASE [JudgementApp] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [JudgementApp] SET DB_CHAINING OFF 
GO
EXEC sys.sp_db_vardecimal_storage_format N'JudgementApp', N'ON'
GO
USE [JudgementApp]
GO
/****** Object:  Table [dbo].[Company]    Script Date: 02/08/2021 9:09:28 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Company](
	[PKCompany] [bigint] IDENTITY(1,1) NOT NULL,
	[CompanyName] [nvarchar](2000) NULL,
	[Id] [varchar](2000) NULL,
	[EmailAddress] [varchar](200) NULL,
	[Password] [varchar](200) NULL,
	[Logo] [varchar](500) NULL,
	[BackgroundColor] [varchar](200) NULL,
	[CardBackgroundColor] [varchar](200) NULL,
	[HeadingColor] [varchar](200) NULL,
	[TableHeaderColor] [varchar](200) NULL,
	[TextColor] [varchar](200) NULL,
	[LinkColor] [varchar](200) NULL,
	[IsActive] [bit] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
(
	[PKCompany] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CreateProblem]    Script Date: 02/08/2021 9:09:28 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CreateProblem](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FKCompany] [bigint] NULL,
	[ProblemName] [varchar](500) NULL,
	[QuestionNo] [bigint] NULL,
	[P1] [varchar](50) NULL,
	[P2] [varchar](50) NULL,
	[P3] [varchar](50) NULL,
	[P4] [varchar](50) NULL,
 CONSTRAINT [PK_CreateProblem] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Judgement]    Script Date: 02/08/2021 9:09:28 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Judgement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FKCompany] [bigint] NULL,
	[Name] [varchar](50) NULL,
	[UserEmail] [varchar](500) NULL,
	[ProblemName] [varchar](500) NULL,
	[ProblemNo] [int] NULL,
	[TotalCorrect] [int] NULL,
	[Date] [date] NULL,
	[Q1] [varchar](50) NULL,
	[T1] [int] NULL,
	[P1] [varchar](500) NULL,
	[Q2] [varchar](50) NULL,
	[T2] [int] NULL,
	[P2] [varchar](500) NULL,
	[Q3] [varchar](50) NULL,
	[T3] [int] NULL,
	[P3] [varchar](500) NULL,
	[Q4] [varchar](50) NULL,
	[T4] [int] NULL,
	[P4] [varchar](500) NULL,
 CONSTRAINT [PK_Judgement] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Questions]    Script Date: 02/08/2021 9:09:28 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Questions](
	[PKQuestion] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [int] NULL,
	[Title] [nvarchar](4000) NULL,
	[CreatedBy] [bigint] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [bigint] NULL,
	[ModifiedDate] [datetime] NULL,
	[FKCompany] [bigint] NULL,
	[ProblemName] [varchar](500) NULL,
 CONSTRAINT [PK_Questions] PRIMARY KEY CLUSTERED 
(
	[PKQuestion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Results]    Script Date: 02/08/2021 9:09:28 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Results](
	[Id] [int] NULL,
	[QuestionNo] [int] NULL,
	[Result] [varchar](50) NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Company] ON 
GO
INSERT [dbo].[Company] ([PKCompany], [CompanyName], [Id], [EmailAddress], [Password], [Logo], [BackgroundColor], [CardBackgroundColor], [HeadingColor], [TableHeaderColor], [TextColor], [LinkColor], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, N'Test Comp', N'STgyWDVleGpLNDIyRThIK3FaZEFKZz090', NULL, NULL, N'/assets/img/company/1/avatar3.jpeg', N'#bfbc', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Company] ([PKCompany], [CompanyName], [Id], [EmailAddress], [Password], [Logo], [BackgroundColor], [CardBackgroundColor], [HeadingColor], [TableHeaderColor], [TextColor], [LinkColor], [IsActive], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2, N'Test 2', N'ZUdmVkNyUFNqM2d1ZUl4MGFUYjBnZz090', NULL, NULL, N'/assets/img/company/2/avatar3.jpeg', N'#bfbc', NULL, NULL, NULL, NULL, NULL, 1, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Company] OFF
GO
SET IDENTITY_INSERT [dbo].[CreateProblem] ON 
GO
INSERT [dbo].[CreateProblem] ([ID], [FKCompany], [ProblemName], [QuestionNo], [P1], [P2], [P3], [P4]) VALUES (1, NULL, NULL, 1, N'TLT', N'1212', N'11:30 am', N'')
GO
INSERT [dbo].[CreateProblem] ([ID], [FKCompany], [ProblemName], [QuestionNo], [P1], [P2], [P3], [P4]) VALUES (2, NULL, NULL, 2, N'NFLX', N'above', N'', N'this week')
GO
INSERT [dbo].[CreateProblem] ([ID], [FKCompany], [ProblemName], [QuestionNo], [P1], [P2], [P3], [P4]) VALUES (3, NULL, NULL, 3, N'AMZN', N'', N'', N'')
GO
INSERT [dbo].[CreateProblem] ([ID], [FKCompany], [ProblemName], [QuestionNo], [P1], [P2], [P3], [P4]) VALUES (4, NULL, N'abc', 4, N'NFLX', N'02:30 pm', N'', N'')
GO
SET IDENTITY_INSERT [dbo].[CreateProblem] OFF
GO
SET IDENTITY_INSERT [dbo].[Judgement] ON 
GO
INSERT [dbo].[Judgement] ([Id], [FKCompany], [Name], [UserEmail], [ProblemName], [ProblemNo], [TotalCorrect], [Date], [Q1], [T1], [P1], [Q2], [T2], [P2], [Q3], [T3], [P3], [Q4], [T4], [P4]) VALUES (1, 1, N'Abdul', NULL, N'Test const', 4, 3, CAST(N'2021-07-19' AS Date), N'Fact', NULL, NULL, N'Fact', NULL, NULL, N'PM', NULL, NULL, N'Before', NULL, NULL)
GO
INSERT [dbo].[Judgement] ([Id], [FKCompany], [Name], [UserEmail], [ProblemName], [ProblemNo], [TotalCorrect], [Date], [Q1], [T1], [P1], [Q2], [T2], [P2], [Q3], [T3], [P3], [Q4], [T4], [P4]) VALUES (2, 1, N'John', NULL, N'Test const', 4, 2, CAST(N'2021-07-19' AS Date), N'Fact', NULL, NULL, N'Fact', NULL, NULL, N'AM', NULL, NULL, N'Before', NULL, NULL)
GO
INSERT [dbo].[Judgement] ([Id], [FKCompany], [Name], [UserEmail], [ProblemName], [ProblemNo], [TotalCorrect], [Date], [Q1], [T1], [P1], [Q2], [T2], [P2], [Q3], [T3], [P3], [Q4], [T4], [P4]) VALUES (3, 1, N'Mike', NULL, N'Test const', 4, 1, CAST(N'2021-07-19' AS Date), N'Fact', NULL, NULL, N'Fiction', NULL, NULL, N'PM', NULL, NULL, N'After', NULL, NULL)
GO
INSERT [dbo].[Judgement] ([Id], [FKCompany], [Name], [UserEmail], [ProblemName], [ProblemNo], [TotalCorrect], [Date], [Q1], [T1], [P1], [Q2], [T2], [P2], [Q3], [T3], [P3], [Q4], [T4], [P4]) VALUES (4, 1, N'Bruce', NULL, N'Test const', 4, 2, CAST(N'2021-07-19' AS Date), N'', NULL, NULL, N'', NULL, NULL, N'', NULL, NULL, N'', NULL, NULL)
GO
INSERT [dbo].[Judgement] ([Id], [FKCompany], [Name], [UserEmail], [ProblemName], [ProblemNo], [TotalCorrect], [Date], [Q1], [T1], [P1], [Q2], [T2], [P2], [Q3], [T3], [P3], [Q4], [T4], [P4]) VALUES (5, 1, N'Duck', NULL, N'Test const', 4, 2, CAST(N'2021-07-19' AS Date), N'Fact', NULL, NULL, N'Fiction', NULL, NULL, N'PM', NULL, NULL, N'Before', NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Judgement] OFF
GO
SET IDENTITY_INSERT [dbo].[Questions] ON 
GO
INSERT [dbo].[Questions] ([PKQuestion], [Type], [Title], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [FKCompany], [ProblemName]) VALUES (1, 1, N'will [P1] hit [P2] by [P3]?', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Questions] ([PKQuestion], [Type], [Title], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [FKCompany], [ProblemName]) VALUES (2, 2, N'will [P1] close [P2] [P3] [P4]?', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Questions] ([PKQuestion], [Type], [Title], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [FKCompany], [ProblemName]) VALUES (3, 3, N'will [P1] have a higher return in the AM or PM?', NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Questions] ([PKQuestion], [Type], [Title], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [FKCompany], [ProblemName]) VALUES (4, 4, N'will [P1] have a higher return before or after [P2]?', NULL, NULL, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Questions] OFF
GO
/****** Object:  StoredProcedure [dbo].[prc_AddJudgmentColumn]    Script Date: 02/08/2021 9:09:28 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- ============================================= prc_AddJudgmentColumn 5
CREATE PROCEDURE [dbo].[prc_AddJudgmentColumn]
	-- Add the parameters for the stored procedure here
	@QuestionID as varchar(200)

AS
BEGIN
	IF COL_LENGTH('dbo.Judgement', 'Q'+@QuestionID) IS  NULL
BEGIN
   declare @str as nvarchar(max)='ALTER TABLE [dbo].[Judgement] ADD [Q'+@QuestionID+'] varchar(500)'
  exec sp_executesql @str
  set  @str ='ALTER TABLE [dbo].[Judgement] ADD [P'+@QuestionID+'] varchar(500)'
    exec sp_executesql @str
	set  @str ='ALTER TABLE [dbo].[Judgement] ADD [T'+@QuestionID+'] int'
    exec sp_executesql @str
END
END
GO
/****** Object:  StoredProcedure [dbo].[prc_AddQuestion]    Script Date: 02/08/2021 9:09:28 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- ============================================= prc_AddQuestion 'abc',1  prc_AddJudgmentColumn 10014
CREATE PROCEDURE [dbo].[prc_AddQuestion]
	-- Add the parameters for the stored procedure here
	@Title as nvarchar(2000),
	@type as int,
	@FKCompany as bigint,
	@ProblemName as nvarchar(2000)
AS
BEGIN
	Insert into Questions(title,type,FKCompany,ProblemName) values(@Title,@type,@FKCompany,@ProblemName)
	declare @id as int=(select @@IDENTITY)
	INSERT INTO [dbo].[CreateProblem]
           ([QuestionNo]
           ,[P1]
           ,[P2]
           ,[P3]
           ,[P4]
		   ,FKCompany,ProblemName
		   )
   Select top 1 @id
           ,[P1]
           ,[P2]
           ,[P3]
           ,[P4],@FKCompany,@ProblemName from Questions as Q 
		   Left join  [dbo].[CreateProblem] C on C.QuestionNo=Q.PKQuestion and C.FKCompany=@FKCompany
		   where Q.[Type]=@type
	print cast(@id as varchar(50))
	exec prc_AddJudgmentColumn @id
END
GO
/****** Object:  StoredProcedure [dbo].[prc_GetCompany]    Script Date: 02/08/2021 9:09:28 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetCompany]
	@id as bigint
AS
BEGIN
declare @ProblemName as varchar(500)='';
Select @ProblemName=ProblemName from CreateProblem where FKCompany=@id 
Select *,@ProblemName as ProblemName from Company where IsActive=1 and PKCompany=@id
END
GO
/****** Object:  StoredProcedure [dbo].[prc_GetProblem]    Script Date: 02/08/2021 9:09:28 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- ============================================= [dbo].[prc_GetProblem] 1
CREATE PROCEDURE [dbo].[prc_GetProblem]
	-- Add the parameters for the stored procedure here
	@FKCompany as bigint,
	@ProblemName as varchar(500)=null
AS
BEGIN
	if (len(ltrim(rtrim(@ProblemName)))<1 or @ProblemName='')
	begin
	set @ProblemName=null;
	end

	Select Q.Title,Q.[Type],ROW_NUMBER() OVER(ORDER BY [Type]) as Row_Num,C.ProblemName,  PKQuestion as ID
	,C.P1,C.P2,C.P3,C.P4
	from Questions as Q
	Left Join CreateProblem as C on C.QuestionNo=Q.PKQuestion and C.FKCompany=@FKCompany and (@ProblemName is null or C.ProblemName=@ProblemName)
	Where C.FKCompany=@FKCompany or Q.PKQuestion in (1,2,3,4)
	Order By [Type],Row_Num
END
GO
USE [master]
GO
ALTER DATABASE [JudgementApp] SET  READ_WRITE 
GO
