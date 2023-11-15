CREATE TABLE PersonBusinessArchive (

Id int primary key identity(1,1),
PersonBusinessId int,
UserPath nvarchar(max),
Name nvarchar(max),
Description nvarchar(max),
Extension nvarchar(max),
Url nvarchar(max),
CreateDate datetime,

FOREIGN KEY (PersonBusinessId) REFERENCES PersonBusiness(Id)

); 



alter table PersonBusinessArchive 
add Title nvarchar(max)