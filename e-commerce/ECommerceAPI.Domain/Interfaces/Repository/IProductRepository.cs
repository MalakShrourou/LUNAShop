using eCommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Interfaces.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IList<Product>> Top10InCategory(int productId);
        Task<IList<Product>> GetPopularProducts();
        Task<(IList<Product>, int)> SearchProducts(string query, int? categoryId, string? sortBy, int? brandId,
                                                          double? minPrice, double? maxPrice,
                                                          int pageNumber, int pageSize, string userLang);
    }
}
