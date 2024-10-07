using eCommerceAPI.Application.Commands;
using eCommerceAPI.Application.Models;
using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Interfaces.Repository;
using eCommerceAPI.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Services
{
    public class BrandService
    {
        private readonly IBrandRepository _repository;
        private readonly IImageService _imageService;

        public BrandService(IBrandRepository brandRepository, IImageService imageService)
        {
            _repository = brandRepository;
            _imageService = imageService;
        }

        public async Task<int> AddBrand(BrandCommand command)
        {
            string fileName = await _imageService.SaveImage(command.ImageBase64);

            Brand brand = new Brand
            {
                NameAR = command.NameAR,
                NameEN = command.NameEN,
                IsActive = true,
                ImageFilePath = fileName,
                CreatedOn = DateTime.Now
            };

            return await _repository.Create(brand);
        }

        public async Task UpdateBrand(BrandCommand command, int id)
        {
            Brand brand = await _repository.GetById(id);
            if (brand == null)
                throw new KeyNotFoundException($"Brand with ID {id} was not found.");

            string newImagePath = await _imageService.SaveImage(command.ImageBase64);

            brand.NameAR = command.NameAR;
            brand.NameEN = command.NameEN;
            brand.IsActive = command.IsActive;
            brand.ImageFilePath = newImagePath;
            brand.UpdatedOn = DateTime.Now;

            await _repository.Update(brand);
        }

        public async Task DeleteBrand(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<IList<BrandModel>> GetAllBrands()
        {
            IList<Brand> brands = await _repository.GetAll();
            IList<BrandModel> brandModels = new List<BrandModel>();

            foreach (Brand brand in brands)
            {
                string imageBase64 = await _imageService.GetImageBase64(brand.ImageFilePath);

                brandModels.Add(new BrandModel
                {
                    Id = brand.Id,
                    NameAR = brand.NameAR,
                    NameEN = brand.NameEN,
                    ImageBase64 = imageBase64,
                    IsActive = brand.IsActive,
                    CreatedOn = brand.CreatedOn,
                    UpdatedOn = brand.UpdatedOn
                });
            }

            return brandModels;
        }

        public async Task<BrandModel> GetBrandById(int id)
        {
            Brand brand = await _repository.GetById(id);
            if (brand == null)
                throw new KeyNotFoundException($"Brand with ID {id} was not found.");

            string imageBase64 = await _imageService.GetImageBase64(brand.ImageFilePath);

            return new BrandModel
            {
                Id = brand.Id,
                NameAR = brand.NameAR,
                NameEN = brand.NameEN,
                ImageBase64 = imageBase64,
                IsActive = brand.IsActive,
                UpdatedOn = brand.UpdatedOn,
                CreatedOn = brand.CreatedOn
            };
        }

        public async Task<(IList<FilteredProductModel>, int)> GetProductsByBrand(FilterCommand command)
        {
            if (command.BrandId == null)
                throw new NullReferenceException();

            (IList<Product> products, int totalCount) = await _repository.GetProductsByBrand(command.CategoryId, command.SortBy, (int)command.BrandId,
                command.MinPrice, command.MaxPrice, command.PageNumber, command.PageSize, command.UserLang);
            IList<FilteredProductModel> filteredProducts = new List<FilteredProductModel>();
            foreach (Product product in products)
            {
                string imageBase64 = await _imageService.GetImageBase64(product.ProductImages
                    .First(pi => pi.Id == product.ImageId).ImageFilePath);

                filteredProducts.Add(new FilteredProductModel
                {
                    Id = product.Id,
                    NameAR = product.NameAR,
                    NameEN = product.NameEN,
                    Price = product.Price,
                    Tax = product.Tax,
                    ImageBase64 = imageBase64
                });
            }
            return (filteredProducts, totalCount);
        }

        public async Task ChangeActiveStatus(int id)
        {
            await _repository.ChangeActiveStatus(id);
        }

        public async Task<IList<BrandModel>> GetActive()
        {
            IList<Brand> brands = await _repository.GetActive();
            IList<BrandModel> brandModels = new List<BrandModel>();

            foreach (var b in brands)
            {
                string imageBase64 = await _imageService.GetImageBase64(b.ImageFilePath);

                brandModels.Add(new BrandModel
                {
                    Id = b.Id,
                    NameAR = b.NameAR,
                    NameEN = b.NameEN,
                    ImageBase64 = imageBase64,
                    IsActive = b.IsActive,
                    CreatedOn = b.CreatedOn,
                    UpdatedOn = b.UpdatedOn
                });
            }

            return brandModels;
        }
    }
}
