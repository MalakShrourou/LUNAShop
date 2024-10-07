using eCommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Interfaces.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {
         Task<(IList<Product>, int)> GetProductsByGategory(int categoryId, string? sortBy, int? brandId,
                                                          double? minPrice, double? maxPrice,
                                                          int pageNumber, int pageSize, string userLang);
         Task ChangeActiveStatus(int categoryId);
         Task<IList<Category>> GetActive();

    }
}
