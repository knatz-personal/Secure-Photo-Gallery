<Query Kind="SQL">
  <Connection>
    <ID>d30dde50-1172-4d49-aeb0-33a70c36f40f</ID>
    <Persist>true</Persist>
    <Server>AS-KNATZ-US</Server>
    <Database>SSDMalta</Database>
    <DisplayName>SSDDatabase</DisplayName>
  </Connection>
</Query>

-- =============================================
-- Author:		Nathan Zwelibanzi Khupe
-- Create date: 22/02/2017
-- Description:	SSD Project
-- =============================================

print '*********----START!----*********'

print 'DROPPING EXISTING FOREIGN KEYS'
if exists (select * from sysobjects where name = 'AspNetUsers') 
    ALTER TABLE AspNetUsers drop constraint  FK_Users_Genders
if exists (select * from sysobjects where name = 'AspNetUsers') 
    ALTER TABLE AspNetUsers drop CONSTRAINT FK_Users_Towns
if exists (select * from sysobjects where name = 'Menus') 
    ALTER TABLE Menus drop CONSTRAINT FK_Menus_Menus
if exists (select * from sysobjects where name = 'MenuRoles') 
    ALTER TABLE MenuRoles drop CONSTRAINT FK_MenuRoles_Roles
if exists (select * from sysobjects where name = 'MenuRoles')
    ALTER TABLE MenuRoles drop CONSTRAINT FK_MenuRoles_Menus
if exists (select * from sysobjects where name = 'Albums') 
	ALTER TABLE Albums drop CONSTRAINT FK_AspNetUser_Albums
if exists (select * from sysobjects where name = 'Images') 
	ALTER TABLE AlbumImages drop CONSTRAINT FK_Album_Images

print 'Drop create tables'


if exists (select * from sysobjects where name = 'EventLog') 
	drop table EventLog
CREATE TABLE EventLog(
    ID                int IDENTITY(1,1) NOT NULL primary key,
    Message           nvarchar(max)  NULL,
	SourceTable       nvarchar(100) not null,
	ModifiedBy        nvarchar(100) not null,
	DateModified      datetime DEFAULT GetDate()
)

if exists (select * from sysobjects where name = 'ErrorLog') 
	drop table ErrorLog
CREATE TABLE ErrorLog(
    ID                int IDENTITY(1,1) NOT NULL primary key,
	Username          nvarchar(50) null,	
    Message           nvarchar(max)  NULL,
	InnerException    nvarchar(max) not null,
	DateTriggered      datetime DEFAULT GetDate()
)
 
if exists (select * from sysobjects where name = 'Genders') 
	drop table Genders
CREATE TABLE  Genders (
    ID int IDENTITY(1,1) NOT NULL primary key,
    Name nvarchar(6)  NOT NULL
) 

if exists (select * from sysobjects where name = 'Towns') 
	drop table Towns
CREATE TABLE  Towns (
    ID int IDENTITY(1,1) NOT NULL primary key,
    Name nvarchar(50)  NOT NULL
)


if exists (select * from sysobjects where name = 'AspNetUsers') 
    drop table AspNetUsers
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	Mobile nvarchar(50) NULL,
	PrivateKey nvarchar(max), 
	PublicKey nvarchar(max),
	[PhoneNumber] [nvarchar](max) NULL,
	GenderID int  NULL CONSTRAINT FK_Users_Genders FOREIGN KEY references 
	Genders (ID),
	[PhoneNumberConfirmed] [bit] NOT NULL,
	TownID  int  NULL CONSTRAINT FK_Users_Towns FOREIGN KEY references 
	Towns (ID), 
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	Name nvarchar(50) NOT NULL,
	Surname nvarchar(50) NOT NULL,
	Address [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


if exists (select * from sysobjects where name = 'AspNetUserClaims') 
	drop table AspNetUserClaims
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO


if exists (select * from sysobjects where name = 'AspNetUserLogins') 
	drop table AspNetUserLogins
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO

if exists (select * from sysobjects where name = 'AspNetRoles') 
	drop table AspNetRoles
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	Discriminator nvarchar(max) NOT NULL, 
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

if exists (select * from sysobjects where name = 'Menus') 
	drop table Menus
CREATE TABLE  Menus (
    ID int NOT NULL identity(1,1) primary key,
    Name nvarchar(50)  NOT NULL,
	SortOrder int null,
    ParentID int  NULL CONSTRAINT FK_Menus_Menus FOREIGN KEY references Menus (ID),  
    Description nvarchar(500)  NULL,
	ActionName nvarchar(50) null,
	ControllerName nvarchar(50) null,
	Url nvarchar(500) null
)


if exists (select * from sysobjects where name = 'AspNetUserRoles') 
	drop table AspNetUserRoles
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY ([UserId] ASC,	[RoleId] ASC)
 WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO

ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO

if exists (select * from sysobjects where name = 'MenuRoles') 
	drop table MenuRoles
CREATE TABLE MenuRoles (
    MenuID int NOT NULL CONSTRAINT FK_MenuRoles_Menus FOREIGN KEY references Menus (ID),
    RoleId nvarchar(128) NOT NULL CONSTRAINT FK_MenuRoles_Roles FOREIGN KEY references AspNetRoles (Id),
    CONSTRAINT PK_Menu_Roles PRIMARY KEY (MenuID, RoleId)
)

if exists (select * from sysobjects where name = 'Albums') 
	drop table Albums
CREATE TABLE Albums (
  ID int NOT NULL identity(1,1) primary key,
  Title nvarchar(50)  NOT NULL,
  Description nvarchar(500)  NULL,
  CreationDate datetime NOT NULL,
  ModifiedOn datetime NOT NULL,
  UserId nvarchar(128) NOT NULL CONSTRAINT FK_AspNetUser_Albums FOREIGN KEY references AspNetUsers (Id) ON DELETE CASCADE
)

if exists (select * from sysobjects where name = 'Images') 
	drop table Images
CREATE TABLE Images (
  ID int NOT NULL identity(1,1) primary key,
  Title nvarchar(50)  NOT NULL,
  Description nvarchar(500)  NULL,
  Path nvarchar(500)  NULL,
  ThumbNail nvarchar(500)  NOT NULL,
  CreationDate datetime NOT NULL,
  ModifiedOn datetime NOT NULL,
  ESecretKey nvarchar(max) NOT NULL,
  EncryptedIV nvarchar(max) NOT NULL,
  Signature nvarchar(max) NOT NULL,
  AlbumId int Not NULL CONSTRAINT FK_Album_Images FOREIGN KEY references Albums (ID) ON DELETE CASCADE
)

GO
if exists (select * from sysobjects where name = '__MigrationHistory') 
	drop table __MigrationHistory
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
print '*********----DONE!-----*********'