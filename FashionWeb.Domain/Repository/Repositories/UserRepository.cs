using FashionWeb.Domain.Entities;
using FashionWeb.Domain.Repository.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using Dapper;
using FashionWeb.Domain.InfraStructure;

namespace FashionWeb.Domain.Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        public UserRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public int Create(UserInfo entity)
        {
            var insertUser = @"INSERT INTO [UserInfo] OUTPUT INSERTED.Id VALUES (@PersonId, @AspNetUserId, @UniqueId, @Password)";
            int id = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                id = db.QuerySingle<int>(insertUser, new
                {
                    PersonId = entity.PersonId,
                    AspNetUserId = entity.AspNetUserId,
                    UniqueId = entity.UniqueId,
                    Password = entity.Password
                });

                db.Close();
            }

            return id;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public UserInfo Get(Guid AspNetUserId)
        {
            UserInfo user = null;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                user = db.QuerySingle<UserInfo>(@"Select userInfo.*
                                                    from UserInfo userInfo 
                                                    where userInfo.AspNetUserId = @AspNetUserId",
                    new
                    {
                        AspNetUserId = AspNetUserId
                    });


                db.Close();
            }

            return user;
        }

        public UserInfo GetUserByPersonId(int PersonId)
        {
            UserInfo user = null;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                user = db.QuerySingle<UserInfo>(@"Select userInfo.*
                                                    from UserInfo userInfo 
                                                    where userInfo.PersonId = @PersonId",
                    new
                    {
                        PersonId = PersonId
                    });


                db.Close();
            }

            return user;
        }

        public IEnumerable<UserInfo> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(int id)
        {
            throw new NotImplementedException();
        }

        public void EmailConfirmed(string Id)
        {
            var confirmUser = @"UPDATE [AspNetUsers] SET EmailConfirmed = 1 WHERE Id = @Id";

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                db.Execute(confirmUser, new
                {
                    Id = Id
                });

                db.Close();
            }
        }

        public bool VerifyEmailConfirmed(string Id)
        {
            bool isConfirmed = false;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                isConfirmed = db.QuerySingle<bool>(@"Select EmailConfirmed from AspNetUsers where Id = @Id",
                    new
                    {
                        Id = Id
                    });


                db.Close();
            }

            return isConfirmed;
        }

        public string GetAspNetUser(Guid Id)
        {
            string user = "";

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                user = db.QuerySingle<string>(@"Select AspNetUsers.UserName
                                                    from AspNetUsers AspNetUsers 
                                                    where AspNetUsers.Id = @Id",
                    new
                    {
                        Id = Id
                    });


                db.Close();
            }

            return user;
        }
    }
}
