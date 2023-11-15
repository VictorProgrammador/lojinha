using FashionWeb.Domain.Entities;
using FashionWeb.Domain.InfraStructure;
using FashionWeb.Domain.Repository.Interfaces;
using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using FashionWeb.Domain.InfraStructure.Response;
using FashionWeb.Domain.InfraStructure.Request;
using FashionWeb.Domain.Entities.Order;

namespace FashionWeb.Domain.Repository.Repositories
{
    public class CoreRepository : ICoreRepository
    {

        private readonly ISqlConnectionFactory _connectionFactory;
        public CoreRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            var db = _connectionFactory.GetConnection();
            var result = db.Query<Category>("SELECT * FROM Category where IsNull(IsDeleted, 0) = 0 and IsDigitalShop = 1 order by Name");
            return result;
        }
        public Category GetCategory(int Id)
        {
            var db = _connectionFactory.GetConnection();
            var result = db.QuerySingle<Category>("SELECT * FROM Category where IsNull(IsDeleted, 0) = 0 and Id = @Id", new { Id = Id });
            return result;
        }

        public List<Tag> GetTagsByCategory(int categoryId)
        {
            var db = _connectionFactory.GetConnection();
            var result = db.Query<Tag>("SELECT * FROM Tag where CategoryId = @CategoryId and IsNull(IsDeleted, 0) = 0", new { CategoryId = categoryId }).ToList();
            return result;
        }

        public bool InsertMyBusiness(PersonBusiness personBusiness)
        {
            var insertPersonBusiness = @"INSERT INTO [PersonBusiness] VALUES (@PersonId, @timeAttend, @DescriptionBusiness, @CategoryId, @Approved, @IsVisible, @IsSubscriber, NULL, @CreateDate, @FazerEntregas, @HabilitarCompras, @ProgramaQRCode, @QtdCheckinPremio)";
            int id = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                id = db.Execute(insertPersonBusiness, new
                {
                    PersonId = personBusiness.PersonId,
                    timeAttend = personBusiness.timeAttend,
                    DescriptionBusiness = personBusiness.DescriptionBusiness,
                    CategoryId = personBusiness.CategoryId,
                    Approved = false,
                    IsVisible = true,
                    IsSubscriber = false,
                    CreateDate = DateTime.Now,
                    FazerEntregas = personBusiness.FazerEntregas,
                    HabilitarCompras = personBusiness.HabilitarCompras,
                    ProgramaQRCode = personBusiness.ProgramaQRCode,
                    QtdCheckinPremio = personBusiness.QtdCheckinPremio
                });

                db.Close();
            }

            return id > 0 ? true : false;
        }

        public bool UpdateMyBusiness(PersonBusiness personBusiness)
        {
            var updatePersonBusiness = @"UPDATE [PersonBusiness] SET timeAttend = @timeAttend, DescriptionBusiness = @DescriptionBusiness, 
                                CategoryId = @CategoryId, IsVisible = @IsVisible, FazerEntregas = @FazerEntregas, HabilitarCompras = @HabilitarCompras WHERE Id = @Id";

            int rowsAffect = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                rowsAffect = db.Execute(updatePersonBusiness, new
                {
                    timeAttend = personBusiness.timeAttend,
                    DescriptionBusiness = personBusiness.DescriptionBusiness,
                    CategoryId = personBusiness.CategoryId,
                    IsVisible = personBusiness.IsVisible,
                    FazerEntregas = personBusiness.FazerEntregas,
                    HabilitarCompras = personBusiness.HabilitarCompras,
                    Id = personBusiness.Id
                });

                db.Close();
            }

            return rowsAffect > 0 ? true : false;
        }

        public bool UpdatePersonBusinessManagement(PersonBusiness personBusiness)
        {
            var updatePersonBusiness = @"UPDATE [PersonBusiness] SET Approved = @Approved, IsSubscriber = @IsSubscriber, LimitSubscription = @LimitSubscription WHERE Id = @Id";

            int rowsAffect = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                rowsAffect = db.Execute(updatePersonBusiness, new
                {
                    Approved = personBusiness.Approved,
                    IsSubscriber = personBusiness.IsSubscriber,
                    LimitSubscription = personBusiness.LimitSubscription,
                    Id = personBusiness.Id
                });

                db.Close();
            }

            return rowsAffect > 0 ? true : false;
        }

        public List<PersonCategoryTag> GetPersonCategoryTag(int PersonId)
        {
            var result = new List<PersonCategoryTag>();
            try
            {
                var db = _connectionFactory.GetConnection();

                var sql = @"SELECT PersonCategoryTag.*,
                                                        Tag.Id,
                                                        Tag.Name
                                                        FROM PersonCategoryTag PersonCategoryTag 
                                                        INNER JOIN Tag Tag
                                                        ON Tag.Id = PersonCategoryTag.TagId
                                                        where PersonCategoryTag.PersonId = @PersonId";

                result = db.Query<PersonCategoryTag, Tag, PersonCategoryTag>(sql, (PersonCategoryTag, Tag) =>
                {
                    PersonCategoryTag.Tag = Tag;
                    return PersonCategoryTag;
                }, new { PersonId = PersonId }, splitOn: "TagId").ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public List<PersonCategoryTag> GetPersonCategoryTag(int PersonId, int CategoryId)
        {
            var result = new List<PersonCategoryTag>();
            try
            {
                var db = _connectionFactory.GetConnection();

                var sql = @"SELECT PersonCategoryTag.*,
                                                        Tag.Id,
                                                        Tag.Name
                                                        FROM PersonCategoryTag PersonCategoryTag 
                                                        INNER JOIN Tag Tag
                                                        ON Tag.Id = PersonCategoryTag.TagId
                                                        where PersonCategoryTag.PersonId = @PersonId AND PersonCategoryTag.CategoryId = @CategoryId";

                result = db.Query<PersonCategoryTag, Tag, PersonCategoryTag>(sql, (PersonCategoryTag, Tag) =>
                {
                    PersonCategoryTag.Tag = Tag;
                    return PersonCategoryTag;
                }, new { PersonId = PersonId, CategoryId = CategoryId }, splitOn: "TagId").ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public bool InsertPersonCategoryTag(int PersonId, int CategoryId, int TagId)
        {
            var insertPersonCategoryTag = @"INSERT INTO [PersonCategoryTag] VALUES (@PersonId, @CategoryId, @TagId)";

            int rowsAffect = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                rowsAffect = db.Execute(insertPersonCategoryTag, new
                {
                    PersonId = PersonId,
                    CategoryId = CategoryId,
                    TagId = TagId
                });

                db.Close();
            }

            return rowsAffect > 0 ? true : false;
        }
        public bool DeletePersonCategoryTag(int Id)
        {
            var deletePersonCategoryTag = @"DELETE [PersonCategoryTag] WHERE Id = @Id";

            int rowsAffect = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                rowsAffect = db.Execute(deletePersonCategoryTag, new
                {
                    Id = Id
                });

                db.Close();
            }

            return rowsAffect > 0 ? true : false;
        }

        public PagedResult<PersonBusiness> GetPersonsBusiness(SearchPersonBusiness searchPersonBusiness)
        {
            var result = new PagedResult<PersonBusiness>();

            var countSql = $"SELECT COUNT(*) FROM PersonBusiness personBusiness inner join Person person on person.Id = personBusiness.PersonId WHERE personBusiness.Approved = 1 and personBusiness.IsVisible = 1";
            bool hasCategory = false;

            if (searchPersonBusiness.Category?.Id > 0)
            {
                countSql += " AND personBusiness.CategoryId = @CategoryId ";
                hasCategory = true;
            }

            if (!string.IsNullOrEmpty(searchPersonBusiness.Name))
            {
                countSql += " AND person.Name LIKE @Name ";
            }

            if(searchPersonBusiness.IsSubscriber == true)
            {
                countSql += " AND personBusiness.IsSubscriber = 1 ";
            }

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                var totalCount = db.ExecuteScalar<int>(countSql, new { CategoryId = searchPersonBusiness.Category?.Id, Name = "%" + searchPersonBusiness.Name + "%" });
                var totalPages = (int)Math.Ceiling((double)totalCount / searchPersonBusiness.PageSize);

                var orderBy = $"[PersonBusiness].{searchPersonBusiness.sortColumn} {(searchPersonBusiness.sortAscending ? "ASC" : "DESC")}";

                //Define as cláusulas LIMIT e OFFSET para a consulta
                var Limit = searchPersonBusiness.PageSize;
                var Offset = (searchPersonBusiness.PageNumber - 1) * searchPersonBusiness.PageSize;

                // Define a consulta SQL para puxar as entidades
                var sql = $@"SELECT [PersonBusiness].Id,
                                    [PersonBusiness].PersonId,
                                    [PersonBusiness].CategoryId,
                                    [PersonBusiness].IsSubscriber,
                                    [PersonBusiness].FazerEntregas,
                                    [Person].[Id] AS [PersonId],
                                    [Person].[Name],
                                    [Person].[PhotoUrl]
                                    FROM [PersonBusiness] 
                                    INNER JOIN [Person]
                                    ON [Person].[Id] = [PersonBusiness].[PersonId]
                                    WHERE [PersonBusiness].[Approved] = 1 AND [PersonBusiness].[IsVisible] = 1";

                if (hasCategory)
                {
                    sql += " AND [PersonBusiness].[CategoryId] = @CategoryId ";
                }

                if (!string.IsNullOrEmpty(searchPersonBusiness.Name))
                {
                    sql += " AND [Person].[Name] LIKE @Name ";
                }

                if (searchPersonBusiness.IsSubscriber == true)
                {
                    sql += " AND [PersonBusiness].[IsSubscriber] = 1 ";
                }

                sql += $" ORDER BY {orderBy} OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY ";
                var entities = db.Query<PersonBusiness, Person, PersonBusiness>(sql, (personBusiness, person) =>
                {
                    person.Id = personBusiness.PersonId;
                    personBusiness.Person = person;
                    return personBusiness;
                }, new { CategoryId = searchPersonBusiness.Category?.Id, Name = "%" + searchPersonBusiness.Name + "%", Offset = Offset, Limit = Limit }, splitOn: "PersonId").ToList();

                //sql += $" ORDER BY {orderBy}";

                //var entities = db.Query<PersonBusiness, Person, PersonBusiness>(sql, (personBusiness, person) =>
                //{
                //    person.Id = personBusiness.PersonId;
                //    personBusiness.Person = person;
                //    return personBusiness;
                //}, new { CategoryId = searchPersonBusiness.CategoryId, Name = "%" + searchPersonBusiness.Name + "%" }, splitOn: "PersonId").ToList();

                if (entities.Count == 0)
                {
                    return new PagedResult<PersonBusiness>
                    {
                        PageNumber = searchPersonBusiness.PageNumber,
                        PageSize = searchPersonBusiness.PageSize,
                        TotalResults = totalCount,
                        TotalPages = totalPages,
                        Results = entities,
                    };
                }

                result = new PagedResult<PersonBusiness>
                {
                    PageNumber = searchPersonBusiness.PageNumber,
                    PageSize = searchPersonBusiness.PageSize,
                    TotalResults = totalCount,
                    TotalPages = totalPages,
                    Results = entities,
                };

                db.Close();
            }

            return result;
        }

        public PagedResult<PersonBusiness> GetPersonsBusinessByTag(SearchPersonBusiness searchPersonBusiness)
        {
            var result = new PagedResult<PersonBusiness>();

            var countSql = @$"SELECT COUNT(*) FROM PersonBusiness personBusiness 
                                inner join Person person 
                                on person.Id = personBusiness.PersonId 
                                INNER JOIN Category Category
                                on Category.Id = personBusiness.CategoryId
                                inner join(
										    select 
											ROW_NUMBER() OVER(PARTITION BY PersonCategoryTag.PersonId ORDER BY PersonCategoryTag.Id DESC) as RowNumber,
											PersonCategoryTag.PersonId,
                                            PersonCategoryTag.CategoryId,
                                            PersonCategoryTag.TagId

										    from PersonCategoryTag PersonCategoryTag with(nolock)
											where PersonCategoryTag.TagId = @TagId
									        ) as PersonCategoryTag
									        on PersonCategoryTag.PersonId = personBusiness.PersonId
									        and PersonCategoryTag.RowNumber=1

                                WHERE personBusiness.Approved = 1 and personBusiness.IsVisible = 1";

            bool hasCategory = false;

            if (searchPersonBusiness.Category?.Id > 0)
            {
                countSql += " AND personBusiness.CategoryId = @CategoryId ";
                hasCategory = true;
            }

            if (!string.IsNullOrEmpty(searchPersonBusiness.Name))
            {
                countSql += " AND person.Name LIKE @Name ";
            }

            if (searchPersonBusiness.IsSubscriber == true)
            {
                countSql += " AND personBusiness.IsSubscriber = 1 ";
            }

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                var totalCount = db.ExecuteScalar<int>(countSql, new { CategoryId = searchPersonBusiness.Category?.Id, Name = "%" + searchPersonBusiness.Name + "%", TagId = searchPersonBusiness.TagId });
                var totalPages = (int)Math.Ceiling((double)totalCount / searchPersonBusiness.PageSize);

                var orderBy = $"[PersonBusiness].{searchPersonBusiness.sortColumn} {(searchPersonBusiness.sortAscending ? "ASC" : "DESC")}";

                //Define as cláusulas LIMIT e OFFSET para a consulta
                var Limit = searchPersonBusiness.PageSize;
                var Offset = (searchPersonBusiness.PageNumber - 1) * searchPersonBusiness.PageSize;

                // Define a consulta SQL para puxar as entidades
                var sql = $@"SELECT [PersonBusiness].Id,
                                    [PersonBusiness].PersonId,
                                    [PersonBusiness].CategoryId,
                                    [PersonBusiness].IsSubscriber,
                                    [PersonBusiness].FazerEntregas,
                                    [Person].[Id] AS [PersonId],
                                    [Person].[Name],
                                    [Person].[PhotoUrl]
                                    FROM [PersonBusiness] 
                                    INNER JOIN [Person]
                                    ON [Person].[Id] = [PersonBusiness].[PersonId]
                                    inner join(
										    select 
											ROW_NUMBER() OVER(PARTITION BY PersonCategoryTag.PersonId ORDER BY PersonCategoryTag.Id DESC) as RowNumber,
											PersonCategoryTag.PersonId,
                                            PersonCategoryTag.CategoryId,
                                            PersonCategoryTag.TagId

										    from PersonCategoryTag PersonCategoryTag with(nolock)
											where PersonCategoryTag.TagId = @TagId
									        ) as PersonCategoryTag
									        on PersonCategoryTag.PersonId = personBusiness.PersonId
									        and PersonCategoryTag.RowNumber=1
                                    WHERE [PersonBusiness].[Approved] = 1 AND [PersonBusiness].[IsVisible] = 1";

                if (hasCategory)
                {
                    sql += " AND [PersonBusiness].[CategoryId] = @CategoryId ";
                }

                if (!string.IsNullOrEmpty(searchPersonBusiness.Name))
                {
                    sql += " AND [Person].[Name] LIKE @Name ";
                }

                if (searchPersonBusiness.IsSubscriber == true)
                {
                    sql += " AND [PersonBusiness].[IsSubscriber] = 1 ";
                }

                sql += $" ORDER BY {orderBy} OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY ";
                var entities = db.Query<PersonBusiness, Person, PersonBusiness>(sql, (personBusiness, person) =>
                {
                    person.Id = personBusiness.PersonId;
                    personBusiness.Person = person;
                    return personBusiness;
                }, new { CategoryId = searchPersonBusiness.Category?.Id, Name = "%" + searchPersonBusiness.Name + "%", TagId = searchPersonBusiness.TagId, Offset = Offset, Limit = Limit }, splitOn: "PersonId").ToList();

                //sql += $" ORDER BY {orderBy}";

                //var entities = db.Query<PersonBusiness, Person, PersonBusiness>(sql, (personBusiness, person) =>
                //{
                //    person.Id = personBusiness.PersonId;
                //    personBusiness.Person = person;
                //    return personBusiness;
                //}, new { CategoryId = searchPersonBusiness.CategoryId, Name = "%" + searchPersonBusiness.Name + "%" }, splitOn: "PersonId").ToList();

                if (entities.Count == 0)
                {
                    return new PagedResult<PersonBusiness>
                    {
                        PageNumber = searchPersonBusiness.PageNumber,
                        PageSize = searchPersonBusiness.PageSize,
                        TotalResults = totalCount,
                        TotalPages = totalPages,
                        Results = entities,
                    };
                }

                result = new PagedResult<PersonBusiness>
                {
                    PageNumber = searchPersonBusiness.PageNumber,
                    PageSize = searchPersonBusiness.PageSize,
                    TotalResults = totalCount,
                    TotalPages = totalPages,
                    Results = entities,
                };

                db.Close();
            }

            return result;
        }
        public PagedResult<PersonBusiness> GetPersonsBusinessManagement(SearchPersonBusiness searchPersonBusiness)
        {
            var result = new PagedResult<PersonBusiness>();

            var countSql = @$"SELECT COUNT(*) FROM PersonBusiness personBusiness 
                                inner join Person person 
                                on person.Id = personBusiness.PersonId
                                WHERE 1 = 1 ";

            bool hasCategory = false;

            if (searchPersonBusiness.CategoryId > 0)
            {
                countSql += " AND personBusiness.CategoryId = @CategoryId ";
                hasCategory = true;
            }

            if (!string.IsNullOrEmpty(searchPersonBusiness.Name))
            {
                countSql += " AND person.Name LIKE @Name ";
            }

            if (searchPersonBusiness.IsSubscriber == true)
            {
                countSql += " AND personBusiness.IsSubscriber = 1 ";
            }

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                var totalCount = db.ExecuteScalar<int>(countSql, new { CategoryId = searchPersonBusiness.CategoryId, Name = "%" + searchPersonBusiness.Name + "%" });
                var totalPages = (int)Math.Ceiling((double)totalCount / searchPersonBusiness.PageSize);

                var orderBy = $"[PersonBusiness].{searchPersonBusiness.sortColumn} {(searchPersonBusiness.sortAscending ? "ASC" : "DESC")}";

                // Define as cláusulas LIMIT e OFFSET para a consulta
                var Limit = searchPersonBusiness.PageSize;
                var Offset = (searchPersonBusiness.PageNumber - 1) * searchPersonBusiness.PageSize;

                // Define a consulta SQL para puxar as entidades
                var sql = $@"SELECT [PersonBusiness].Id,
                                    [PersonBusiness].PersonId,
                                    [PersonBusiness].CategoryId,
                                    [PersonBusiness].Approved,
                                    [PersonBusiness].IsVisible,
                                    [PersonBusiness].IsSubscriber,
                                    [PersonBusiness].LimitSubscription,
                                    [Person].[Id] AS [PersonId],
                                    [Person].[Name]
                                    FROM [PersonBusiness] 
                                    INNER JOIN [Person]
                                    ON [Person].[Id] = [PersonBusiness].[PersonId]
                                    WHERE 1 = 1";

                if (hasCategory)
                {
                    sql += " AND [PersonBusiness].[CategoryId] = @CategoryId ";
                }

                if (!string.IsNullOrEmpty(searchPersonBusiness.Name))
                {
                    sql += " AND [Person].[Name] LIKE @Name ";
                }

                if (searchPersonBusiness.IsSubscriber == true)
                {
                    sql += " AND [PersonBusiness].[IsSubscriber] = 1 ";
                }

                sql += $" ORDER BY {orderBy} OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY ";

                var entities = db.Query<PersonBusiness, Person, PersonBusiness>(sql, (personBusiness, person) =>
                {
                    person.Id = personBusiness.PersonId;
                    personBusiness.Person = person;
                    return personBusiness;
                }, new { CategoryId = searchPersonBusiness.CategoryId, Name = "%" + searchPersonBusiness.Name + "%", Offset = Offset, Limit = Limit }, splitOn: "PersonId").ToList();

                //Populando categorias e tags.
                foreach (var entity in entities)
                {
                    if (entity.CategoryId > 0)
                    {
                        entity.Category = this.GetCategory(entity.CategoryId.Value);
                        entity.Category.Tags = new List<Tag>();
                        var PersonCategoryTags = this.GetPersonCategoryTag(entity.PersonId, entity.CategoryId.Value);

                        foreach (var PersonCategoryTag in PersonCategoryTags)
                        {
                            entity.Category.Tags.Add(PersonCategoryTag.Tag);
                        }
                    }
                }

                if (entities.Count == 0)
                {
                    return new PagedResult<PersonBusiness>
                    {
                        PageNumber = searchPersonBusiness.PageNumber,
                        PageSize = searchPersonBusiness.PageSize,
                        TotalResults = totalCount,
                        TotalPages = totalPages,
                        Results = entities,
                    };
                }

                result = new PagedResult<PersonBusiness>
                {
                    PageNumber = searchPersonBusiness.PageNumber,
                    PageSize = searchPersonBusiness.PageSize,
                    TotalResults = totalCount,
                    TotalPages = totalPages,
                    Results = entities,
                };

                db.Close();
            }

            return result;
        }

        public PersonBusiness GetPersonBusinessToVisit(int PersonId)
        {
            var result = new PersonBusiness();
          
            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                // Define a consulta SQL para puxar as entidades
                var sql = $@"SELECT [PersonBusiness].Id,
                                    [PersonBusiness].PersonId,
                                    [PersonBusiness].CategoryId,
                                    [PersonBusiness].IsSubscriber,
                                    [PersonBusiness].DescriptionBusiness,
                                    [PersonBusiness].HabilitarCompras,
                                    [PersonBusiness].FazerEntregas,
                                    [PersonBusiness].timeAttend,
                                    [PersonBusiness].ProgramaQRCode,
                                    [PersonBusiness].QtdCheckinPremio,
                                    [Person].[Id] AS [PersonId],
                                    [Person].[Name],
                                    [Person].[PhotoUrl],
                                    [Person].[BannerUrl],
                                    [Person].[Address],
                                    [Person].[Neighborhood],
                                    [Person].[WhatsApp],
                                    [Person].[Instagram]
                                    FROM [PersonBusiness] 
                                    INNER JOIN [Person]
                                    ON [Person].[Id] = [PersonBusiness].[PersonId]
                                    WHERE [PersonBusiness].[Approved] = 1 AND [PersonBusiness].[IsVisible] = 1 AND [Person].[Id] = @PersonId";

                result = db.Query<PersonBusiness, Person, PersonBusiness>(sql, (personBusiness, person) =>
                {
                    person.Id = personBusiness.PersonId;
                    personBusiness.Person = person;
                    return personBusiness;
                }, new { PersonId = PersonId }, splitOn: "PersonId").FirstOrDefault();

                if (result != null && result.CategoryId > 0)
                {
                    result.Category = this.GetCategory(result.CategoryId.Value);
                    result.Category.Tags = new List<Tag>();
                    var PersonCategoryTags = this.GetPersonCategoryTag(result.PersonId, result.CategoryId.Value);

                    foreach (var PersonCategoryTag in PersonCategoryTags)
                    {
                        result.Category.Tags.Add(PersonCategoryTag.Tag);
                    }
                }

                db.Close();
            }

            return result;
        }
        public ClassificationBusiness GetAllClassificationsBusiness(int PersonBusinessId)
        {
            var db = _connectionFactory.GetConnection();
            var result = db.QuerySingleOrDefault<ClassificationBusiness>("SELECT SUM(Rating) as 'SomaAvaliacoes', COUNT(*) as 'TotalAvaliacoes' FROM UserClassificationBusiness where PersonBusinessId = @PersonBusinessId", new { PersonBusinessId = PersonBusinessId });
            return result;
        }
        public UserClassificationBusiness GetUserClassificationBusiness(UserClassificationBusiness userClassificationBusiness)
        {
            var db = _connectionFactory.GetConnection();
            var result = db.QuerySingleOrDefault<UserClassificationBusiness>("SELECT TOP 1 * FROM UserClassificationBusiness where PersonBusinessId = @PersonBusinessId and PersonId = @PersonId", new { PersonBusinessId = userClassificationBusiness.PersonBusinessId, PersonId = userClassificationBusiness.PersonId });
            return result;
        }
        public bool InsertUserClassificationBusiness(UserClassificationBusiness userClassificationBusiness)
        {
            var insertUserClassificationBusiness = @"INSERT INTO [UserClassificationBusiness] VALUES (@PersonBusinessId, @PersonId, @Rating)";
            int id = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                id = db.Execute(insertUserClassificationBusiness, new
                {
                    PersonBusinessId = userClassificationBusiness.PersonBusinessId,
                    PersonId = userClassificationBusiness.PersonId,
                    Rating = userClassificationBusiness.Rating
                });

                db.Close();
            }

            return id > 0 ? true : false;
        }

        public bool UpdateUserClassificationBusiness(UserClassificationBusiness userClassificationBusiness)
        {
            var updateUserClassificationBusiness = @"UPDATE [UserClassificationBusiness] SET Rating = @Rating WHERE Id = @Id";

            int rowsAffect = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                rowsAffect = db.Execute(updateUserClassificationBusiness, new
                {
                    Rating = userClassificationBusiness.Rating,
                    Id = userClassificationBusiness.Id
                });

                db.Close();
            }

            return rowsAffect > 0 ? true : false;
        }

        public PersonBusiness GetPersonBusiness(int Id)
        {
            var db = _connectionFactory.GetConnection();
            var result = db.QuerySingle<PersonBusiness>("SELECT Approved, IsVisible, IsSubscriber, LimitSubscription FROM PersonBusiness where Id = @Id", new { Id = Id });
            return result;
        }
        public PersonBusiness GetPersonBusinessByPersonId(int PersonId)
        {
            var db = _connectionFactory.GetConnection();
            var result = db.QuerySingleOrDefault<PersonBusiness>("SELECT Id FROM PersonBusiness where PersonId = @PersonId", new { PersonId = PersonId });
            return result;
        }

        public Product GetProduct(int Id)
        {
            var db = _connectionFactory.GetConnection();
            var result = db.QuerySingleOrDefault<Product>("SELECT * FROM Product where Id = @Id", new { Id = Id });
            return result;
        }

        public List<Product> GetPersonBusinessComplements(int PersonBusinessId)
        {
            var db = _connectionFactory.GetConnection();
            var result = db.Query<Product>("SELECT * FROM Product where PersonBusinessId = @PersonBusinessId and IsComplement = 1", new { PersonBusinessId = PersonBusinessId }).ToList();
            return result;
        }
        public PagedResult<Product> GetProductsByPersonBusinessId(SearchPersonBusinessProducts searchPersonBusinessProducts)
        {
            var result = new PagedResult<Product>();
            var countSql = $"SELECT COUNT(*) FROM Product Product WHERE PersonBusinessId = @PersonBusinessId ";

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                var totalCount = db.ExecuteScalar<int>(countSql, new { PersonBusinessId = searchPersonBusinessProducts.PersonBusinessId });
                var totalPages = (int)Math.Ceiling((double)totalCount / searchPersonBusinessProducts.PageSize);

                var orderBy = $"CreateDate DESC";

                // Define as cláusulas LIMIT e OFFSET para a consulta
                var Limit = searchPersonBusinessProducts.PageSize;
                var Offset = (searchPersonBusinessProducts.PageNumber - 1) * searchPersonBusinessProducts.PageSize;

                // Define a consulta SQL para puxar as entidades
                var sql = $@"SELECT [Product].Id,
                                    [Product].Name,
                                    [Product].Value,
                                    [Product].Description,
                                    [Product].AcceptComplement,
                                    [Product].IsComplement,
                                    [Product].MaxComplement,
                                    [Product].Image,
                                    [Product].CreateDate,
                                    [Product].PersonBusinessId,
                                    [Product].CategoryId
                                    FROM [Product] 
                                    WHERE [PersonBusinessId] = @PersonBusinessId";

                if (searchPersonBusinessProducts.AllProducts)
                {
                    sql += $" ORDER BY {orderBy} ";
                }
                else
                {
                    sql += $" ORDER BY {orderBy} OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY ";
                }

                var entities = db.Query<Product>(sql, new { PersonBusinessId = searchPersonBusinessProducts.PersonBusinessId, Offset = Offset, Limit = Limit }).ToList();

                if (entities.Count == 0)
                {
                    return new PagedResult<Product>
                    {
                        PageNumber = searchPersonBusinessProducts.PageNumber,
                        PageSize = searchPersonBusinessProducts.PageSize,
                        TotalResults = totalCount,
                        TotalPages = totalPages,
                        Results = entities,
                    };
                }

                result = new PagedResult<Product>
                {
                    PageNumber = searchPersonBusinessProducts.PageNumber,
                    PageSize = searchPersonBusinessProducts.PageSize,
                    TotalResults = totalCount,
                    TotalPages = totalPages,
                    Results = entities,
                };

                db.Close();
            }

            return result;
        }

        public bool InsertProduct(Product product)
        {
            var insertProduct = @"INSERT INTO [Product] VALUES (@Name, @Value, @CreateDate, @PersonBusinessId, @Description, @Image, @AcceptComplement, @IsComplement, @MaxComplement, @CategoryId, @IsDigitalShop)";
            int id = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                id = db.Execute(insertProduct, new
                {
                    PersonBusinessId = product.PersonBusinessId,
                    Name = product.Name,
                    Value = product.Value,
                    CreateDate = product.CreateDate,
                    Description = product.Description,
                    Image = product.Image,
                    AcceptComplement = product.AcceptComplement,
                    IsComplement = product.IsComplement,
                    MaxComplement = product.MaxComplement,
                    CategoryId = product.CategoryId,
                    IsDigitalShop = true
                });

                db.Close();
            }

            return id > 0 ? true : false;
        }

        public bool UpdateProduct(Product product)
        {
            var updateProduct = @"UPDATE [Product] SET Name = @Name, Description = @Description, Value = @Value, Image = @Image, AcceptComplement = @AcceptComplement, IsComplement = @IsComplement, MaxComplement = @MaxComplement, CategoryId = @CategoryId WHERE Id = @Id";

            int rowsAffect = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                rowsAffect = db.Execute(updateProduct, new
                {
                    Name = product.Name,
                    Description = product.Description,
                    Value = product.Value,
                    Image = product.Image,
                    AcceptComplement = product.AcceptComplement,
                    IsComplement = product.IsComplement,
                    MaxComplement = product.MaxComplement,
                    Id = product.Id,
                    CategoryId = product.CategoryId
                });

                db.Close();
            }

            return rowsAffect > 0 ? true : false;
        }
        public bool ExcluirProduto(Product product)
        {
            var excluirProduct = @"DELETE Product WHERE Id = @Id";

            int rowsAffect = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                rowsAffect = db.Execute(excluirProduct, new
                {
                    Id = product.Id
                });

                db.Close();
            }

            return rowsAffect > 0 ? true : false;
        }

        public bool PersonBusinessHasProducts(int PersonBusinessId)
        {
            var querytotalProduct = @"SELECT COUNT(*) FROM Product WHERE PersonBusinessId = @PersonBusinessId";
            int totalProduct = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                totalProduct = db.ExecuteScalar<int>(querytotalProduct, new { PersonBusinessId = PersonBusinessId });

                db.Close();
            }

            return totalProduct > 0 ? true : false;
        }
        public List<ProductComplement> GetProductComplements(int ProductId)
        {
            var result = new List<ProductComplement>();
            try
            {
                var db = _connectionFactory.GetConnection();

                var sql = @"SELECT [ProductComplement].Id,
                            [ProductComplement].[ComplementId] AS [ComplementId],
                            [complement].[Id],
                            [complement].[Name],
                            [complement].[Value]
                            FROM ProductComplement ProductComplement 
                            INNER JOIN Product complement
                            ON complement.Id = ProductComplement.ComplementId
                            where ProductComplement.ProductId = @ProductId";

                result = db.Query<ProductComplement, Product, ProductComplement>(sql, (ProductComplement, complement) =>
                {
                    ProductComplement.Complement = complement;
                    return ProductComplement;
                }, new { ProductId = ProductId }, splitOn: "ComplementId").ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public bool DeleteProductComplement(int Id)
        {
            var queryDeleteProductComplement = @"DELETE ProductComplement WHERE Id = @Id";
            int result = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                result = db.ExecuteScalar<int>(queryDeleteProductComplement, new { Id = Id });

                db.Close();
            }

            return result > 0 ? true : false;
        }
        public bool InsertProductComplement(ProductComplement productComplement)
        {
            var insertProductComplement = @"INSERT INTO [ProductComplement] VALUES (@ProductId, @ComplementId)";
            int id = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                id = db.Execute(insertProductComplement, new
                {
                    ProductId = productComplement.ProductId,
                    ComplementId = productComplement.ComplementId
                });

                db.Close();
            }

            return id > 0 ? true : false;
        }

        public PagedResult<Raffle> GetRafflesByPersonBusinessId(SearchPersonBusinessRaffles searchPersonBusinessRaffles)
        {
            var result = new PagedResult<Raffle>();
            var countSql = $"SELECT COUNT(*) FROM Raffle Raffle WHERE PersonBusinessId = @PersonBusinessId ";

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                var totalCount = db.ExecuteScalar<int>(countSql, new { PersonBusinessId = searchPersonBusinessRaffles.PersonBusinessId });
                var totalPages = (int)Math.Ceiling((double)totalCount / searchPersonBusinessRaffles.PageSize);

                var orderBy = $"IsClosed ASC";

                // Define as cláusulas LIMIT e OFFSET para a consulta
                var Limit = searchPersonBusinessRaffles.PageSize;
                var Offset = (searchPersonBusinessRaffles.PageNumber - 1) * searchPersonBusinessRaffles.PageSize;

                // Define a consulta SQL para puxar as entidades
                var sql = $@"SELECT [Raffle].Id,
                                    [Raffle].Name,
                                    [Raffle].Value,
                                    [Raffle].Description,
                                    [Raffle].Image,
                                    [Raffle].CreateDate,
                                    [Raffle].EndDate,
                                    [Raffle].IsClosed,
                                    [Raffle].UniqueId,
                                    [Raffle].QtdNumber,
                                    [Raffle].PersonBusinessId
                                    FROM [Raffle] 
                                    WHERE [PersonBusinessId] = @PersonBusinessId";

                if (searchPersonBusinessRaffles.AllRaffles)
                {
                    sql += $" ORDER BY {orderBy} ";
                }
                else
                {
                    sql += $" ORDER BY {orderBy} OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY ";
                }

                var entities = db.Query<Raffle>(sql, new { PersonBusinessId = searchPersonBusinessRaffles.PersonBusinessId, Offset = Offset, Limit = Limit }).ToList();

                if (entities.Count == 0)
                {
                    return new PagedResult<Raffle>
                    {
                        PageNumber = searchPersonBusinessRaffles.PageNumber,
                        PageSize = searchPersonBusinessRaffles.PageSize,
                        TotalResults = totalCount,
                        TotalPages = totalPages,
                        Results = entities,
                    };
                }

                result = new PagedResult<Raffle>
                {
                    PageNumber = searchPersonBusinessRaffles.PageNumber,
                    PageSize = searchPersonBusinessRaffles.PageSize,
                    TotalResults = totalCount,
                    TotalPages = totalPages,
                    Results = entities,
                };

                db.Close();
            }

            return result;
        }

        public IList<Raffle> GetPersonBusinessRafflesActive(int PersonBusinessId)
        {
            var db = _connectionFactory.GetConnection();
            var result = db.Query<Raffle>("SELECT * FROM Raffle where PersonBusinessId = @PersonBusinessId and IsClosed = 0", new { PersonBusinessId = PersonBusinessId }).ToList();
            return result;
        }

        public Raffle GetRaffle(Guid UniqueId)
        {
            var db = _connectionFactory.GetConnection();
            var result = db.QuerySingleOrDefault<Raffle>("SELECT * FROM Raffle where UniqueId = @UniqueId", new { UniqueId = UniqueId });
            return result;
        }

        public bool InsertRaffle(Raffle raffle)
        {
            var insertRaffle = @"INSERT INTO [Raffle] VALUES (@PersonBusinessId, @Name, @Description, @Value, @CreateDate, @Image, @EndDate, @IsClosed, @QtdNumber, NEWID())";
            int id = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                id = db.Execute(insertRaffle, new
                {
                    PersonBusinessId = raffle.PersonBusinessId,
                    Name = raffle.Name,
                    Description = raffle.Description,
                    Value = raffle.Value,
                    CreateDate = raffle.CreateDate,
                    Image = raffle.Image,
                    EndDate = raffle.EndDate,
                    IsClosed = raffle.IsClosed,
                    QtdNumber = raffle.QtdNumber
                });

                db.Close();
            }

            return id > 0 ? true : false;
        }
        public bool UpdateRaffle(Raffle raffle)
        {
            var updateRaffle = @"UPDATE [Raffle] SET Name = @Name, Description = @Description, Value = @Value, Image = @Image, EndDate = @EndDate, IsClosed = @IsClosed WHERE Id = @Id";

            int rowsAffect = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                rowsAffect = db.Execute(updateRaffle, new
                {
                    Name = raffle.Name,
                    Description = raffle.Description,
                    Value = raffle.Value,
                    Image = raffle.Image,
                    EndDate = raffle.EndDate,
                    IsClosed = raffle.IsClosed,
                    Id = raffle.Id
                });

                db.Close();
            }

            return rowsAffect > 0 ? true : false;
        }
        public bool ExcluirSorteio(Raffle raffle)
        {
            var excluirSorteio = @"DELETE Raffle WHERE Id = @Id";

            int rowsAffect = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                rowsAffect = db.Execute(excluirSorteio, new
                {
                    Id = raffle.Id
                });

                db.Close();
            }

            return rowsAffect > 0 ? true : false;
        }
        public bool ExistsNumberPersonBusinessRaffle(int RaffleId, int PersonBusinessId, int SortedNumber)
        {
            var queryExistsNumber = @"SELECT SortedNumber FROM CheckinPersonBusinessRaffle WHERE RaffleId = @RaffleId AND PersonBusinessId = @PersonBusinessId AND SortedNumber = @SortedNumber";
            int totalNumber = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                totalNumber = db.ExecuteScalar<int>(queryExistsNumber, new { RaffleId = RaffleId, PersonBusinessId = PersonBusinessId, SortedNumber = SortedNumber });

                db.Close();
            }

            return totalNumber > 0 ? true : false;
        }
        public int GetLastNumberPersonBusinessRaffle(int RaffleId, int PersonBusinessId)
        {
            var db = _connectionFactory.GetConnection();
            var result = db.QuerySingleOrDefault<int>("SELECT TOP 1 SortedNumber FROM CheckinPersonBusinessRaffle where PersonBusinessId = @PersonBusinessId and RaffleId = @RaffleId order by SortedNumber desc", new { PersonBusinessId = PersonBusinessId, RaffleId = RaffleId });
            return result;
        }
        public CheckinPersonBusinessRaffle GetCheckinPersonBusinessRaffle(int RaffleId, int PersonBusinessId, string Instagram)
        {
            var db = _connectionFactory.GetConnection();
            var result = db.QuerySingleOrDefault<CheckinPersonBusinessRaffle>("SELECT * FROM CheckinPersonBusinessRaffle where PersonBusinessId = @PersonBusinessId and Instagram = @Instagram and RaffleId = @RaffleId", new { PersonBusinessId = PersonBusinessId, Instagram = Instagram, RaffleId = RaffleId });
            return result;
        }
        public bool SaveCheckinPersonBusinessRaffles(CheckinPersonBusinessRaffle checkinPersonBusiness)
        {
            var insertCheckinPersonBusinessRaffles = @"INSERT INTO [CheckinPersonBusinessRaffle] VALUES (@PersonBusinessId, @Instagram, @RaffleId, @CreateDate, @SortedNumber)";
            int id = 0;

            try
            {
                using (var db = _connectionFactory.GetConnection())
                {
                    db.Open();

                    id = db.Execute(insertCheckinPersonBusinessRaffles, new
                    {
                        PersonBusinessId = checkinPersonBusiness.PersonBusinessId,
                        Instagram = checkinPersonBusiness.Instagram,
                        RaffleId = checkinPersonBusiness.RaffleId,
                        CreateDate = DateTime.Now,
                        SortedNumber = checkinPersonBusiness.SortedNumber
                    });

                    db.Close();
                }
            }
            catch(Exception ex)
            {
                id = 0;
            }

            return id > 0 ? true : false;
        }

        public List<CheckinPersonBusinessRaffle> GetCheckinsPersonBusinessRaffle(int RaffleId)
        {
            var db = _connectionFactory.GetConnection();
            var result = db.Query<CheckinPersonBusinessRaffle>("SELECT * FROM CheckinPersonBusinessRaffle where RaffleId = @RaffleId", new { RaffleId = RaffleId }).ToList();
            return result;
        }
        public bool DeleteCheckinPersonBusinessRaffle(int RaffleId, int PersonBusinessId)
        {
            var deleteCheckinPersonBusinessRaffle = @"DELETE CheckinPersonBusinessRaffle WHERE PersonBusinessId = @PersonBusinessId AND RaffleId = @RaffleId";
            int id = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                id = db.Execute(deleteCheckinPersonBusinessRaffle, new
                {
                    PersonBusinessId = PersonBusinessId,
                    RaffleId = RaffleId
                });

                db.Close();
            }

            return id > 0 ? true : false;
        }

        public bool InsertPersonBusinessArchive(PersonBusinessArchive personBusinessArchive)
        {
            var insertArchive = @"INSERT INTO [PersonBusinessArchive] VALUES (@PersonBusinessId, @UserPath, @Name, @Description, @Extension, @Url, @CreateDate, @Title)";
            int id = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                id = db.Execute(insertArchive, new
                {
                    PersonBusinessId = personBusinessArchive.PersonBusinessId,
                    UserPath = personBusinessArchive.UserPath,
                    Name = personBusinessArchive.Name,
                    Description = personBusinessArchive.Description,
                    Extension = personBusinessArchive.Extension,
                    Url = personBusinessArchive.Url,
                    CreateDate = personBusinessArchive.CreateDate,
                    Title = personBusinessArchive.Title
                });

                db.Close();
            }

            return id > 0 ? true : false;
        }

        public bool ExcluirPersonBusinessArchive(PersonBusinessArchive personBusinessArchive)
        {
            var excluirPersonBusinessArchive = @"DELETE PersonBusinessArchive WHERE Id = @Id";

            int rowsAffect = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                rowsAffect = db.Execute(excluirPersonBusinessArchive, new
                {
                    Id = personBusinessArchive.Id
                });

                db.Close();
            }

            return rowsAffect > 0 ? true : false;
        }

        public PagedResult<PersonBusinessArchive> GetArchivesByPersonBusinessId(SearchPersonBusinessArchive searchPersonBusinessArchive)
        {
            var result = new PagedResult<PersonBusinessArchive>();
            var countSql = $"SELECT COUNT(*) FROM PersonBusinessArchive PersonBusinessArchive WHERE PersonBusinessId = @PersonBusinessId ";

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                var totalCount = db.ExecuteScalar<int>(countSql, new { PersonBusinessId = searchPersonBusinessArchive.PersonBusinessId });
                var totalPages = (int)Math.Ceiling((double)totalCount / searchPersonBusinessArchive.PageSize);

                var orderBy = $"CreateDate DESC";

                // Define as cláusulas LIMIT e OFFSET para a consulta
                var Limit = searchPersonBusinessArchive.PageSize;
                var Offset = (searchPersonBusinessArchive.PageNumber - 1) * searchPersonBusinessArchive.PageSize;

                // Define a consulta SQL para puxar as entidades
                var sql = $@"SELECT [PersonBusinessArchive].Id,
                                    [PersonBusinessArchive].PersonBusinessId,
                                    [PersonBusinessArchive].UserPath,
                                    [PersonBusinessArchive].Name,
                                    [PersonBusinessArchive].Description,
                                    [PersonBusinessArchive].Extension,
                                    [PersonBusinessArchive].Url,
                                    [PersonBusinessArchive].CreateDate,
                                    [PersonBusinessArchive].Title
                                    FROM [PersonBusinessArchive] 
                                    WHERE [PersonBusinessId] = @PersonBusinessId";

                sql += $" ORDER BY {orderBy} OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY ";

                var entities = db.Query<PersonBusinessArchive>(sql, new { PersonBusinessId = searchPersonBusinessArchive.PersonBusinessId, Offset = Offset, Limit = Limit }).ToList();

                if (entities.Count == 0)
                {
                    return new PagedResult<PersonBusinessArchive>
                    {
                        PageNumber = searchPersonBusinessArchive.PageNumber,
                        PageSize = searchPersonBusinessArchive.PageSize,
                        TotalResults = totalCount,
                        TotalPages = totalPages,
                        Results = entities,
                    };
                }

                result = new PagedResult<PersonBusinessArchive>
                {
                    PageNumber = searchPersonBusinessArchive.PageNumber,
                    PageSize = searchPersonBusinessArchive.PageSize,
                    TotalResults = totalCount,
                    TotalPages = totalPages,
                    Results = entities,
                };

                db.Close();
            }

            return result;
        }

        public PagedResult<Product> GetProductsByCategoryId(SearchPersonBusinessProducts searchPersonBusinessProducts)
        {
            var result = new PagedResult<Product>();
            var countSql = $"SELECT COUNT(*) FROM Product Product WHERE IsDigitalShop = 1 ";

            if (searchPersonBusinessProducts.CategoryId > 0)
                countSql += " AND CategoryId = @CategoryId";

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                var totalCount = db.ExecuteScalar<int>(countSql, new { CategoryId = searchPersonBusinessProducts.CategoryId });
                var totalPages = (int)Math.Ceiling((double)totalCount / searchPersonBusinessProducts.PageSize);

                var orderBy = $"CreateDate DESC";

                // Define as cláusulas LIMIT e OFFSET para a consulta
                var Limit = searchPersonBusinessProducts.PageSize;
                var Offset = (searchPersonBusinessProducts.PageNumber - 1) * searchPersonBusinessProducts.PageSize;

                // Define a consulta SQL para puxar as entidades
                var sql = $@"SELECT [Product].Id,
                                    [Product].Name,
                                    [Product].Value,
                                    [Product].Description,
                                    [Product].AcceptComplement,
                                    [Product].IsComplement,
                                    [Product].MaxComplement,
                                    [Product].Image,
                                    [Product].CreateDate,
                                    [Product].PersonBusinessId
                                    FROM [Product] 
                                    WHERE [IsDigitalShop] = 1 ";

                if(searchPersonBusinessProducts.CategoryId > 0)
                {
                    sql += " AND [CategoryId] = @CategoryId";
                }

                if (searchPersonBusinessProducts.AllProducts)
                {
                    sql += $" ORDER BY {orderBy} ";
                }
                else
                {
                    sql += $" ORDER BY {orderBy} OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY ";
                }

                var entities = db.Query<Product>(sql, new { CategoryId = searchPersonBusinessProducts.CategoryId, Offset = Offset, Limit = Limit }).ToList();

                if (entities.Count == 0)
                {
                    return new PagedResult<Product>
                    {
                        PageNumber = searchPersonBusinessProducts.PageNumber,
                        PageSize = searchPersonBusinessProducts.PageSize,
                        TotalResults = totalCount,
                        TotalPages = totalPages,
                        Results = entities,
                    };
                }

                result = new PagedResult<Product>
                {
                    PageNumber = searchPersonBusinessProducts.PageNumber,
                    PageSize = searchPersonBusinessProducts.PageSize,
                    TotalResults = totalCount,
                    TotalPages = totalPages,
                    Results = entities,
                };

                db.Close();
            }

            return result;
        }

        public Cart GetCart(int PersonId)
        {
            var db = _connectionFactory.GetConnection();
            var result = db.QuerySingleOrDefault<Cart>("SELECT * FROM Cart where PersonId = @PersonId", new { PersonId = PersonId });
            return result;
        }
        public bool InsertCart(Cart cart)
        {
            var insertCart = @"INSERT INTO [Cart] VALUES (@PersonId)";
            int id = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                id = db.Execute(insertCart, new
                {
                    PersonId = cart.PersonId,
                });

                db.Close();
            }

            return id > 0 ? true : false;
        }

        public bool InsertCartProduct(CartProduct cartProduct)
        {
            var insertCartProduct = @"INSERT INTO [CartProduct] VALUES (@ProductId, @CartId)";
            int id = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                id = db.Execute(insertCartProduct, new
                {
                    ProductId = cartProduct.ProductId,
                    CartId = cartProduct.CartId,
                });

                db.Close();
            }

            return id > 0 ? true : false;
        }

        public List<CartProduct> GetCartProducts(int CartId)
        {
            var result = new List<CartProduct>();
            try
            {
                var db = _connectionFactory.GetConnection();

                var sql = @"SELECT CartProduct.*,
                                                        Product.Id,
                                                        Product.Name,
                                                        Product.Value
                                                        FROM CartProduct CartProduct 
                                                        INNER JOIN Product Product
                                                        ON Product.Id = CartProduct.ProductId
                                                        where CartProduct.CartId = @CartId";

                result = db.Query<CartProduct, Product, CartProduct>(sql, (CartProduct, Product) =>
                {
                    CartProduct.Product = Product;
                    return CartProduct;
                }, new { CartId = CartId }, splitOn: "ProductId").ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public bool ExcludeProductCard(int Id)
        {
            var excludeCartProduct = @"DELETE [CartProduct] where Id = @Id";
            int id = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                id = db.Execute(excludeCartProduct, new
                {
                    Id = Id
                });

                db.Close();
            }

            return id > 0 ? true : false;
        }

        public bool SaveCard(Card card)
        {
            var insertCard = @"INSERT INTO [Card] VALUES (@CpfTitular, @Cvv, @NomeTitular, @NumeroCartao, @TelefoneTitular, @Validade, @CreateDate)";
            int id = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                id = db.Execute(insertCard, new
                {
                    CpfTitular = card.CpfTitular,
                    Cvv = card.Cvv,
                    NomeTitular = card.NomeTitular,
                    NumeroCartao = card.NumeroCartao,
                    TelefoneTitular = card.TelefoneTitular,
                    Validade = card.Validade,
                    CreateDate = card.CreateDate
                });

                db.Close();
            }

            return id > 0 ? true : false;
        }

        public int GetCard(string NumeroCartao)
        {
            var db = _connectionFactory.GetConnection();
            var result = db.QuerySingleOrDefault<int>("SELECT TOP 1 Id FROM Card where NumeroCartao = @NumeroCartao", new { NumeroCartao = NumeroCartao });
            return result;
        }

        public bool SaveOrder(Orderr order)
        {
            var insertOrder = @"INSERT INTO [Orderr] VALUES (@PersonId, @OrderNumber, @CreateDate, @UpdateDate, @OrderStatusId, @Observation, @CardId, @FreteService, @DeliveryRangeMin, @DeliveryRangeMax, @FretePrice, @Parcelamento, @ValorTotal, @Bairro, @Cep, @Cidade, @Complemento, @NumeroCasa, @Rua)";
            int id = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                id = db.Execute(insertOrder, new
                {
                    PersonId = order.PersonId,
                    OrderNumber = order.OrderNumber,
                    CreateDate = order.CreateDate,
                    UpdateDate = order.UpdateDate,
                    OrderStatusId = order.OrderStatusId,
                    Observation = order.Observation,
                    CardId = order.CardId,
                    FreteService = order.FreteService,
                    DeliveryRangeMin = order.DeliveryRangeMin,
                    DeliveryRangeMax = order.DeliveryRangeMax,
                    FretePrice = order.FretePrice,
                    Parcelamento = order.Parcelamento,
                    ValorTotal = order.ValorTotal,
                    Bairro = order.Bairro,
                    Cep = order.Cep,
                    Cidade = order.Cidade,
                    Complemento = order.Complemento,
                    NumeroCasa = order.NumeroCasa,
                    Rua = order.Rua
                });

                db.Close();
            }

            return id > 0 ? true : false;
        }

        public Orderr GetOrder(string OrderNumber)
        {
            var db = _connectionFactory.GetConnection();
            var result = db.QuerySingleOrDefault<Orderr>("SELECT * FROM Orderr where OrderNumber = @OrderNumber", new { OrderNumber = OrderNumber });
            return result;
        }

        public bool SaveOrderProduct(OrderProduct orderProduct)
        {
            var insertOrderProduct = @"INSERT INTO [OrderProduct] VALUES (@OrderId, @ProductId)";
            int id = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                id = db.Execute(insertOrderProduct, new
                {
                    OrderId = orderProduct.OrderId,
                    ProductId = orderProduct.ProductId
                });

                db.Close();
            }

            return id > 0 ? true : false;
        }

        public bool DeleteAllCartProduct(int CartId)
        {
            var deleteCartProducts = @"DELETE [CartProduct] WHERE CartId = @CartId";
            int id = 0;

            using (var db = _connectionFactory.GetConnection())
            {
                db.Open();

                id = db.Execute(deleteCartProducts, new
                {
                    CartId = CartId
                });

                db.Close();
            }

            return id > 0 ? true : false;
        }

        public Orderr GetOrder(int Id)
        {
            var db = _connectionFactory.GetConnection();
            var result = db.QuerySingleOrDefault<Orderr>(@"SELECT * 
                                                            FROM Orderr 
                                                            where Id = @Id", new { Id = Id });
            return result;
        }

        public List<OrderProduct> GetOrderProducts(int Id)
        {
            var result = new List<OrderProduct>();
            try
            {
                var db = _connectionFactory.GetConnection();

                var sql = @"SELECT OrderProduct.*,
                                                        Product.Id,
                                                        Product.Name,
                                                        Product.Value
                                                        FROM OrderProduct OrderProduct 
                                                        INNER JOIN Product Product
                                                        ON Product.Id = OrderProduct.ProductId
                                                        where OrderProduct.OrderId = @Id";

                result = db.Query<OrderProduct, Product, OrderProduct>(sql, (OrderProduct, Product) =>
                {
                    OrderProduct.Product = Product;
                    return OrderProduct;
                }, new { Id = Id }, splitOn: "ProductId").ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }

        public List<Orderr> GetOrders(int PersonId)
        {
            var db = _connectionFactory.GetConnection();
            var result = db.Query<Orderr>("SELECT * FROM Orderr where PersonId = @PersonId", new { PersonId = PersonId }).ToList();
            return result;
        }

    }
}
