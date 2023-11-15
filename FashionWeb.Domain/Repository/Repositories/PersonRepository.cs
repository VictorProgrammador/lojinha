using FashionWeb.Domain.Entities;
using FashionWeb.Domain.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using Dapper;
using FashionWeb.Domain.InfraStructure;

namespace FashionWeb.Domain.Repository.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        public PersonRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public int Create(Person person)
        {
            var insertPerson = @"INSERT INTO [Person] OUTPUT INSERTED.Id VALUES (@Name, @Age, @BirthDate, @Address, @Neighborhood, @Phone1, @Phone2, @WhatsApp, @Instagram, @PhotoUrl, NULL, @Cpf, @City, @Zipcode)";
            int id = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                id = db.QuerySingle<int>(insertPerson, new
                {
                    person.Name,
                    person.Age,
                    person.BirthDate,
                    person.Address,
                    person.Neighborhood,
                    person.Phone1,
                    person.Phone2,
                    person.WhatsApp,
                    person.Instagram,
                    person.PhotoUrl,
                    person.Cpf,
                    person.City,
                    person.Zipcode
                });

                db.Close();
            }

            return id;
        }
        public bool Update(Person entity)
        {
            var updatePerson = @"UPDATE [Person] SET Name = @Name, Address = @Address, 
                                Neighborhood = @Neighborhood, Phone1 = @Phone1, Phone2 = @Phone2,
                                Cpf = @Cpf, City = @City, Zipcode = @Zipcode WHERE Id = @Id";

            int rowsAffect = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                rowsAffect = db.Execute(updatePerson, new
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Address = entity.Address,
                    Neighborhood = entity.Neighborhood,
                    Phone1 = entity.Phone1,
                    Phone2 = entity.Phone2,
                    Cpf = entity.Cpf,
                    City = entity.City,
                    Zipcode = entity.Zipcode
                });

                db.Close();
            }

            return rowsAffect > 0 ? true : false;
        }
        public Person Get(int id)
        {
            Person person = null;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                person = db.QuerySingle<Person>(@"Select person.*
                                                    from Person person 
                                                    where person.Id = @Id",
                    new
                    {
                        Id = id
                    });


                db.Close();
            }

            return person;
        }
        public PersonBusiness GetPersonBusiness(int PersonId)
        {
            PersonBusiness personBusiness = null;
            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();
                var sql = @"select Personbusiness.*
                                                    from Personbusiness Personbusiness 
                                                    LEFT JOIN Category category
                                                    ON category.Id = Personbusiness.CategoryId
                                                    where Personbusiness.Personid = @PersonId";

                personBusiness = db.QuerySingleOrDefault<PersonBusiness>(sql, new { PersonId = PersonId });
                if(personBusiness != null && personBusiness.CategoryId != null)
                {
                    var category = new Category()
                    {
                        Id = personBusiness.CategoryId.Value
                    };

                    personBusiness.Category = category;
                }

                db.Close();
            }
            return personBusiness;
        }

        public bool UpdatePersonPhoto(int Id, string PhotoUrl)
        {
            var updatePerson = @"UPDATE [Person] SET PhotoUrl = @PhotoUrl WHERE Id = @Id";

            int rowsAffect = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                rowsAffect = db.Execute(updatePerson, new
                {
                    Id = Id,                    
                    PhotoUrl = PhotoUrl
                });

                db.Close();
            }

            return rowsAffect > 0 ? true : false;
        }

        public bool UpdatePersonBannerPhoto(int Id, string BannerUrl)
        {
            var updatePerson = @"UPDATE [Person] SET BannerUrl = @BannerUrl WHERE Id = @Id";

            int rowsAffect = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                rowsAffect = db.Execute(updatePerson, new
                {
                    Id = Id,
                    BannerUrl = BannerUrl
                });

                db.Close();
            }

            return rowsAffect > 0 ? true : false;
        }
    }
}
