using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Repository.Interfaces
{
    public interface IGenericRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        int Create(T entity);
        bool Update(T entity);
        void Delete(int id);
    }
}
