--create table Tag (

--Id int primary key identity(1,1),
--Name nvarchar(255),
--CategoryId int,
--IsDeleted BIT DEFAULT 0,

--FOREIGN KEY(CategoryId) REFERENCES Category(Id)

--);

--alter table Tag
--add Image nvarchar(max)

--INSERT INTO Tag VALUES ('Maquiagem', (Select Id from Category where Name = 'Estética'), 0)
--INSERT INTO Tag VALUES ('Cabelos', (Select Id from Category where Name = 'Estética'), 0)
--INSERT INTO Tag VALUES ('Unhas', (Select Id from Category where Name = 'Estética'), 0)
--INSERT INTO Tag VALUES ('Pedicure', (Select Id from Category where Name = 'Estética'), 0)
--INSERT INTO Tag VALUES ('Manicure', (Select Id from Category where Name = 'Estética'), 0)

--INSERT INTO Tag VALUES ('Desenvolvimento Web', (Select Id from Category where Name = 'Tecnologia'), 0)
--INSERT INTO Tag VALUES ('Criador de Aplicativo', (Select Id from Category where Name = 'Tecnologia'), 0)
--INSERT INTO Tag VALUES ('Manutenção de Computador', (Select Id from Category where Name = 'Tecnologia'), 0)
--INSERT INTO Tag VALUES ('Instalação de Redes/Wifi', (Select Id from Category where Name = 'Tecnologia'), 0)

--INSERT INTO Tag VALUES ('Motorista Aplicativo', (Select Id from Category where Name = 'Motorista'), 0)
--INSERT INTO Tag VALUES ('Motorista Transporte de Cargas', (Select Id from Category where Name = 'Motorista'), 0)
--INSERT INTO Tag VALUES ('Motorista Entregador', (Select Id from Category where Name = 'Motorista'), 0)
--INSERT INTO Tag VALUES ('Motorista Escolar', (Select Id from Category where Name = 'Motorista'), 0)

--CREATE TABLE PersonCategoryTag(
--Id int primary key identity(1,1),
--PersonId int,
--CategoryId int,
--TagId int,

--FOREIGN KEY(PersonId) REFERENCES Person(Id),
--FOREIGN KEY(CategoryId) REFERENCES Category(Id),
--FOREIGN KEY(TagId) REFERENCES Tag(Id)

--);

