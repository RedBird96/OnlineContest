USE [master]
GO
/****** Object:  Database [JudgementApp]    Script Date: 25/07/2021 6:44:05 pm ******/
CREATE DATABASE [JudgementApp] 
GO


USE [JudgementApp]
GO
/****** Object:  Table [dbo].[CreateProblem]    Script Date: 25/07/2021 6:44:06 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CreateProblem](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[QuestionNo] [int] NULL,
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
/****** Object:  Table [dbo].[Judgement]    Script Date: 25/07/2021 6:44:06 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Judgement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
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
/****** Object:  Table [dbo].[Questions]    Script Date: 25/07/2021 6:44:06 pm ******/
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
 CONSTRAINT [PK_Questions] PRIMARY KEY CLUSTERED 
(
	[PKQuestion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Results]    Script Date: 25/07/2021 6:44:06 pm ******/
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
SET IDENTITY_INSERT [dbo].[CreateProblem] ON 
GO
INSERT [dbo].[CreateProblem] ([ID], [QuestionNo], [P1], [P2], [P3], [P4]) VALUES (1, 1, N'TSLA', N'1122', N'01:00 pm', N'')
GO
INSERT [dbo].[CreateProblem] ([ID], [QuestionNo], [P1], [P2], [P3], [P4]) VALUES (2, 2, N'GOOGL', N'below', N'333', N'today')
GO
INSERT [dbo].[CreateProblem] ([ID], [QuestionNo], [P1], [P2], [P3], [P4]) VALUES (3, 3, N'NFLX', N'', N'', N'')
GO
INSERT [dbo].[CreateProblem] ([ID], [QuestionNo], [P1], [P2], [P3], [P4]) VALUES (4, 4, N'NVDA', N'12:00 noon', N'', N'')
GO
SET IDENTITY_INSERT [dbo].[CreateProblem] OFF
GO
SET IDENTITY_INSERT [dbo].[Judgement] ON 
GO
INSERT [dbo].[Judgement] ([Id], [Name], [ProblemNo], [TotalCorrect], [Date], [Q1], [T1], [P1], [Q2], [T2], [P2], [Q3], [T3], [P3], [Q4], [T4], [P4]) VALUES (1, N'Abdul', NULL, 3, CAST(N'2021-07-19' AS Date), N'Fact', NULL, NULL, N'Fact', NULL, NULL, N'PM', NULL, NULL, N'Before', NULL, NULL)
GO
INSERT [dbo].[Judgement] ([Id], [Name], [ProblemNo], [TotalCorrect], [Date], [Q1], [T1], [P1], [Q2], [T2], [P2], [Q3], [T3], [P3], [Q4], [T4], [P4]) VALUES (2, N'John', NULL, 2, CAST(N'2021-07-19' AS Date), N'Fact', NULL, NULL, N'Fact', NULL, NULL, N'AM', NULL, NULL, N'Before', NULL, NULL)
GO
INSERT [dbo].[Judgement] ([Id], [Name], [ProblemNo], [TotalCorrect], [Date], [Q1], [T1], [P1], [Q2], [T2], [P2], [Q3], [T3], [P3], [Q4], [T4], [P4]) VALUES (3, N'Mike', NULL, 1, CAST(N'2021-07-19' AS Date), N'Fact', NULL, NULL, N'Fiction', NULL, NULL, N'PM', NULL, NULL, N'After', NULL, NULL)
GO
INSERT [dbo].[Judgement] ([Id], [Name], [ProblemNo], [TotalCorrect], [Date], [Q1], [T1], [P1], [Q2], [T2], [P2], [Q3], [T3], [P3], [Q4], [T4], [P4]) VALUES (4, N'Bruce', NULL, 0, CAST(N'2021-07-19' AS Date), N'', NULL, NULL, N'', NULL, NULL, N'', NULL, NULL, N'', NULL, NULL)
GO
INSERT [dbo].[Judgement] ([Id], [Name], [ProblemNo], [TotalCorrect], [Date], [Q1], [T1], [P1], [Q2], [T2], [P2], [Q3], [T3], [P3], [Q4], [T4], [P4]) VALUES (5, N'Duck', NULL, 2, CAST(N'2021-07-19' AS Date), N'Fact', NULL, NULL, N'Fiction', NULL, NULL, N'PM', NULL, NULL, N'Before', NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Judgement] OFF
GO
SET IDENTITY_INSERT [dbo].[Questions] ON 
GO
INSERT [dbo].[Questions] ([PKQuestion], [Type], [Title], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (1, 1, N'will [P1] hit [P2] by [P3]?', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Questions] ([PKQuestion], [Type], [Title], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (2, 2, N'will [P1] close [P2] [P3] [P4]?', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Questions] ([PKQuestion], [Type], [Title], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (3, 3, N'will [P1] have a higher return in the AM or PM?', NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Questions] ([PKQuestion], [Type], [Title], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) VALUES (4, 4, N'will [P1] have a higher return before or after [P2]?', NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Questions] OFF
GO
/****** Object:  StoredProcedure [dbo].[prc_AddJudgmentColumn]    Script Date: 25/07/2021 6:44:06 pm ******/
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
/****** Object:  StoredProcedure [dbo].[prc_AddQuestion]    Script Date: 25/07/2021 6:44:06 pm ******/
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
	@type as int
AS
BEGIN
	Insert into Questions(title,type) values(@Title,@type)
	declare @id as int=(select @@IDENTITY)
	INSERT INTO [dbo].[CreateProblem]
           ([QuestionNo]
           ,[P1]
           ,[P2]
           ,[P3]
           ,[P4])
   Select top 1 @id
           ,[P1]
           ,[P2]
           ,[P3]
           ,[P4] from Questions as Q 
		   Left join  [dbo].[CreateProblem] C on C.QuestionNo=Q.PKQuestion
		   where Q.[Type]=@type
	print cast(@id as varchar(50))
	exec prc_AddJudgmentColumn @id
END
GO
/****** Object:  StoredProcedure [dbo].[prc_GetProblem]    Script Date: 25/07/2021 6:44:06 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[prc_GetProblem]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	
	Select Q.Title,Q.[Type],ROW_NUMBER() OVER(ORDER BY [Type]) as Row_Num,  PKQuestion as ID
	,C.P1,C.P2,C.P3,C.P4
	from Questions as Q
	Left Join CreateProblem as C on C.QuestionNo=Q.PKQuestion
	Order By [Type],Row_Num
END
GO
USE [master]
GO
ALTER DATABASE [JudgementApp] SET  READ_WRITE 
GO
