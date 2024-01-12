CREATE DATABASE SampleDotnetCleanArchitecture
GO

USE SampleDotnetCleanArchitecture
GO

CREATE TABLE dbo.Client(
	Id 					BIGINT 			NOT NULL IDENTITY(1,1),
    FirstName 		 	VARCHAR(100) 	NOT NULL,
    LastName  		 	VARCHAR(100) 	NOT NULL,
    BirthDate 		 	DATETIME 		NOT NULL,
    CreationDate 	 	DATETIME 		NULL DEFAULT GETDATE(),
	CreatedBy		 	VARCHAR(100) 	NULL,
    LastModifiedDate 	DATETIME	 	NULL,
    LastModifiedBy		VARCHAR(100) 	NULL,
	CONSTRAINT PK_Client PRIMARY KEY (Id)
)
GO

CREATE TABLE dbo.Account(
    UserName 		 	VARCHAR(100) 	NOT NULL,
	PasswordHash  		VARCHAR(1000) 	NOT NULL,
    CreationDate 	 	DATETIME 		NULL DEFAULT GETDATE(),
	CreatedBy		 	VARCHAR(100) 	NULL,
    LastModifiedDate 	DATETIME	 	NULL,
    LastModifiedBy		VARCHAR(100) 	NULL,
	CONSTRAINT PK_Account PRIMARY KEY (UserName)
)
GO

CREATE TABLE dbo.AccountRoles(
    RoleName 		 VARCHAR(100) 	NOT NULL,
	Account_UserName VARCHAR(100) 	NOT NULL,
)
GO

CREATE TABLE dbo.AccountClaims(
    ClaimName 		    VARCHAR(100) 	NOT NULL,
	Account_UserName  	VARCHAR(1000) 	NOT NULL,
)
GO