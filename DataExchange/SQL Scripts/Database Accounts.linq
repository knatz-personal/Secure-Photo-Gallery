<Query Kind="SQL" />

print 'DROP USERS AND LOGINS'
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = 'Administrator')
    DROP LOGIN Administrator
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = 'Administrator')
    DROP USER Administrator
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = 'Customer')
    DROP LOGIN Customer
IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = 'Customer')
    DROP USER Customer

print 'CREATE USER AND LOGIN'
-- Creates the login 
CREATE LOGIN Administrator 
    WITH PASSWORD = 'u66W0N52mwV8a';

-- Creates a database user for the login
CREATE USER Administrator FOR LOGIN Administrator;
  go
  
  -- Creates the login 
CREATE LOGIN Customer 
    WITH PASSWORD = 'u30aeiNOCD9o';

-- Creates a database user for the login
CREATE USER Customer FOR LOGIN Customer;
go

print 'ASSIGN TABLE PERMISSIONS'
SELECT 'GRANT SELECT ON "' + TABLE_SCHEMA + '"."' + TABLE_NAME + '" TO "Administrator"' 
FROM information_schema.tables

SELECT 'GRANT ALTER ON "' + TABLE_SCHEMA + '"."' + TABLE_NAME + '" TO "Administrator"' 
FROM information_schema.tables

SELECT 'GRANT UPDATE ON "' + TABLE_SCHEMA + '"."' + TABLE_NAME + '" TO "Administrator"' 
FROM information_schema.tables

SELECT 'GRANT INSERT ON "' + TABLE_SCHEMA + '"."' + TABLE_NAME + '" TO "Administrator"' 
FROM information_schema.tables

SELECT 'GRANT DELETE ON "' + TABLE_SCHEMA + '"."' + TABLE_NAME + '" TO "Administrator"' 
FROM information_schema.tables