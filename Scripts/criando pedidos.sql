

create table OrderStatus (

Id int primary key identity(1,1),
Name varchar(max)

);

INSERT INTO OrderStatus VALUES ('Pedido Realizado')
INSERT INTO OrderStatus VALUES ('Pedido Cancelado')

create table Card (

Id int primary key identity(1,1),
CpfTitular nvarchar(max),
Cvv nvarchar(max),
NomeTitular nvarchar(max),
NumeroCartao nvarchar(max),
TelefoneTitular nvarchar(max),
Validade nvarchar(max),
CreateDate datetime

);

create table Orderr (

Id int primary key identity(1,1),
PersonId int,
OrderNumber nvarchar(255),
CreateDate datetime,
UpdateDate datetime,
OrderStatusId int,
Observation nvarchar(max),
CardId int,
FreteService nvarchar(max),
DeliveryRangeMin int,
DeliveryRangeMax int,
FretePrice decimal(14,2),
Parcelamento int,
ValorTotal decimal(14,2),
Bairro nvarchar(max),
Cep nvarchar(max),
Cidade nvarchar(max),
Complemento nvarchar(max),
NumeroCasa nvarchar(max),
Rua nvarchar(max),

FOREIGN KEY (PersonId) REFERENCES Person(Id),
FOREIGN KEY (OrderStatusId) REFERENCES OrderStatus(Id),
FOREIGN KEY (CardId) REFERENCES Card(Id)

);

alter table Orderr
add Revelado bit default 0


create table OrderProduct (

Id int primary key identity(1,1),
OrderId int,
ProductId int,

FOREIGN KEY (OrderId) REFERENCES Orderr(Id),
FOREIGN KEY (ProductId) REFERENCES Product(Id)


);


select * from CartProduct