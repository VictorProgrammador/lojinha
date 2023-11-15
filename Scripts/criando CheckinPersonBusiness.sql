

create table CheckinPersonBusinessRaffle (

Id int primary key identity(1,1),
PersonBusinessId INT,
Instagram nvarchar(max),
RaffleId int,
CreateDate datetime,
SortedNumber INT,

CONSTRAINT UQ_RaffleId_SortedNumber UNIQUE (RaffleId, SortedNumber),
FOREIGN KEY (PersonBusinessId) REFERENCES PersonBusiness(Id),
FOREIGN KEY (RaffleId) REFERENCES Raffle(Id)

);


