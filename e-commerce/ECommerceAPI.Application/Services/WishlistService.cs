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
    public class WishlistService 
    {
        private readonly IWishlistRepository _repository;
        private readonly IImageService _imageService;

        public WishlistService(IWishlistRepository wishlistRepository, IImageService imageService)
        {
            _repository = wishlistRepository;
            _imageService = imageService;
        }

        public async Task<int> AddWishlist(WishlistCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            Wishlist wishlist = new Wishlist
            {
                ProductId = command.ProductId,
                CustomerId = command.CustomerId,
                Quantity = command.Quantity,
                CreatedOn = DateTime.Now
            };

            return await _repository.Create(wishlist);
        }

        public async Task UpdateWishlist(WishlistCommand command, int id)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var wishlist = await _repository.GetById(id);
            if (wishlist == null)
                throw new KeyNotFoundException($"Wishlist with ID {id} not found.");

            wishlist.ProductId = command.ProductId;
            wishlist.CustomerId = command.CustomerId;
            wishlist.Quantity = command.Quantity;
            wishlist.UpdatedOn = DateTime.Now;

            await _repository.Update(wishlist);
        }

        public async Task DeleteWishlist(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<IList<WishlistModel>> GetAllWishlists()
        {
            IList<Wishlist> wishlists = await _repository.GetAll();

            IList<WishlistModel> wishlistModels = new List<WishlistModel>();
            foreach (Wishlist wishlist in wishlists)
            {
                string imageBase64 = await _imageService.GetImageBase64(wishlist.Product.ProductImages
                   .First(pi => pi.Id == wishlist.Product.ImageId).ImageFilePath);

                wishlistModels.Add(new WishlistModel
                {
                    Id = wishlist.Id,
                    ProductId = wishlist.ProductId,
                    Product = new FilteredProductModel
                    {
                        Id = wishlist.ProductId,
                        ImageBase64 = imageBase64,
                        NameAR = wishlist.Product.NameAR,
                        NameEN = wishlist.Product.NameEN,
                        Price = wishlist.Product.Price
                    },
                    CustomerId = wishlist.CustomerId,
                    Quantity = wishlist.Quantity,
                    CreatedOn = wishlist.CreatedOn,
                    UpdatedOn = wishlist.UpdatedOn
                });
            }

            return wishlistModels;
        }

        public async Task<WishlistModel> GetWishlistById(int id)
        {
            Wishlist wishlist = await _repository.GetById(id);
            if (wishlist == null)
                throw new KeyNotFoundException($"Wishlist with ID {id} not found.");

            string imageBase64 = await _imageService.GetImageBase64(wishlist.Product.ProductImages
                   .First(pi => pi.Id == wishlist.Product.ImageId).ImageFilePath);

            return new WishlistModel
            {
                Id = wishlist.Id,
                ProductId = wishlist.ProductId,
                Product = new FilteredProductModel
                {
                    Id = wishlist.ProductId,
                    ImageBase64 = imageBase64,
                    NameAR = wishlist.Product.NameAR,
                    NameEN = wishlist.Product.NameEN,
                    Price = wishlist.Product.Price
                },
                CustomerId = wishlist.CustomerId,
                Quantity = wishlist.Quantity,
                CreatedOn = wishlist.CreatedOn,
                UpdatedOn = wishlist.UpdatedOn
            };
        }

        public async Task<IList<WishlistModel>> GetCustomerWishlists(int customerId)
        {
            IList<Wishlist> wishlists = await _repository.GetCustomerWishlists(customerId);
            if (wishlists.Count == 0)
                throw new KeyNotFoundException($"Customer with ID {customerId} has no wishlists.");

            IList<WishlistModel> wishlistModels = new List<WishlistModel>();
            foreach (Wishlist wishlist in wishlists)
            {
                string imageBase64 = await _imageService.GetImageBase64(wishlist.Product.ProductImages
                   .First(pi => pi.Id == wishlist.Product.ImageId).ImageFilePath);

                wishlistModels.Add(new WishlistModel
                {
                    Id = wishlist.Id,
                    ProductId = wishlist.ProductId,
                    Product = new FilteredProductModel
                    {
                        Id = wishlist.ProductId,
                        ImageBase64 = imageBase64,
                        NameAR = wishlist.Product.NameAR,
                        NameEN = wishlist.Product.NameEN,
                        Price = wishlist.Product.Price
                    },
                    CustomerId = wishlist.CustomerId,
                    Quantity = wishlist.Quantity,
                    CreatedOn = wishlist.CreatedOn,
                    UpdatedOn = wishlist.UpdatedOn
                });
            }

            return wishlistModels;
        }
    }
}
