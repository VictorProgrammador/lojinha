using FashionWeb.Domain.Repository;
using FashionWeb.Domain.Entities;
using FashionWeb.Domain.Repository.Interfaces;
using System.Transactions;
using System;
using System.Linq;

namespace FashionWeb.Domain.BusinessRules
{
    public class PersonBusinessRules : IPersonBusinessRules
    {
        private readonly IPersonRepository _personRepository;
        private readonly IUserRepository _userInfoRepository;
        private readonly ICoreRepository _coreRepository;
        public PersonBusinessRules(IPersonRepository personRepository,
            IUserRepository userInfoRepository,
            ICoreRepository coreRepository)
        {
            _personRepository = personRepository;
            _userInfoRepository = userInfoRepository;
            _coreRepository = coreRepository;
        }
        public void InsertUser(UserInfo userInfo)
        {
            using(var scope = new TransactionScope())
            {
                int personId = _personRepository.Create(new Person() { 
                    Name = userInfo.Profile.Name
                });

                userInfo.PersonId = personId;

                _userInfoRepository.Create(userInfo);
                scope.Complete();
            }
        }
        public bool UpdateUser(UserInfo userInfo)
        {
            bool update = false;

            using (var scope = new TransactionScope())
            {
                update = this._personRepository.Update(userInfo.Profile);
                scope.Complete();
            }

            return update;
        }

        public UserInfo GetUser(Guid AspNetUserId)
        {
            UserInfo user = _userInfoRepository.Get(AspNetUserId);
            user.Profile = _personRepository.Get(user.PersonId);
            user.Profile.PersonBusiness = _personRepository.GetPersonBusiness(user.PersonId);

            //Se a pessoa tiver um negócio, e esse negócio tiver categoria
            if(user.Profile.PersonBusiness != null &&
               user.Profile.PersonBusiness.CategoryId != null)
            {
                var PersonCategoryTags = this._coreRepository.GetPersonCategoryTag(user.PersonId, user.Profile.PersonBusiness.CategoryId.Value);

                if(PersonCategoryTags != null && PersonCategoryTags.Count() > 0)
                    user.Profile.PersonBusiness.Category.Tags = new System.Collections.Generic.List<Tag>();

                foreach (var PersonCategoryTag in PersonCategoryTags)
                {
                    user.Profile.PersonBusiness.Category.Tags.Add(PersonCategoryTag.Tag);
                }

            }

            return user;
        }

        public bool UpdatePersonPhoto(int Id, string PhotoUrl)
        {
            bool update = false;

            using (var scope = new TransactionScope())
            {
                update = this._personRepository.UpdatePersonPhoto(Id, PhotoUrl);
                scope.Complete();
            }

            return update;
        }

        public bool UpdatePersonBannerPhoto(int Id, string BannerUrl)
        {
            bool update = false;

            using (var scope = new TransactionScope())
            {
                update = this._personRepository.UpdatePersonBannerPhoto(Id, BannerUrl);
                scope.Complete();
            }

            return update;
        }

        public UserInfo GetUserTiny(Guid AspNetUserId)
        {
            return _userInfoRepository.Get(AspNetUserId);
        }
    }
}
