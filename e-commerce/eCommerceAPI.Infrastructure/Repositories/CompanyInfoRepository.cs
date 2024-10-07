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
    public class CompanyInfoRepository : ICompanyInfoRepository
    {
        private readonly eCommerceContext _context;

        public CompanyInfoRepository(eCommerceContext context)
        {
            _context = context;
        }

        public async Task<CompanyInfo> Get()
        {
            return await _context.CompanyInfo.FirstAsync();
        }

        public async Task Update(CompanyInfo entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.CompanyInfo.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
