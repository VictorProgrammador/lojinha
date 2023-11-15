select * from AspNetUsers where Id = '91B89D7A-E270-409B-8994-7DFA3B13A982'
select * from UserInfo order by Id desc


--select * from AspNetUserRoles

delete AspNetUserRoles where UserId in ('ffb65439-d808-46a6-b968-db4f4d70728a')


delete AspNetUsers where Id in('ffb65439-d808-46a6-b968-db4f4d70728a')

--antes de deletar, pegar o personid.
delete UserInfo where AspNetUserId in('ffb65439-d808-46a6-b968-db4f4d70728a')

select * from UserInfo where AspNetUserId in('ffb65439-d808-46a6-b968-db4f4d70728a')

delete PersonBusiness where PersonId in(30)

delete Person where Id in(30)

select * from Person where Id = 24

delete UserClassificationBusiness
delete PersonCategoryTag


select  AspNetUsers.Id as 'Id',
AspNetUsers.UserName as 'Email',
userInfo.Password as 'Senha',
person.Id as 'Pessoa',
PersonBusiness.Id as 'Negócio'
from Person person
inner join UserInfo userInfo
on userInfo.PersonId = person.Id
left join PersonBusiness PersonBusiness
on PersonBusiness.PersonId = person.Id
inner join AspNetUsers AspNetUsers
on AspNetUsers.Id = userInfo.AspNetUserId