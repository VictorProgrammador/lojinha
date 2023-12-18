  
create table Cart (

Id int primary key identity(1,1),
PersonId int,

FOREIGN KEY (PersonId) references Person(Id)


);


create table CartProduct (

Id int primary key identity(1,1),
ProductId int,
CartId int,
Color nvarchar(max),
Tamanho nvarchar(max),

FOREIGN KEY (CartId) references Cart(Id),
FOREIGN KEY (ProductId) references Product(Id),
);



