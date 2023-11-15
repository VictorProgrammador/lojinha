select person.Name as 'Empresa', 
category.Name as 'Categoria', 
LastPersonCategoryTag.Tag as 'Tag',
person.WhatsApp as 'WhatsApp',
CASE WHEN PersonBusiness.IsSubscriber = 1 THEN '20' ELSE '10' END AS 'Valor',
FORMAT(PersonBusiness.LimitSubscription, 'dd/MM/yyyy') as 'Dt. Vencimento'
from UserInfo userInfo
inner join AspNetUsers netUser
on netUser.Id = userInfo.AspNetUserId
inner join Person person
on person.Id = userInfo.PersonId
inner join PersonBusiness PersonBusiness
on PersonBusiness.PersonId = person.Id
inner join Category category
on category.Id = PersonBusiness.CategoryId
left join(
			select ROW_NUMBER() OVER(PARTITION BY PersonCategoryTag.PersonId ORDER BY PersonCategoryTag.Id DESC) as RowNumber,
			PersonCategoryTag.PersonId as 'PersonId',
			Tag.Name as 'Tag'
			from PersonCategoryTag PersonCategoryTag with(nolock)
		    inner join Tag Tag
			on Tag.Id = PersonCategoryTag.TagId										
) as LastPersonCategoryTag
on LastPersonCategoryTag.PersonId = person.Id
and LastPersonCategoryTag.RowNumber = 1
where PersonBusiness.Approved = 1
order by person.Name



