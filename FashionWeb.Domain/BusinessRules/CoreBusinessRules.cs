using FashionWeb.Domain.Entities;
using FashionWeb.Domain.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Transactions;
using System.Linq;
using FashionWeb.Domain.InfraStructure.Request;
using FashionWeb.Domain.InfraStructure.Response;
using FashionWeb.Domain.Entities.Order;
using FashionWeb.Domain.Utils;

namespace FashionWeb.Domain.BusinessRules
{
    public class CoreBusinessRules : ICoreBusinessRules
    {

        private readonly IUserRepository _userInfoRepository;
        private readonly ICoreRepository _coreRepository;
        public CoreBusinessRules(IUserRepository userInfoRepository, ICoreRepository coreRepository)
        {
            _userInfoRepository = userInfoRepository;
            _coreRepository = coreRepository;
        }
        public void ConfirmarEmail(Guid AspNetUserId)
        {
            using (var scope = new TransactionScope())
            {
                _userInfoRepository.EmailConfirmed(AspNetUserId.ToString());
                scope.Complete();
            }
        }

        public bool VerifyEmailConfirmed(string Id)
        {
            return _userInfoRepository.VerifyEmailConfirmed(Id);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            var categories = _coreRepository.GetAllCategories();
            
            foreach(var category in categories)
            {
                category.Tags = _coreRepository.GetTagsByCategory(category.Id);
            }

            return categories; 
        }

        public bool SaveMyBusiness(PersonBusiness personBusiness)
        {
            bool savedPersonBusiness = false;

            var originalPersonBusiness = this._coreRepository.GetPersonBusinessByPersonId(personBusiness.PersonId);

            //Cadastro já existe.
            if (originalPersonBusiness != null && originalPersonBusiness.Id > 0)
                personBusiness.Id = originalPersonBusiness.Id;

            if (personBusiness.Id == 0)
            {
                savedPersonBusiness = this._coreRepository.InsertMyBusiness(personBusiness);
            }
            else
            {
                savedPersonBusiness = this._coreRepository.UpdateMyBusiness(personBusiness);
            }

            //Aqui eu salvo as tags do usuário!
            if(personBusiness.Category != null && 
                personBusiness.Category.Tags != null)
            {
                var personCategoryTags = this._coreRepository.GetPersonCategoryTag(personBusiness.PersonId);

                //Precisa excluir da PersonCategoryTag todas as tags anteriores que não fazem parte das novas tags.
                foreach(var personCategoryTag in personCategoryTags)
                {
                    if(personBusiness.Category.Tags.Where(x=> x.Id == personCategoryTag.Tag.Id).Count() == 0)
                    {
                        //Remove a tag da tabela de vinculo com a person.
                        this._coreRepository.DeletePersonCategoryTag(personCategoryTag.Id);
                    }
                }

                //Adicionando tags que não fazem parte da PersonCategoryTag, ou seja, não estavam vinculadas.
                foreach (var tag in personBusiness.Category.Tags)
                {
                    if (personCategoryTags.Where(x => x.Tag.Id == tag.Id).Count() == 0)
                    {
                        this._coreRepository.InsertPersonCategoryTag(personBusiness.PersonId, tag.CategoryId, tag.Id);
                    }
                }
            }

            return savedPersonBusiness;
        }
        public PagedResult<PersonBusiness> GetPersonsBusiness(SearchPersonBusiness searchPersonBusiness)
        {
            var result = new PagedResult<PersonBusiness>();

            if(searchPersonBusiness.TagId == null || searchPersonBusiness.TagId == 0)
            {
                result = this._coreRepository.GetPersonsBusiness(searchPersonBusiness);
            }
            else
            {
                result = this._coreRepository.GetPersonsBusinessByTag(searchPersonBusiness);
            }



            if(result.Results != null && result.Results.Count() > 0)
            {
                //List<PersonBusiness> data = new List<PersonBusiness>();
                //if(searchPersonBusiness.PageNumber <= 1)
                //{
                //    data = result.Results.Take(searchPersonBusiness.PageSize).ToList();
                //}
                //else
                //{
                //    data = result.Results.Skip((searchPersonBusiness.PageNumber - 1) * searchPersonBusiness.PageSize).Take(searchPersonBusiness.PageSize).ToList();
                //}

                //if(data.Count() == 0)
                //{
                //    data = result.Results.Take(searchPersonBusiness.PageSize).ToList();
                //}

                //result.Results = data;

                foreach (var item in result.Results)
                {
                    item.HasProducts = this._coreRepository.PersonBusinessHasProducts(item.Id);

                    ClassificationBusiness classificationBusiness = this._coreRepository.GetAllClassificationsBusiness(item.Id);

                    if (classificationBusiness != null)
                    {
                        item.TotalAvaliacoes = classificationBusiness.TotalAvaliacoes;
                        item.SomaAvaliacoes = classificationBusiness.SomaAvaliacoes;

                        if (item.TotalAvaliacoes > 0 && item.SomaAvaliacoes > 0)
                        {
                            item.mediaAvaliacoes = item.SomaAvaliacoes / item.TotalAvaliacoes;
                        }
                    }
                    var PersonCategoryTags = this._coreRepository.GetPersonCategoryTag(item.PersonId, item.CategoryId.Value);

                    if(PersonCategoryTags != null && PersonCategoryTags.Count() > 0)
                    {
                        item.TagPrincipal = PersonCategoryTags.First().Tag.Name;
                    }
                }
            }

            return result;
        }

        public PagedResult<PersonBusiness> GetPersonsBusinessManagement(SearchPersonBusiness searchPersonBusiness)
        {
            return this._coreRepository.GetPersonsBusinessManagement(searchPersonBusiness);
        }

        public bool SavePersonBusinessManagement(PersonBusiness personBusiness, string htmltemplate)
        {
            return this._coreRepository.UpdatePersonBusinessManagement(personBusiness);

            //Notificar o usuário de que o perfil dele foi aprovado.
            //var original = this._coreRepository.GetPersonBusiness(personBusiness.Id);

            //var userInfo = this._userInfoRepository.GetUserByPersonId(personBusiness.PersonId);
            //string mail = this._userInfoRepository.GetAspNetUser(userInfo.AspNetUserId);

            //if (personBusiness.Approved == true &&
            //    original.Approved == false)
            //{
            //    string content = @"<p>Seu perfil acaba de ser aprovado pelo administrador!</p>";

            //    if(personBusiness.IsVisible == false)
            //    {
            //        content += "<p>Faça login e habilite 'Perfil Vísivel', para seu perfil ser visto por todos!</p>";                   
            //    }

            //    Utils.SendEmail.Send(htmltemplate, content, "Perfil Aprovado", mail);
            //}

            //string welcomeSubscribe = @"";
            //string subject = "";

            //if(personBusiness.Approved && personBusiness.IsSubscriber == true &&
            //    original.IsSubscriber == false)
            //{
            //    welcomeSubscribe = $@"<p>Parabéns, você agora é assinante da plataforma!</p>
            //                         <p>Agora você pode ser avaliado, faz parte dos assinantes e pode cadastrar produtos.</p>
            //                         <p>Sua assinatura expira em: {personBusiness.LimitSubscription}.</p>";

            //    subject = "Parabéns por se tornar nosso parceiro";
            //}
            //else if(personBusiness.Approved && original.IsSubscriber &&
            //    !personBusiness.IsSubscriber)
            //{
            //    welcomeSubscribe = $@"<p>Sua assinatura expirou, porém seu cadastro no site continua com acesso às funcionalidades básicas.</p>
            //                         <p>Sentimos muito, sua assinatura foi expirada, para renovar entre em contato conosco.</p>
            //                         <p>Sua assinatura expirou em: {personBusiness.LimitSubscription}.</p>";

            //    subject = "Assinatura expirou!";
            //}
            //else
            //{
            //    welcomeSubscribe = $@"<p>Sabia que sendo assinante da plataforma você adquire vários benefícios que podem alavancar seu negócio na cidade?</p>
            //                         <p>Isso inclui poder receber avaliações, cadastrar produtos, fazer parte dos assinantes tendo destaque na lista, entre diversos futuros benefícios.</p>";

            //    subject = "Conheça nossos benefícios";
            //}

            //Utils.SendEmail.Send(htmltemplate, welcomeSubscribe, subject, mail);

        }
        public UserInfo GetUserByPersonId(int PersonId)
        {
            return this._userInfoRepository.GetUserByPersonId(PersonId);
        }

        public PersonBusiness GetPersonBusinessToVisit(int PersonId)
        {
            var personBusiness = this._coreRepository.GetPersonBusinessToVisit(PersonId);

            if(personBusiness != null && personBusiness.Id > 0)
            {
                ClassificationBusiness classificationBusiness = this._coreRepository.GetAllClassificationsBusiness(personBusiness.Id);

                if (classificationBusiness != null)
                {
                    personBusiness.TotalAvaliacoes = classificationBusiness.TotalAvaliacoes;
                    personBusiness.SomaAvaliacoes = classificationBusiness.SomaAvaliacoes;

                    if (personBusiness.TotalAvaliacoes > 0 && personBusiness.SomaAvaliacoes > 0)
                    {
                        personBusiness.mediaAvaliacoes = personBusiness.SomaAvaliacoes / personBusiness.TotalAvaliacoes;
                    }
                }

                if (personBusiness.IsSubscriber)
                {
                    personBusiness.HasProducts = this._coreRepository.PersonBusinessHasProducts(personBusiness.Id);
                }
            }

            return personBusiness;
        }
        public int GetRateProfileByPerson(UserClassificationBusiness userClassificationBusiness)
        {
            UserClassificationBusiness original = this._coreRepository.GetUserClassificationBusiness(userClassificationBusiness);
            if (original != null)
                return original.Rating;

            return 0;
        }
        public bool SaveUserClassificationBusiness(UserClassificationBusiness userClassificationBusiness)
        {
            bool result = false;
            UserClassificationBusiness original = this._coreRepository.GetUserClassificationBusiness(userClassificationBusiness);

            if (original == null || original.Id == 0)
            {
                result = this._coreRepository.InsertUserClassificationBusiness(userClassificationBusiness);
            }
            else
            {
                userClassificationBusiness.Id = original.Id;
                result = this._coreRepository.UpdateUserClassificationBusiness(userClassificationBusiness);
            }

            return result;
        }

        public List<Product> GetPersonBusinessComplements(int PersonId)
        {
            var personBusiness = this._coreRepository.GetPersonBusinessByPersonId(PersonId);

            var complements = this._coreRepository.GetPersonBusinessComplements(personBusiness.Id);
            return complements;
        }
        public PersonBusiness GetPersonBusinessWithProducts(SearchPersonBusinessProducts searchPersonBusinessProducts)
        {
            var personBusiness = this._coreRepository.GetPersonBusinessByPersonId(searchPersonBusinessProducts.PersonId);

            searchPersonBusinessProducts.PersonBusinessId = personBusiness.Id;
            personBusiness.Products = this._coreRepository.GetProductsByPersonBusinessId(searchPersonBusinessProducts);

            if(personBusiness.Products.Results != null && personBusiness.Products.Results.Count() > 0)
            {
                foreach (var product in personBusiness.Products.Results)
                {
                    if (product.AcceptComplement)
                    {
                        var productComplements = this._coreRepository.GetProductComplements(product.Id);
                        product.Complements = new List<Product>();

                        foreach (var productComplement in productComplements)
                        {
                            product.Complements.Add(new Product()
                            {
                                Id = productComplement.Complement.Id,
                                Name = productComplement.Complement.Name,
                                Value = productComplement.Complement.Value
                            });
                        }
                    }
                }
            }

            return personBusiness;
        }

        public bool SaveProduct(Product product)
        {
            if(product != null && product.Id == 0)
            {
                product.CreateDate = DateTime.Now;
                this._coreRepository.InsertProduct(product);
            }
            else
            {
                this._coreRepository.UpdateProduct(product);
            }

            //Cuidando dos complementos do produto !
            var productComplements = this._coreRepository.GetProductComplements(product.Id);

            //Deleta os complementos existentes caso na nova lista nao tiver nenhum.
            if(product.Complements == null || product.Complements.Count() == 0)
            {
                foreach(var productComplement in productComplements)
                {
                    this._coreRepository.DeleteProductComplement(productComplement.Id);
                }
            }
            else
            {
                //Verificando existentes
                foreach(var productComplement in productComplements)
                {
                    //O complemento antigo nao está na lista atual
                    if(product.Complements.Where(x=> x.Id == productComplement.Complement.Id).Count() == 0)
                    {
                        //Removendo ele 
                        this._coreRepository.DeleteProductComplement(productComplement.Id);
                    }
                }
                
                foreach(var complement in product.Complements)
                {
                    //O complemento atual nao está na lista existente
                    if (productComplements.Where(x => x.Complement.Id == complement.Id).Count() == 0)
                    {
                        ProductComplement productComplement = new ProductComplement()
                        {
                            ComplementId = complement.Id,
                            ProductId = product.Id
                        };

                        this._coreRepository.InsertProductComplement(productComplement);
                    }
                }


            }


            return true;
        }

        public bool ExcluirProduto(Product product)
        {
            if(product.Id > 0)
            {
                //Cuidando dos complementos do produto !
                var productComplements = this._coreRepository.GetProductComplements(product.Id);

                //Deleta os complementos existentes caso na nova lista nao tiver nenhum.
                if (productComplements != null && productComplements.Count() > 0)
                {
                    foreach (var productComplement in productComplements)
                    {
                        this._coreRepository.DeleteProductComplement(productComplement.Id);
                    }
                }

            }

            return this._coreRepository.ExcluirProduto(product);
        }

        public PersonBusiness GetPersonBusinessWithRaffles(SearchPersonBusinessRaffles searchPersonBusinessRaffles)
        {
            var personBusiness = this._coreRepository.GetPersonBusinessByPersonId(searchPersonBusinessRaffles.PersonId);

            searchPersonBusinessRaffles.PersonBusinessId = personBusiness.Id;
            personBusiness.Raffles = this._coreRepository.GetRafflesByPersonBusinessId(searchPersonBusinessRaffles);
            return personBusiness;
        }

        public PagedResult<Raffle> GetPersonBusinessRafflesPaged(SearchPersonBusinessRaffles searchPersonBusinessRaffles)
        {
            PagedResult<Raffle> raffles = this._coreRepository.GetRafflesByPersonBusinessId(searchPersonBusinessRaffles);
            return raffles;
        }
        public IList<Raffle> GetPersonBusinessRafflesActive(int PersonBusinessId)
        {
            return this._coreRepository.GetPersonBusinessRafflesActive(PersonBusinessId);
        }
        public bool SaveRaffle(Raffle raffle)
        {
            if (raffle != null && raffle.Id == 0)
            {
                raffle.CreateDate = DateTime.Now;
                this._coreRepository.InsertRaffle(raffle);
            }
            else
            {
                this._coreRepository.UpdateRaffle(raffle);
            }
            return true;
        }
        public bool ExcluirSorteio(Raffle raffle)
        {
            //Primeiro tem que deletar CheckinPersonBusinessRaffle
            this._coreRepository.DeleteCheckinPersonBusinessRaffle(raffle.Id, raffle.PersonBusinessId);


            return this._coreRepository.ExcluirSorteio(raffle);
        }
        public ResponseSaveCheckinPersonBusiness SaveCheckinPersonBusinesRaffles(CheckinPersonBusinessRaffle checkinPersonBusiness)
        {
            ResponseSaveCheckinPersonBusiness responseSaveCheckinPersonBusiness = new ResponseSaveCheckinPersonBusiness();

            var raffles = this.GetPersonBusinessRafflesActive(checkinPersonBusiness.PersonBusinessId);

            if(raffles == null || raffles.Count() == 0)
            {
                responseSaveCheckinPersonBusiness.success = false;
                responseSaveCheckinPersonBusiness.message = "Esse perfil não tem sorteios ativos.";
            }
            else
            {
                responseSaveCheckinPersonBusiness.PedrasSorteadas = new Dictionary<string, int>();
                foreach (var raffle in raffles)
                {
                    var existCheckinPersonBusinessRaffle = this._coreRepository.GetCheckinPersonBusinessRaffle(raffle.Id, checkinPersonBusiness.PersonBusinessId, checkinPersonBusiness.Instagram);
                    int lastNumber = this._coreRepository.GetLastNumberPersonBusinessRaffle(raffle.Id, checkinPersonBusiness.PersonBusinessId);

                    if (lastNumber == raffle.QtdNumber)
                    {
                        raffle.IsClosed = true;
                        this._coreRepository.UpdateRaffle(raffle);
                        responseSaveCheckinPersonBusiness.PedrasSorteadas.Add(raffle.Name, 0);
                        continue;
                    }

                    if (existCheckinPersonBusinessRaffle == null || existCheckinPersonBusinessRaffle.Id == 0)
                    {
                        bool savedSuccess = false;
                        checkinPersonBusiness.RaffleId = raffle.Id;

                        int newNumber = lastNumber + 1;
                        while (!savedSuccess)
                        {

                            if (!this._coreRepository.ExistsNumberPersonBusinessRaffle(raffle.Id, checkinPersonBusiness.PersonBusinessId, newNumber))
                            {
                                checkinPersonBusiness.SortedNumber = newNumber;
                                savedSuccess = this._coreRepository.SaveCheckinPersonBusinessRaffles(checkinPersonBusiness);

                                if (savedSuccess)
                                {
                                    responseSaveCheckinPersonBusiness.PedrasSorteadas.Add(raffle.Name, checkinPersonBusiness.SortedNumber);
                                }
                            }

                            newNumber++;
                        }
                    }
                    else
                    {
                        responseSaveCheckinPersonBusiness.PedrasSorteadas.Add(raffle.Name, existCheckinPersonBusinessRaffle.SortedNumber);
                    }
                }

                responseSaveCheckinPersonBusiness.success = true;
                responseSaveCheckinPersonBusiness.message = "Preste atenção na mensagem abaixo!";
            }


            return responseSaveCheckinPersonBusiness;
        }

        public List<CheckinPersonBusinessRaffle> GetCheckinsPersonBusinessRaffle(Guid UniqueId)
        {
            Raffle raffle = this._coreRepository.GetRaffle(UniqueId);
            var data = new List<CheckinPersonBusinessRaffle>();

            if(raffle != null && raffle.Id > 0)
            {
                data = this._coreRepository.GetCheckinsPersonBusinessRaffle(raffle.Id);
            }

            return data;
        }

        public bool SavePersonBusinessArchive(PersonBusinessArchive personBusinessArchive)
        {
            if (personBusinessArchive != null && personBusinessArchive.Id == 0)
            {
                personBusinessArchive.CreateDate = DateTime.Now;
                this._coreRepository.InsertPersonBusinessArchive(personBusinessArchive);
            }
            else
            {
                //Atualiza informações
            }

            return true;
        }

        public bool ExcluirPersonBusinessArchive(PersonBusinessArchive personBusinessArchive)
        {
            return this._coreRepository.ExcluirPersonBusinessArchive(personBusinessArchive);
        }
        public PersonBusiness GetPersonBusinessWithArchives(SearchPersonBusinessArchive searchPersonBusinessArchive)
        {
            var personBusiness = this._coreRepository.GetPersonBusinessByPersonId(searchPersonBusinessArchive.PersonId);

            searchPersonBusinessArchive.PersonBusinessId = personBusiness.Id;

            personBusiness.PersonBusinessArchive = this._coreRepository.GetArchivesByPersonBusinessId(searchPersonBusinessArchive);

            return personBusiness;
        }

        public PagedResult<PersonBusinessArchive> GetPersonBusinessArchives(SearchPersonBusinessArchive searchPersonBusinessArchive)
        {
            return this._coreRepository.GetArchivesByPersonBusinessId(searchPersonBusinessArchive);
        }

        public PagedResult<Product> GetProductsByCategoryId(SearchPersonBusinessProducts searchPersonBusinessProducts)
        {
            return this._coreRepository.GetProductsByCategoryId(searchPersonBusinessProducts);
        }

        public Product GetProduct(int Id)
        {
            return this._coreRepository.GetProduct(Id);
        }

        public Cart GetCart(int PersonId)
        {
            return this._coreRepository.GetCart(PersonId);
        }
        public bool InsertCart(Cart cart)
        {
            return this._coreRepository.InsertCart(cart);
        }

        public bool InsertCartProduct(CartProduct cartProduct)
        {
            return this._coreRepository.InsertCartProduct(cartProduct);
        }

        public List<CartProduct> GetCartProducts(int CartId)
        {
            return this._coreRepository.GetCartProducts(CartId);
        }

        public bool ExcludeProductCard(int Id)
        {
            return this._coreRepository.ExcludeProductCard(Id);
        }

        public int SaveOrder(Orderr orderr)
        {
            orderr.CreateDate = DateTime.Now;
            orderr.UpdateDate = DateTime.Now;
            orderr.OrderStatusId = (int)OrderStatus.OrderStatusType.Realizado;
            orderr.Observation = "Seu pedido foi recebido, aguarde à aprovação pela administradora do cartão de crédito.";
            orderr.OrderNumber = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            orderr.Revelado = false;

            orderr.Card.CreateDate = DateTime.Now;

            //Salvando o Card.
            this._coreRepository.SaveCard(orderr.Card);

            orderr.CardId = this._coreRepository.GetCard(orderr.Card.NumeroCartao);

            //Salvando o Pedido.
            this._coreRepository.SaveOrder(orderr);

            Orderr inserted = this._coreRepository.GetOrder(orderr.OrderNumber);

            //Vinculando produtos no pedido .
            foreach (var orderProduct in orderr.OrderProducts)
            {
                orderProduct.OrderId = inserted.Id;

                //Salvando produtos no pedido.
                this._coreRepository.SaveOrderProduct(orderProduct);
            }

            //Limpar carrinho.
            this._coreRepository.DeleteAllCartProduct(orderr.CartId);

            return inserted.Id;
        }

        public Orderr GetOrder(int Id)
        {
            Orderr order = this._coreRepository.GetOrder(Id);
            order.OrderProducts = this._coreRepository.GetOrderProducts(Id);
            order.OrderStatus = this._coreRepository.GetOrderStatus(order.OrderStatusId);

            return order;
        }

        public List<Orderr> GetOrders(int PersonId)
        {
            var orders = this._coreRepository.GetOrders(PersonId);

            if(orders != null && orders.Count() > 0)
            {
                foreach(var order in orders)
                    order.OrderStatus = this._coreRepository.GetOrderStatus(order.OrderStatusId);
            }

            return orders;
        }

        public PagedResult<Orderr> GetOrders(SearchPersonBusiness filter)
        {
            var orders = this._coreRepository.GetOrders(filter);

            if (orders != null && orders.Results != null && orders.Results.Count() > 0)
            {
                foreach (var order in orders.Results)
                {
                    var newOrder = this._coreRepository.GetOrder(order.Id);
                    order.OrderStatus = this._coreRepository.GetOrderStatus(newOrder.OrderStatusId);
                    order.Revelado = newOrder.Revelado;
                }
            }

            return orders;
        }

        public bool UpdateOrderStatus(Orderr orderr)
        {        
            return this._coreRepository.UpdateOrderStatus(orderr);
        }

        public bool UpdateOrderRevelado(int Id)
        {
            return this._coreRepository.UpdateOrderRevelado(Id);
        }

        public bool SaveProductArchive(ProductArchive productArchive)
        {
            productArchive.CreateDate = DateTime.Now;
            return this._coreRepository.SaveProductArchive(productArchive);
        }
        public bool ExcluirProductArchive(ProductArchive productArchive)
        {
            return this._coreRepository.ExcluirProductArchive(productArchive);
        }

        public List<ProductArchive> GetProductArchives(int ProductId)
        {
            return this._coreRepository.GetProductArchives(ProductId);
        }

        public List<SubCategory> GetSubCategories(int? CategoryId)
        {
            return this._coreRepository.GetSubCategories(CategoryId);
        }

        public List<ProductType> GetProductTypes(int? SubCategoryId)
        {
            return this._coreRepository.GetProductTypes(SubCategoryId);
        }

        public List<Tamanho> GetTamanho()
        {
            return this._coreRepository.GetTamanho();
        }

    }
}
