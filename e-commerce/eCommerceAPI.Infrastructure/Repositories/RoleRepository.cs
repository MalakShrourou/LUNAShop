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
    public class RoleRepository : IRoleRepository
    {
        private readonly eCommerceContext _context;

        public RoleRepository(eCommerceContext context)
        {
            _context = context;
        }

        public async Task<int> Create(Role entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Roles.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task Delete(int id)
        {
            Role role = await GetById(id);
            if (role == null)
                throw new KeyNotFoundException($"Role with ID {id} not found.");

            role.IsDeleted= true;
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Role>> GetAll()
        {
            return await _context.Roles
                .Include(r => r.Permissions)
                .Where(r => !r.IsDeleted)
                .ToListAsync();
        }

        public async Task<Role> GetById(int id)
        {
            return  await _context.Roles
                .Include(r => r.Permissions)
                .Where(r => !r.IsDeleted)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task Update(Role entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (GetById(entity.Id) == null)
                throw new KeyNotFoundException($"Role with ID {entity.Id} not found.");

            _context.Roles.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AssignPermissions(int[] permissionIds,int roleId)
        {
            Role role = await GetById(roleId);
            IList<Permission> permissions = new List<Permission>();

            foreach (int id in permissionIds)
            {
                Permission permission = await _context.Permissions.FirstAsync(p => p.Id == id);
                permissions.Add(permission);                
            }

            role.Permissions = permissions;
            await _context.SaveChangesAsync();
        }
    }
}
