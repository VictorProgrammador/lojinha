create table Product(

Id int primary key identity(1,1),
Name nvarchar(max),
Value decimal(14,2),
CreateDate DATETIME,
PersonBusinessId INT,

FOREIGN KEY (PersonBusinessId) references PersonBusiness(Id)
);


alter table Product
add Description nvarchar(max)

alter table Product
add Image nvarchar(max)

alter table Product
add AcceptComplement bit default 0

alter table Product
add IsComplement bit default 0


alter table Product
add MaxComplement int 

alter table Product
add CategoryId INT


alter table Product
add IsDigitalShop BIT
