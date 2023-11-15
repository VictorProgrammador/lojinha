create table UserClassificationBusiness(

Id int primary key identity(1,1),
PersonBusinessId int,
PersonId int,
Rating int,

FOREIGN KEY (PersonBusinessId) REFERENCES PersonBusiness(Id),
FOREIGN KEY(PersonId) REFERENCES Person(Id)

);

--select * from AspNetUsers

--select * from UserClassificationBusiness

SELECT *  FROM UserClassificationBusiness where PersonBusinessId = 2

select * from PersonBusiness
select * from Person where Id = 8