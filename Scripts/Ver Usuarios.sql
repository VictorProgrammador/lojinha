select person.Id, person.Name, netUser.UserName, userInfo.PassWord
from UserInfo userInfo
inner join AspNetUsers netUser
on netUser.Id = userInfo.AspNetUserId
inner join Person person
on person.Id = userInfo.PersonId
where person.Name like '%inform%'
order by person.Name


SELECT * FROM Tag where CategoryId = 25

INSERT INTO Tag VALUES ('Escola de Informática', 25, 0, '')