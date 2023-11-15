--create database FashionProject

--use FashionProject	


-- no visual studio, precisa rodar um: Update-Database para poder criar as entidades de Identity das migrations.


--create table Person (

--Id INT PRIMARY KEY IDENTITY(1,1),
--Name NVARCHAR(255),
--Age INT,
--BirthDate DATETIME


--);



--CREATE TABLE UserInfo (

--Id INT PRIMARY KEY IDENTITY(1,1),
--PersonId INT,
--AspNetUserId UNIQUEIDENTIFIER,
--UniqueId UNIQUEIDENTIFIER,

--FOREIGN KEY (PersonId) REFERENCES Person(Id)

--);


--alter table UserInfo
--add Password nvarchar(max)