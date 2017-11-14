<Query Kind="SQL">
  <Connection>
    <ID>d30dde50-1172-4d49-aeb0-33a70c36f40f</ID>
    <Persist>true</Persist>
    <Server>AS-KNATZ-US</Server>
    <Database>SSDMalta</Database>
    <DisplayName>SSDDatabase</DisplayName>
  </Connection>
</Query>

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
	
DROP TABLE [dbo].[AspNetUserClaims]
GO
DROP TABLE [dbo].[AspNetUserLogins]
GO
DROP TABLE [dbo].[AspNetUserRoles]
GO
DROP TABLE [dbo].[AspNetUsers]
GO
DROP TABLE [dbo].[ErrorLog]
GO
DROP TABLE [dbo].[EventLog]
GO
DROP TABLE [dbo].[Genders]
GO
DROP TABLE [dbo].[Images]
GO
DROP TABLE [dbo].[MenuRoles]
GO
DROP TABLE [dbo].[Menus]
GO
DROP TABLE [dbo].[Towns]
GO
DROP TABLE [dbo].[Albums]
GO
DROP TABLE [dbo].[AspNetRoles]
GO
DROP TABLE [dbo].[__MigrationHistory]
GO