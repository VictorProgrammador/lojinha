select top 5 * from PersonBusinessArchive


CREATE TABLE ProductArchive (

Id int primary key identity(1,1),
ProductId int, 
Name nvarchar(max),
Extension nvarchar(max),
Url nvarchar(max),
CreateDate DATETIME,
Description nvarchar(max),
UserPath nvarchar(max),

FOREIGN KEY (ProductId) REFERENCES Product(Id)

);


