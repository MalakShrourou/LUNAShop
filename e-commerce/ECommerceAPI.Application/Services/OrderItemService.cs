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
    public class OrderItemService
    {
        private readonly IOrderItemRepository _repository;
        private readonly IProductRepository _productRepository;
        private readonly IImageService _imageService;

        public OrderItemService(IOrderItemRepository repository, IProductRepository productRepository, IImageService imageService)
        {
            _repository = repository;
            _productRepository = productRepository;
            _imageService = imageService;
        }

        public async Task<int> AddOrderItem(OrderItemCommand command)
        {
            if (command.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");

            Product product = await _productRepository.GetById(command.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {command.ProductId} not found.");

            OrderItem orderItem = new OrderItem
            {
                ProductId = command.ProductId,
                Product = product,
                OrderId = command.OrderId,
                CustomerId = command.CustomerId,
                Quantity = command.Quantity,
                CreatedOn = DateTime.UtcNow
            };
            return await _repository.Create(orderItem);
        }

        public async Task UpdateOrderItem(OrderItemCommand command, int id)
        {
            OrderItem orderItem = await _repository.GetById(id);
            if (orderItem == null)
                throw new KeyNotFoundException($"Order Item with ID {id} not found.");

            Product product = await _productRepository.GetById(command.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {command.ProductId} not found.");

            orderItem.ProductId = command.ProductId;
            orderItem.Product = product;
            orderItem.OrderId = command.OrderId;
            orderItem.CustomerId = command.CustomerId;
            orderItem.Quantity = command.Quantity;
            orderItem.UpdatedOn = DateTime.UtcNow;
            await _repository.Update(orderItem);
        }

        public async Task DeleteOrderItem(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<IList<OrderItemModel>> GetAllOrderItems()
        {
            IList<OrderItem> orderItems = await _repository.GetAll();
            IList<OrderItemModel> orderItemModels = new List<OrderItemModel>();

            foreach (OrderItem item in orderItems)
            {
                string imageBase64 = await _imageService.GetImageBase64(item.Product.ProductImages
                  .First(pi => pi.Id == item.Product.ImageId).ImageFilePath);

                orderItemModels.Add(new OrderItemModel
                {
                    Id = item.Id,                    
                    OrderId = item.OrderId,
                    ProductId = item.ProductId,
                    CustomerId = item.CustomerId,
                    Product = new FilteredProductModel
                    {
                        Id = item.Product.Id,
                        ImageBase64 = imageBase64,
                        NameEN = item.Product.NameEN,
                        NameAR = item.Product.NameAR,
                        Price = item.Product.Price,
                        Tax = item.Product.Tax
                    },
                    Quantity = item.Quantity,
                    CreatedOn = item.CreatedOn,
                    UpdatedOn = item.UpdatedOn
                });
            }

            return orderItemModels;
        }

        public async Task<IList<OrderItemModel>> GetCustomerItems(int customerId)
        {
            IList<OrderItem> orderItems = await _repository.GetCustomerItems(customerId);
            IList<OrderItemModel> orderItemModels = new List<OrderItemModel>();

            foreach (OrderItem item in orderItems)
            {
                string imageBase64 = await _imageService.GetImageBase64(item.Product.ProductImages
                  .First(pi => pi.Id == item.Product.ImageId).ImageFilePath);

                orderItemModels.Add(new OrderItemModel
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    ProductId = item.ProductId,
                    CustomerId = item.CustomerId,
                    Product = new FilteredProductModel
                    {
                        Id = item.Product.Id,
                        ImageBase64 = imageBase64,
                        NameEN = item.Product.NameEN,
                        NameAR = item.Product.NameAR,
                        Price = item.Product.Price,
                        Tax = item.Product.Tax
                    },
                    Quantity = item.Quantity,
                    CreatedOn = item.CreatedOn,
                    UpdatedOn = item.UpdatedOn
                });
            }

            return orderItemModels;
        }

        public async Task<OrderItemModel> GetOrderItemById(int id)
        {
            OrderItem orderItem = await _repository.GetById(id);
            if (orderItem == null)
                throw new KeyNotFoundException($"Order Item with ID {id} not found.");

            string imageBase64 = await _imageService.GetImageBase64(orderItem.Product.ProductImages
                  .First(pi => pi.Id == orderItem.Product.ImageId).ImageFilePath);

            return new OrderItemModel
            {
                Id = orderItem.Id,
                ProductId = orderItem.ProductId,
                Product = new FilteredProductModel
                {
                    Id = orderItem.Product.Id,
                    ImageBase64 = imageBase64,
                    NameEN = orderItem.Product.NameEN,
                    NameAR = orderItem.Product.NameAR,
                    Price = orderItem.Product.Price,
                    Tax = orderItem.Product.Tax
                },
                OrderId = orderItem.OrderId,
                Quantity = orderItem.Quantity,
                CustomerId = orderItem.CustomerId,
                UpdatedOn = orderItem.UpdatedOn,
                CreatedOn = orderItem.CreatedOn
            };
        }

        public async Task<IList<TopProductsModel>> GetTopProducts()
        {
            IList<OrderItem> products = await _repository.GetAll();
            return products.GroupBy(oi => oi.ProductId)
                .Select(g => new TopProductsModel
                {
                    Id = g.Key,
                    TotalQuantity = g.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(p => p.TotalQuantity)
                .Take(10)
                .ToList();
        }

        public async Task<IList<CategorySalesModel>> GetTopCategories()
        {
            IList<OrderItem> orderItems = await _repository.GetOrderItemsCategory();
            return orderItems.GroupBy(oi => oi.Product.Category)
                .Select(g => new CategorySalesModel
                {
                    CategoryName = g.Key.NameEN,
                    Quantity = g.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(c => c.Quantity)
                .Take(10)
                .ToList();
        }

        public async Task<IList<OrderItemModel>> GetOrderItemsByOrderId(int orderId)
        {
            IList<OrderItem> orderItems = await _repository.GetOrderItemsByOrderId(orderId);
            IList<OrderItemModel> orderItemModels = new List<OrderItemModel>();

            foreach (OrderItem item in orderItems)
            {
                string imageBase64 = await _imageService.GetImageBase64(item.Product.ProductImages
                  .First(pi => pi.Id == item.Product.ImageId).ImageFilePath);

                orderItemModels.Add(new OrderItemModel
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    ProductId = item.ProductId,
                    CustomerId = item.CustomerId,
                    Product = new FilteredProductModel
                    {
                        Id = item.Product.Id,
                        ImageBase64 = imageBase64,
                        NameEN = item.Product.NameEN,
                        NameAR = item.Product.NameAR,
                        Price = item.Product.Price,
                        Tax = item.Product.Tax
                    },
                    Quantity = item.Quantity,
                    CreatedOn = item.CreatedOn,
                    UpdatedOn = item.UpdatedOn
                });
            }

            return orderItemModels;
        }

        public async Task<IList<TrendingProductsModel>> GetTrendingProducts()
        {
            DateTime sevenDaysAgo = DateTime.UtcNow.AddDays(-7);
            IList<OrderItem> products = await _repository.GetTrendingProducts(sevenDaysAgo);
            return products.GroupBy(oi => oi.Product)
                .Select(g => new TrendingProductsModel
                {
                    ProductNameEN = g.Key.NameEN,
                    ProductNameAR = g.Key.NameAR,
                    Quantity = g.Sum(oi => oi.Quantity)
                })
                .OrderByDescending(p => p.Quantity)
                .Take(10)
                .ToList();
        }

        public async Task<IList<ProductSalesModel>> GetProductSales(int productId)
        {
            DateTime sevenDaysAgo = DateTime.UtcNow.AddDays(-30);
            IList<OrderItem> products = await _repository.GetProductSales(productId, sevenDaysAgo);
            return products.GroupBy(oi => oi.Order.CreatedOn.Date)
                .Select(g => new ProductSalesModel
                {
                    Date = g.Key,
                    TotalQuantity = g.Sum(oi => oi.Quantity)
                })
                .OrderBy(g => g.Date)
                .ToList();
        }
    }
}
