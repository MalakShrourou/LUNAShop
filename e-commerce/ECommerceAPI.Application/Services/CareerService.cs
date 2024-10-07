using eCommerceAPI.Application.Commands;
using eCommerceAPI.Application.Models;
using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Services
{
    public class CareerService
    {
        private readonly IRepository<Career> _repository;

        public CareerService(IRepository<Career> repository)
        {
            _repository = repository;
        }

        public async Task<int> AddCareer(CareerCommand command)
        {
            Career career = new Career()
            {
                TitleAR= command.TitleAR,
                TitleEN= command.TitleEN,
                DescriptionAR= command.DescriptionAR,
                DescriptionEN= command.DescriptionEN,
                CreatedOn = DateTime.Now
            };
            return await _repository.Create(career);
        }

        public async Task UpdateCareer(CareerCommand command, int id)
        {
            Career career = await _repository.GetById(id);
            if (career == null)
                throw new Exception("Career not found");
            career.TitleEN = command.TitleEN;
            career.TitleAR = command.TitleAR;
            career.DescriptionEN = command.DescriptionEN;
            career.DescriptionAR = command.DescriptionAR;
            career.UpdatedOn = DateTime.Now;
            await _repository.Update(career);
        }

        public async Task DeleteCareer(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<IList<CareerModel>> GetAllCareers()
        {
            IList<Career> careers = await _repository.GetAll();
            return careers.Select(c => new CareerModel
            {
                Id = c.Id,
                TitleAR= c.TitleAR,
                TitleEN=c.TitleEN,
                DescriptionAR= c.DescriptionAR,
                DescriptionEN=c.DescriptionEN,
                CreatedOn = c.CreatedOn,
                UpdatedOn = c.UpdatedOn
            }).ToList();
        }

        public async Task<CareerModel> GetCareerById(int id)
        {
            Career career = await _repository.GetById(id);
            if (career == null)
                throw new Exception("Career not found");
            return new CareerModel
            {
                Id = career.Id,
                TitleAR = career.TitleAR,
                TitleEN = career.TitleEN,
                DescriptionEN = career.DescriptionEN,
                DescriptionAR = career.DescriptionAR,
                UpdatedOn = career.UpdatedOn,
                CreatedOn = career.CreatedOn
            };
        }
    }
}
