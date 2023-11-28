
CREATE TABLE SubCategory (

Id int primary key identity(1,1),
CategoryId int,
Name nvarchar(max),
IsDeleted bit default 0,

FOREIGN KEY (CategoryId) REFERENCES Category(Id)


);

INSERT INTO SubCategory VALUES ((select Id from Category where Name = 'Masculino'), 'Roupas', 0)
INSERT INTO SubCategory VALUES ((select Id from Category where Name = 'Feminino'), 'Roupas', 0)

CREATE TABLE ProductType (

Id int primary key identity(1,1),
SubCategoryId int,
Name nvarchar(max),
IsDeleted bit default 0,

FOREIGN KEY (SubCategoryId) REFERENCES SubCategory(Id)

);

INSERT INTO ProductType values ((Select Id from SubCategory where Name = 'Roupas' and CategoryId = (select Id from Category where Name = 'Masculino')), 'Camisetas', 0)
INSERT INTO ProductType values ((Select Id from SubCategory where Name = 'Roupas' and CategoryId = (select Id from Category where Name = 'Feminino')), 'Camisetas', 0)


alter table Product 
add SubCategoryId int


alter table Product 
add ProductTypeId int