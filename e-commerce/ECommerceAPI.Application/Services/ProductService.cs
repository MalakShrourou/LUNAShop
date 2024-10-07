using eCommerceAPI.Application.Commands;
using eCommerceAPI.Application.Models;
using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Interfaces.Repository;
using eCommerceAPI.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Services
{
    public class ProductService
    {
        private readonly IProductRepository _repository;
        private readonly IImageService _imageService;

        public ProductService(IProductRepository productRepository, IImageService imageService)
        {
            _repository = productRepository;
            _imageService = imageService;
        }

        public async Task<int> AddProduct(ProductCommand command)
        {
            IList<ProductImage> productImages = new List<ProductImage>();

            if (command.ProductImages != null && command.ProductImages.Any())
            {
                foreach (ProductImageCommand imageCommand in command.ProductImages)
                {
                    if (string.IsNullOrEmpty(imageCommand.ImageBase64))
                        throw new ArgumentException("Product image data is missing.");

                    string filePath = await _imageService.SaveImage(imageCommand.ImageBase64);
                    productImages.Add(new ProductImage
                    {
                        ImageFilePath = filePath,
                        CreatedOn = DateTime.Now
                    });                  
                }
            }
           

            Product product = new Product
            {
                NameAR = command.NameAR,
                NameEN = command.NameEN,
                Availability = command.Availability,
                BrandId = command.BrandId,
                CategoryId = command.CategoryId,
                DescriptionAR = command.DescriptionAR,
                DescriptionEN = command.DescriptionEN,
                ImageId = command.ImageId,
                Price = command.Price,
                Tax = command.Tax,
                UserId = command.UserId,
                ProductImages = productImages,
                CreatedOn = DateTime.Now
            };

            return await _repository.Create(product);
        }

        public async Task UpdateProduct(ProductCommand command, int id)
        {
            Product product = await _repository.GetById(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            product.NameAR = command.NameAR;
            product.NameEN = command.NameEN;
            product.Availability = command.Availability;
            product.BrandId = command.BrandId;
            product.CategoryId = command.CategoryId;
            product.DescriptionAR = command.DescriptionAR;
            product.DescriptionEN = command.DescriptionEN;
            product.ImageId = command.ImageId;
            product.Price = command.Price;
            product.Tax = command.Tax;
            product.UserId = command.UserId;
            product.UpdatedOn = DateTime.Now;

            if (command.ProductImages != null && command.ProductImages.Any())
            {
                product.ProductImages.Clear();

                foreach (ProductImageCommand imageCommand in command.ProductImages)
                {
                    if (string.IsNullOrEmpty(imageCommand.ImageBase64))
                        throw new ArgumentException("Product image data is missing.");

                    string filePath = await _imageService.SaveImage(imageCommand.ImageBase64);
                    ProductImage productImage = new ProductImage
                    {
                        ImageFilePath = filePath,
                        ProductId = product.Id,
                        CreatedOn = DateTime.Now
                    };
                    
                    product.ProductImages.Add(productImage);
                }
            }                                                

            await _repository.Update(product);
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _repository.GetById(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            await _repository.Delete(id);
        }

        public async Task<IList<ProductModel>> GetAllProducts()
        {
            IList<Product> products = await _repository.GetAll();

            // Use Task.WhenAll to ensure asynchronous operations for each product are handled efficiently
            return await Task.WhenAll(products.Select(async p => new ProductModel
            {
                Id = p.Id,
                NameAR = p.NameAR,
                NameEN = p.NameEN,
                Availability = p.Availability,
                BrandId = p.BrandId,
                CategoryId = p.CategoryId,
                DescriptionAR = p.DescriptionAR,
                DescriptionEN = p.DescriptionEN,
                ImageId = p.ImageId,
                Price = p.Price,
                Tax = p.Tax,
                UserId = p.UserId,
                User = new UserModel
                {
                    FNameEN = p.User.FNameEN,
                    LNameEN = p.User.LNameEN,
                    CreatedOn = p.User.CreatedOn,
                    Email = p.User.Email,
                    FNameAR = p.User.FNameAR,
                    LNameAR = p.User.LNameAR,
                    Id = p.User.Id,
                    Phone = p.User.Phone,
                    RoleId = p.User.RoleId,
                    RoleName = p.User.Role.NameEN,
                    Role = p.User.Role,
                    UpdatedOn = p.User.UpdatedOn
                },
                CreatedOn = p.CreatedOn,
                UpdatedOn = p.UpdatedOn,
                ProductImages = await Task.WhenAll(p.ProductImages.Select(async pi => new ProductImageModel
                {
                    Id = pi.Id,
                    ImageBase64 = await _imageService.GetImageBase64(pi.ImageFilePath)
                }).ToList())
            }));
        }

        public async Task<ProductModel> GetProductById(int id)
        {
            Product product = await _repository.GetById(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            return new ProductModel
            {
                Id = product.Id,
                NameAR = product.NameAR,
                NameEN = product.NameEN,
                Availability = product.Availability,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                DescriptionAR = product.DescriptionAR,
                DescriptionEN = product.DescriptionEN,
                ImageId = product.ImageId,
                Price = product.Price,
                Tax = product.Tax,
                Reviews = product.Reviews?.Select(r => new ReviewModel
                {
                    Id = r.Id,
                    TextReview = r.TextReview,
                    Rate = r.Rate,
                    CustomerId = r.CustomerId,
                    Customer = new CustomerModel
                    {
                        DefaultAddress = r.Customer.DefaultAddress,
                        Id = r.Customer.Id,
                        FNameAR = r.Customer.FNameAR,
                        LNameAR = r.Customer.LNameAR,
                        FNameEN = r.Customer.FNameEN,
                        LNameEN = r.Customer.LNameEN,
                        Email = r.Customer.Email,
                        LastLogin = r.Customer.LastLogin,
                        LastOrder = r.Customer.LastOrder,
                        IsActive = r.Customer.IsActive,
                    },
                    ProductId = product.Id,
                    CreatedOn = r.CreatedOn
                }).ToList() ,
                ProductImages = await Task.WhenAll(product.ProductImages.Select(async pi => new ProductImageModel
                {
                    Id = pi.Id,
                    ImageBase64 = await _imageService.GetImageBase64(pi.ImageFilePath)
                })),
                UserId = product.UserId,
                UpdatedOn = product.UpdatedOn,
                CreatedOn = product.CreatedOn
            };
        }

        public async Task<IList<FilteredProductModel>> Top10InCategory(int productId)
        {
            IList<Product> products = await _repository.Top10InCategory(productId);
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
            return filteredProducts;
        }

        public async Task<IList<FilteredProductModel>> GetPopularProducts()
        {
            IList<Product> products = await _repository.GetPopularProducts();
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
            return filteredProducts;
        }

        public async Task<(IList<FilteredProductModel>, int)> SearchProducts(FilterCommand command, string query)
        {
            if (query == null)
                throw new NullReferenceException();

            (IList<Product> products, int totalCount) = await _repository.SearchProducts(query, command.CategoryId, command.SortBy, command.BrandId,
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
    }
}
