CREATE TABLE Raffle (

Id int primary key identity(1,1),
PersonBusinessId int,
Name nvarchar(max),
Description nvarchar(max),
Value decimal (14,2),
CreateDate DATETIME,
Image nvarchar(max),

FOREIGN KEY (PersonBusinessId) REFERENCES PersonBusiness(Id)


);


alter table Raffle
add UniqueId UNIQUEIDENTIFIER


