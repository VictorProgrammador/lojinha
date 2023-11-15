create table Category(

Id INT PRIMARY KEY IDENTITY(1,1),
Name nvarchar(max),
IsDeleted BIT DEFAULT 0

);

alter table Category
add IsDigitalShop bit default 0

--INSERT INTO Category VALUES ('Estética', 0)
--INSERT INTO Category VALUES ('Tecnologia', 0)
--INSERT INTO Category VALUES ('Motorista', 0)


alter table Category 
add Icon nvarchar(max)