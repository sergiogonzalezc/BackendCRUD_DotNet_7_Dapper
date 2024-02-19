USE [BD_Team]
GO
/**************************** CREATION TABLES ******************************/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Member]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Member](
	[id] int IDENTITY(1,1) NOT NULL,
	[name]  varchar(50) NOT NULL,
	[salary_per_year] int NOT NULL,
	[type] varchar(1) NOT NULL,
	[role] int NULL,
	[country] varchar(40) NULL,
	[currencie_name] varchar(40) NULL,
 CONSTRAINT [PK_Member] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MemberType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MemberType](
	[id] varchar(1) NOT NULL,
	[description]  [varchar](50) NOT NULL,
 CONSTRAINT [PK_MemberType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RoleType]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RoleType](
	[id] int NOT NULL,
	[description]  [varchar](50) NOT NULL,
 CONSTRAINT [PK_RoleType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tag]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Tag](
	[id] int IDENTITY(1,1) NOT NULL,
	[label]  [varchar](50) NOT NULL,
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MemberTag]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MemberTag](
	[member_id] int NOT NULL,
	[tag_id] int NOT NULL,	
 CONSTRAINT [PK_MemberTag] PRIMARY KEY CLUSTERED 
(
	[member_id] ASC,[tag_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END

ALTER TABLE [dbo].[Member] WITH CHECK ADD CONSTRAINT [FK_Member_TypeId] FOREIGN KEY([type]) 
REFERENCES [dbo].[MemberType] ([id])

ALTER TABLE [dbo].[Member] CHECK CONSTRAINT [FK_Member_TypeId]

ALTER TABLE [dbo].[Member] WITH CHECK ADD CONSTRAINT [FK_Member_RoleId] FOREIGN KEY([role]) 
REFERENCES [dbo].[RoleType] ([id])

ALTER TABLE [dbo].[Member] CHECK CONSTRAINT [FK_Member_RoleId]

ALTER TABLE [dbo].[MemberTag] WITH CHECK ADD CONSTRAINT [FK_MemberTag_member_id] FOREIGN KEY([member_id]) 
REFERENCES [dbo].[Member] ([id])

ALTER TABLE [dbo].[MemberTag] CHECK CONSTRAINT [FK_MemberTag_member_id]

ALTER TABLE [dbo].[MemberTag] WITH CHECK ADD CONSTRAINT [FK_MemberTag_tag_id] FOREIGN KEY([tag_id]) 
REFERENCES [dbo].[Tag] ([id])

ALTER TABLE [dbo].[MemberTag] CHECK CONSTRAINT [FK_MemberTag_tag_id]
GO