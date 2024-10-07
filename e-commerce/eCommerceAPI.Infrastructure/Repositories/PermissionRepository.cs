using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Interfaces.Repository;
using eCommerceAPI.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly eCommerceContext _context;

        public PermissionRepository(eCommerceContext context)
        {
            _context = context;
        }

        public async Task<IList<Permission>> GetAll()
        {
            return await _context.Permissions.ToListAsync();
        }
    }
}
