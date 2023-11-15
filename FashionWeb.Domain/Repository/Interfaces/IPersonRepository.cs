using FashionWeb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Repository.Interfaces
{
    public interface IPersonRepository
    {
        PersonBusiness GetPersonBusiness(int PersonId);
        Person Get(int id);
        int Create(Person entity);
        bool Update(Person entity);
        bool UpdatePersonPhoto(int Id, string PhotoUrl);
        bool UpdatePersonBannerPhoto(int Id, string BannerUrl);
    }
}
