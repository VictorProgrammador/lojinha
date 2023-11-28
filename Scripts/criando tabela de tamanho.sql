CREATE TABLE Tamanho (

Id int primary key identity(1,1),
Name nvarchar(max),
IsDeleted bit default 0

);


INSERT INTO Tamanho VALUES ('P', 0)
INSERT INTO Tamanho VALUES ('M', 0)
INSERT INTO Tamanho VALUES ('G', 0)