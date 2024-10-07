using eCommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Domain.Interfaces.Repository
{
    public interface ISliderRepository
    {
        Task<IList<Slider>> GetSliders();
        Task AddSliders(IList<Slider> sliders);
        Task DeleteSlider(int id);

    }
}
