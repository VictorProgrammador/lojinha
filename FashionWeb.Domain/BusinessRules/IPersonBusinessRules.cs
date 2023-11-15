using FashionWeb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.BusinessRules
{
    public interface IPersonBusinessRules
    {
        void InsertUser(UserInfo userInfo);
        UserInfo GetUser(Guid AspNetUserId);
        bool UpdateUser(UserInfo userInfo);
        bool UpdatePersonPhoto(int Id, string PhotoUrl);
        bool UpdatePersonBannerPhoto(int Id, string BannerUrl);
        UserInfo GetUserTiny(Guid AspNetUserId);
    }
}
