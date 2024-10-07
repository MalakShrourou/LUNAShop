using eCommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Interfaces.Repository
{
    public interface IWishlistRepository : IRepository<Wishlist>
    {
         Task<IList<Wishlist>> GetCustomerWishlists(int customerId);
    }
}
