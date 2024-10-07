using eCommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Interfaces.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IList<T>> GetAll();
        Task<T> GetById(int id);
        Task<int> Create(T entity);
        Task Update(T entity);
        Task Delete(int id);
    }
}
