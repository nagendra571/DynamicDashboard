

/****** Object:  Table [dbo].[Templates]    Script Date: 8/13/2021 11:58:34 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Templates]') AND type in (N'U'))
DROP TABLE [dbo].[Templates]
GO
/****** Object:  Table [dbo].[Elements]    Script Date: 8/13/2021 11:58:34 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Elements]') AND type in (N'U'))
DROP TABLE [dbo].[Elements]
GO
/****** Object:  Table [dbo].[DashboardsInfo]    Script Date: 8/13/2021 11:58:34 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DashboardsInfo]') AND type in (N'U'))
DROP TABLE [dbo].[DashboardsInfo]
GO
/****** Object:  Table [dbo].[DashboardLinkedElements]    Script Date: 8/13/2021 11:58:34 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DashboardLinkedElements]') AND type in (N'U'))
DROP TABLE [dbo].[DashboardLinkedElements]
GO
/****** Object:  Table [dbo].[DashboardLinkedElements]    Script Date: 8/13/2021 11:58:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DashboardLinkedElements](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DashboardId] [int] NOT NULL,
	[ElementId] [int] NOT NULL,
	[Placement] [varchar](55) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DashboardsInfo]    Script Date: 8/13/2021 11:58:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DashboardsInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[TemplateId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Elements]    Script Date: 8/13/2021 11:58:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Elements](
	[Id] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Templates]    Script Date: 8/13/2021 11:58:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Templates](
	[Id] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[ElementsCount] [int] NOT NULL
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[DashboardLinkedElements] ON 
GO
INSERT [dbo].[DashboardLinkedElements] ([Id], [DashboardId], [ElementId], [Placement]) VALUES (5, 2, 2, N'2')
GO
INSERT [dbo].[DashboardLinkedElements] ([Id], [DashboardId], [ElementId], [Placement]) VALUES (6, 2, 3, N'1')
GO
INSERT [dbo].[DashboardLinkedElements] ([Id], [DashboardId], [ElementId], [Placement]) VALUES (7, 1, 3, N'2')
GO
INSERT [dbo].[DashboardLinkedElements] ([Id], [DashboardId], [ElementId], [Placement]) VALUES (9, 1, 4, N'1')
GO
SET IDENTITY_INSERT [dbo].[DashboardLinkedElements] OFF
GO
SET IDENTITY_INSERT [dbo].[DashboardsInfo] ON 
GO
INSERT [dbo].[DashboardsInfo] ([Id], [Name], [TemplateId]) VALUES (1, N'First Dashboard', 3)
GO
INSERT [dbo].[DashboardsInfo] ([Id], [Name], [TemplateId]) VALUES (2, N'Second Dashboard', 2)
GO
SET IDENTITY_INSERT [dbo].[DashboardsInfo] OFF
GO
INSERT [dbo].[Elements] ([Id], [Name]) VALUES (1, N'Element1')
GO
INSERT [dbo].[Elements] ([Id], [Name]) VALUES (2, N'Element2')
GO
INSERT [dbo].[Elements] ([Id], [Name]) VALUES (3, N'Element3')
GO
INSERT [dbo].[Elements] ([Id], [Name]) VALUES (4, N'Element4')
GO
INSERT [dbo].[Elements] ([Id], [Name]) VALUES (5, N'Element5')
GO
INSERT [dbo].[Templates] ([Id], [Name], [ElementsCount]) VALUES (1, N'Template1', 1)
GO
INSERT [dbo].[Templates] ([Id], [Name], [ElementsCount]) VALUES (2, N'Template2', 2)
GO
INSERT [dbo].[Templates] ([Id], [Name], [ElementsCount]) VALUES (3, N'Template3', 2)
GO
INSERT [dbo].[Templates] ([Id], [Name], [ElementsCount]) VALUES (4, N'Template4', 3)
GO
INSERT [dbo].[Templates] ([Id], [Name], [ElementsCount]) VALUES (5, N'Template5', 3)
GO
INSERT [dbo].[Templates] ([Id], [Name], [ElementsCount]) VALUES (6, N'Template6', 4)
GO
INSERT [dbo].[Templates] ([Id], [Name], [ElementsCount]) VALUES (7, N'Template7', 4)
GO
INSERT [dbo].[Templates] ([Id], [Name], [ElementsCount]) VALUES (8, N'Template8', 5)
GO
