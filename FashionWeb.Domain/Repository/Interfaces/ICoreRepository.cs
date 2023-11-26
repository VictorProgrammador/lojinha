using FashionWeb.Domain.Entities;
using FashionWeb.Domain.Entities.Order;
using FashionWeb.Domain.InfraStructure.Request;
using FashionWeb.Domain.InfraStructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Repository.Interfaces
{
    public interface ICoreRepository
    {
        Category GetCategory(int Id);
        IEnumerable<Category> GetAllCategories();
        List<Tag> GetTagsByCategory(int categoryId);
        bool InsertMyBusiness(PersonBusiness personBusiness);
        bool UpdateMyBusiness(PersonBusiness personBusiness);
        List<PersonCategoryTag> GetPersonCategoryTag(int PersonId);
        List<PersonCategoryTag> GetPersonCategoryTag(int PersonId, int CategoryId);
        bool InsertPersonCategoryTag(int PersonId, int CategoryId, int TagId);
        bool DeletePersonCategoryTag(int Id);
        PagedResult<PersonBusiness> GetPersonsBusiness(SearchPersonBusiness searchPersonBusiness);
        PagedResult<PersonBusiness> GetPersonsBusinessByTag(SearchPersonBusiness searchPersonBusiness);
        PagedResult<PersonBusiness> GetPersonsBusinessManagement(SearchPersonBusiness searchPersonBusiness);
        bool UpdatePersonBusinessManagement(PersonBusiness personBusiness);
        PersonBusiness GetPersonBusinessToVisit(int PersonId);
        ClassificationBusiness GetAllClassificationsBusiness(int PersonBusinessId);
        UserClassificationBusiness GetUserClassificationBusiness(UserClassificationBusiness userClassificationBusiness);
        bool InsertUserClassificationBusiness(UserClassificationBusiness userClassificationBusiness);
        bool UpdateUserClassificationBusiness(UserClassificationBusiness userClassificationBusiness);
        PersonBusiness GetPersonBusiness(int Id);
        PersonBusiness GetPersonBusinessByPersonId(int PersonId);
        bool PersonBusinessHasProducts(int PersonBusinessId);
        Product GetProduct(int Id);
        List<Product> GetPersonBusinessComplements(int PersonBusinessId);
        PagedResult<Product> GetProductsByPersonBusinessId(SearchPersonBusinessProducts searchPersonBusinessProducts);
        bool InsertProduct(Product product);
        bool UpdateProduct(Product product);
        bool ExcluirProduto(Product product);
        List<ProductComplement> GetProductComplements(int ProductId);
        bool DeleteProductComplement(int Id);
        bool InsertProductComplement(ProductComplement productComplement);
        PagedResult<Raffle> GetRafflesByPersonBusinessId(SearchPersonBusinessRaffles searchPersonBusinessRaffles);
        IList<Raffle> GetPersonBusinessRafflesActive(int PersonBusinessId);
        Raffle GetRaffle(Guid UniqueId);
        bool InsertRaffle(Raffle raffle);
        bool UpdateRaffle(Raffle raffle);
        bool ExcluirSorteio(Raffle raffle);
        bool ExistsNumberPersonBusinessRaffle(int RaffleId, int PersonBusinessId, int SortedNumber);
        int GetLastNumberPersonBusinessRaffle(int RaffleId, int PersonBusinessId);
        CheckinPersonBusinessRaffle GetCheckinPersonBusinessRaffle(int RaffleId, int PersonBusinessId, string Instagram);
        bool SaveCheckinPersonBusinessRaffles(CheckinPersonBusinessRaffle checkinPersonBusiness);
        List<CheckinPersonBusinessRaffle> GetCheckinsPersonBusinessRaffle(int RaffleId);
        bool DeleteCheckinPersonBusinessRaffle(int RaffleId, int PersonBusinessId);
        bool InsertPersonBusinessArchive(PersonBusinessArchive personBusinessArchive);
        bool ExcluirPersonBusinessArchive(PersonBusinessArchive personBusinessArchive);
        PagedResult<PersonBusinessArchive> GetArchivesByPersonBusinessId(SearchPersonBusinessArchive searchPersonBusinessArchive);
        PagedResult<Product> GetProductsByCategoryId(SearchPersonBusinessProducts searchPersonBusinessProducts);
        Cart GetCart(int PersonId);
        bool InsertCart(Cart cart);
        bool InsertCartProduct(CartProduct cartProduct);
        List<CartProduct> GetCartProducts(int CartId);
        bool ExcludeProductCard(int Id);
        bool SaveCard(Card card);
        int GetCard(string NumeroCartao);
        bool SaveOrder(Orderr order);
        Orderr GetOrder(string OrderNumber);
        bool SaveOrderProduct(OrderProduct orderProduct);
        bool DeleteAllCartProduct(int CartId);
        Orderr GetOrder(int Id);
        List<OrderProduct> GetOrderProducts(int Id);
        List<Orderr> GetOrders(int PersonId);
        OrderStatus GetOrderStatus(int Id);
        PagedResult<Orderr> GetOrders(SearchPersonBusiness filter);
        bool UpdateOrderStatus(Orderr orderr);
        bool UpdateOrderRevelado(int Id);

        bool SaveProductArchive(ProductArchive productArchive);
        bool ExcluirProductArchive(ProductArchive productArchive);
        List<ProductArchive> GetProductArchives(int ProductId);

    }
}
