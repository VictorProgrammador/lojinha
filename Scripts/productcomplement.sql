create table ProductComplement (

Id int primary key identity(1,1),
ProductId int,
ComplementId int,

FOREIGN KEY(ProductId) REFERENCES Product(Id),
FOREIGN KEY(ComplementId) REFERENCES Product(Id)

);


