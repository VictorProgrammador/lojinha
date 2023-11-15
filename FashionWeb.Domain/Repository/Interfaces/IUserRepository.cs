using FashionWeb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Repository.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<UserInfo> GetAll();
        UserInfo Get(Guid id);
        UserInfo GetUserByPersonId(int PersonId);
        int Create(UserInfo entity);
        void Update(int id);
        void Delete(int id);
        void EmailConfirmed(string Id);
        bool VerifyEmailConfirmed(string Id);

        string GetAspNetUser(Guid Id);
    }
}
