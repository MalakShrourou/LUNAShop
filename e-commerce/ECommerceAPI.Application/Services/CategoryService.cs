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
    public class CategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IImageService _imageService;
        public CategoryService(ICategoryRepository categoryRepository, IImageService imageService)
        {
            _repository = categoryRepository;
            _imageService = imageService;            
        }

        public async Task<int> AddCategory(CategoryCommand command)
        {
            string fileName = await _imageService.SaveImage(command.ImageBase64);

            Category category = new Category()
            {
                NameAR = command.NameAR,
                DescriptionAR = command.DescriptionAR,
                NameEN = command.NameEN,
                DescriptionEN = command.DescriptionEN,
                ImageFilePath = fileName,
                IsActive = command.IsActive,
                CreatedOn = DateTime.Now
            };

            return await _repository.Create(category);
        }

        public async Task UpdateCategory(CategoryCommand command, int id)
        {
            Category category = await _repository.GetById(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} was not found.");

            string newImagePath = await _imageService.SaveImage(command.ImageBase64);

            category.NameAR = command.NameAR;
            category.DescriptionAR = command.DescriptionAR;
            category.NameEN = command.NameEN;
            category.DescriptionEN = command.DescriptionEN;
            category.IsActive = command.IsActive;
            category.ImageFilePath = newImagePath;
            category.UpdatedOn = DateTime.Now;

            await _repository.Update(category);
        }

        public async Task DeleteCategory(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<IList<CategoryModel>> GetAllCategories()
        {
            IList<Category> categories = await _repository.GetAll();
            IList<CategoryModel> models = new List<CategoryModel>();

            foreach (Category category in categories)
            {
                string imageBase64 = await _imageService.GetImageBase64(category.ImageFilePath);

                models.Add(new CategoryModel
                {
                    Id = category.Id,
                    NameAR = category.NameAR,
                    DescriptionAR = category.DescriptionAR,
                    NameEN = category.NameEN,
                    DescriptionEN = category.DescriptionEN,
                    ImageBase64 = imageBase64,
                    IsActive = category.IsActive,
                    CreatedOn = category.CreatedOn,
                    UpdatedOn = category.UpdatedOn

                });
            }
            return models;
        }

        public async Task<IList<CategoryModel>> GetActive()
        {
            IList<Category> categories = await _repository.GetActive();
            IList<CategoryModel> models = new List<CategoryModel>();

            foreach (Category category in categories)
            {
                string imageBase64 = await _imageService.GetImageBase64(category.ImageFilePath);

                models.Add(new CategoryModel
                {
                    Id = category.Id,
                    NameAR = category.NameAR,
                    DescriptionAR = category.DescriptionAR,
                    NameEN = category.NameEN,
                    DescriptionEN = category.DescriptionEN,
                    ImageBase64 = imageBase64,
                    IsActive = category.IsActive,
                    CreatedOn = category.CreatedOn,
                    UpdatedOn = category.UpdatedOn
                });
            }
            return models;
        }

        public async Task<CategoryModel> GetCategoryById(int id)
        {
            Category category = await _repository.GetById(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} was not found.");

            string imageBase64 = await _imageService.GetImageBase64(category.ImageFilePath);

            return new CategoryModel
            {
                Id = category.Id,
                NameAR = category.NameAR,
                DescriptionAR = category.DescriptionAR,
                NameEN = category.NameEN,
                DescriptionEN = category.DescriptionEN,
                IsActive = category.IsActive,
                ImageBase64 = imageBase64,
                UpdatedOn = category.UpdatedOn,
                CreatedOn = category.CreatedOn
            };
        }

        public async Task<(IList<FilteredProductModel>, int)> GetProductsByCategory(FilterCommand command)
        {
            if (command.CategoryId == null)
                throw new NullReferenceException();

            (IList<Product> products, int totalCount) = await _repository.GetProductsByGategory((int)command.CategoryId, command.SortBy, command.BrandId,
                command.MinPrice, command.MaxPrice, command.PageNumber, command.PageSize, command.UserLang);
            IList<FilteredProductModel> filteredProducts = new List<FilteredProductModel>();
            foreach(Product product in products)
            {
                string imageBase64 = await  _imageService.GetImageBase64(product.ProductImages
                    .First(pi => pi.Id == product.ImageId).ImageFilePath);

                filteredProducts.Add(new FilteredProductModel
                {
                    Id = product.Id,
                    NameAR = product.NameAR,
                    NameEN = product.NameEN,
                    Price = product.Price,
                    Tax= product.Tax,
                    ImageBase64 = imageBase64
                });
            }
            return (filteredProducts, totalCount);
        }

        public async Task ChangeActiveStatus(int id)
        {
             await _repository.ChangeActiveStatus(id);
        }
    }
}
