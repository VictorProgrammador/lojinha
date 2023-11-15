using FashionWeb.Domain.Entities;
using FashionWeb.Domain.Entities.Order;
using FashionWeb.Domain.InfraStructure;
using FashionWeb.Domain.InfraStructure.Request;
using FashionWeb.Domain.InfraStructure.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.BusinessRules
{
    public interface ICoreBusinessRules
    {
        void ConfirmarEmail(Guid AspNetUserId);
        bool VerifyEmailConfirmed(string Id);
        IEnumerable<Category> GetAllCategories();
        bool SaveMyBusiness(PersonBusiness personBusiness);
        PagedResult<PersonBusiness> GetPersonsBusiness(SearchPersonBusiness searchPersonBusiness);
        PagedResult<PersonBusiness> GetPersonsBusinessManagement(SearchPersonBusiness searchPersonBusiness);
        bool SavePersonBusinessManagement(PersonBusiness personBusiness, string htmltemplate);
        UserInfo GetUserByPersonId(int PersonId);
        PersonBusiness GetPersonBusinessToVisit(int PersonId);
        int GetRateProfileByPerson(UserClassificationBusiness userClassificationBusiness);
        bool SaveUserClassificationBusiness(UserClassificationBusiness userClassificationBusiness);
        List<Product> GetPersonBusinessComplements(int PersonId);
        PersonBusiness GetPersonBusinessWithProducts(SearchPersonBusinessProducts searchPersonBusinessProducts);
        bool SaveProduct(Product product);
        bool ExcluirProduto(Product product);
        PersonBusiness GetPersonBusinessWithRaffles(SearchPersonBusinessRaffles searchPersonBusinessRaffles);
        PagedResult<Raffle> GetPersonBusinessRafflesPaged(SearchPersonBusinessRaffles searchPersonBusinessRaffles);
        IList<Raffle> GetPersonBusinessRafflesActive(int PersonBusinessId);
        bool SaveRaffle(Raffle raffle);
        bool ExcluirSorteio(Raffle raffle);
        ResponseSaveCheckinPersonBusiness SaveCheckinPersonBusinesRaffles(CheckinPersonBusinessRaffle checkinPersonBusiness);
        List<CheckinPersonBusinessRaffle> GetCheckinsPersonBusinessRaffle(Guid UniqueId);
        bool SavePersonBusinessArchive(PersonBusinessArchive personBusinessArchive);
        bool ExcluirPersonBusinessArchive(PersonBusinessArchive personBusinessArchive);
        PersonBusiness GetPersonBusinessWithArchives(SearchPersonBusinessArchive searchPersonBusinessArchive);
        PagedResult<PersonBusinessArchive> GetPersonBusinessArchives(SearchPersonBusinessArchive searchPersonBusinessArchive);
        PagedResult<Product> GetProductsByCategoryId(SearchPersonBusinessProducts searchPersonBusinessProducts);
        Product GetProduct(int Id);
        Cart GetCart(int PersonId);
        bool InsertCart(Cart cart);
        bool InsertCartProduct(CartProduct cartProduct);
        List<CartProduct> GetCartProducts(int CartId);
        bool ExcludeProductCard(int Id);
        int SaveOrder(Orderr orderr);
        Orderr GetOrder(int Id);
        List<Orderr> GetOrders(int PersonId);
    }
}
