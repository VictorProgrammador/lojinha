CREATE TABLE Cor (

Id int primary key identity(1,1),
Name nvarchar(max),
IsDeleted bit default 0

);


INSERT INTO Cor VALUES ('Branca', 0)


create table ProductCor (

Id int primary key identity(1,1),
ProductId int,
CorId int,

FOREIGN KEY (ProductId) REFERENCES Product(Id),
FOREIGN KEY (CorId) REFERENCES Cor(Id)

);