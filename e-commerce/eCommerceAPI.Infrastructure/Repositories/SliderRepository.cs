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
    public class SliderRepository : ISliderRepository
    {
        private readonly eCommerceContext _context;

        public SliderRepository(eCommerceContext context)
        {
            _context = context;
        }

        public async Task AddSliders(IList<Slider> sliders)
        {
            await _context.Sliders.AddRangeAsync(sliders);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSlider(int id)
        {
            Slider slider = await _context.Sliders.FirstAsync(s => s.Id == id);
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Slider>> GetSliders()
        {
            return await _context.Sliders
                .Where(s => !s.IsDeleted)
                .ToListAsync();
        }
    }
}
