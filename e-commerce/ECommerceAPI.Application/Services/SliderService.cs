using eCommerceAPI.Application.Commands;
using eCommerceAPI.Application.Models;
using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Interfaces.Repository;
using eCommerceAPI.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Services
{
    public class SliderService
    {
        private readonly ISliderRepository _repository;
        private readonly IImageService _imageService;

        public SliderService(ISliderRepository repository, IImageService imageService)
        {
            _repository = repository;
            _imageService = imageService;
        }

        public async Task<IList<SliderModel>> GetSliders()
        {
            IList<Slider> sliders = await _repository.GetSliders();
            IList<SliderModel> models = new List<SliderModel>();
            foreach (var slider in sliders)
            {
                string imageBase64 = await _imageService.GetImageBase64(slider.ImageUrl);

                models.Add(new SliderModel
                {
                    Id = slider.Id,
                    CreatedOn=slider.CreatedOn,
                    ImageBase64 = imageBase64,
                    UpdatedOn = slider.UpdatedOn,
                });
            }
                return models; 
        }

        public async Task DeleteSlider(int id)
        {
            await _repository.DeleteSlider(id);
        }

        public async Task AddSliders(IList<SliderCommand> commands)
        {
            IList<Slider> sliders = new List<Slider>();
            foreach (var command in commands)
            {
                string image = await _imageService.SaveImage(command.ImageBase64);
                sliders.Add( new Slider()
                {
                    ImageUrl = image,
                    CreatedOn = DateTime.Now
                });
            }
            await _repository.AddSliders(sliders);
        }
    }
}
