create table PersonBusiness (

Id int primary key identity(1,1),
PersonId int,
timeAttend nvarchar(max),
DescriptionBusiness nvarchar(max),
CategoryId int,

FOREIGN KEY (PersonId) REFERENCES Person(Id),
FOREIGN KEY (CategoryId) REFERENCES Category(Id)

);

alter table PersonBusiness
add Approved BIT DEFAULT 0

alter table PersonBusiness
add IsVisible BIT DEFAULT 0


alter table PersonBusiness
add IsSubscriber BIT DEFAULT 0

alter table PersonBusiness
add LimitSubscription DATETIME


alter table PersonBusiness
add CreateDate datetime


alter table PersonBusiness
add FazerEntregas BIT DEFAULT 0


alter table PersonBusiness
add HabilitarCompras BIT DEFAULT 0

-- RODAR QUANDO SUBIR OS PREMIOS

alter table PersonBusiness
add ProgramaQRCode BIT DEFAULT 0


alter table PersonBusiness
add QtdCheckinPremio INT




