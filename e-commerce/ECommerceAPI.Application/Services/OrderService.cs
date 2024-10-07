using eCommerceAPI.Application.Commands;
using eCommerceAPI.Application.Models;
using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Enums;
using eCommerceAPI.Domain.Interfaces.Repository;
using eCommerceAPI.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Services
{
    public class OrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IProductRepository _productRepository;
        private readonly IEmailService _emailService;

        public OrderService(IOrderRepository repository, IProductRepository productRepository, IEmailService emailService) 
        {
            _repository = repository;
            _productRepository = productRepository;
            _emailService = emailService;
        }

        public async Task<int> AddOrder(OrderCommand command)
        {
            if (command == null)
                throw new ArgumentException("Invalid order command.");

            if (command.OrderItems == null || !command.OrderItems.Any())
                throw new ArgumentException("No order items provided.");
          
            Order order = new Order
            {
                CustomerId = command.CustomerId,
                Notes = command.Notes,
                Status = Status.New,
                AddressId = command.AddressId,
                CreatedOn = DateTime.Now,
                OrderItems = new List<OrderItem>()
            };

            foreach (OrderItemCommand item in command.OrderItems)
            {
                if (item.Quantity <= 0)
                    throw new ArgumentException("Order item quantity must be greater than zero.");

                Product product = await _productRepository.GetById(item.ProductId);
                if (product == null)
                    throw new ArgumentException($"Product with ID {item.ProductId} not found.");

                OrderItem orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    CustomerId = item.CustomerId,
                    Quantity = item.Quantity,
                    CreatedOn = DateTime.Now,
                    Product = product
                };

                order.OrderItems.Add(orderItem);
            }

            order.TotalPrice = order.OrderItems.Sum(oi => oi.NetPrice);
            int id = await _repository.Create(order);
            Order confirmedOrder = await _repository.GetById(id);
           // await _emailService.SendOrderConfirmationEmail(confirmedOrder);
            return id;
        }

        public async Task UpdateOrder(OrderCommand command, int id)
        {
            Order order = await _repository.GetById(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found.");

            order.TotalPrice = order.OrderItems.Sum(oi => oi.NetPrice);
            order.AddressId = command.AddressId;
            order.Notes = command.Notes;
            order.CustomerId = command.CustomerId;
            order.Status = command.Status;
            order.Rate = command.Rate;
            order.Feedback = command.Feedback;
            order.UpdatedOn = DateTime.Now;

            await _repository.Update(order);
        }

        public async Task DeleteOrder(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<IList<OrderModel>> GetAllOrders()
        {
            IList<Order> orders = await _repository.GetAll();
            IList<OrderModel> orderModels = new List<OrderModel>();
            
            return orders.Select(o => new OrderModel
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                CustomerName = $"{o.Customer.FNameEN} {o.Customer.LNameEN}",
                CustomerPhone = o.Customer.Phone,
                TotalPrice = o.TotalPrice,
                Notes = o.Notes,
                Status = o.Status,
                AddressId = o.AddressId,
                DeliveryDate = o.DeliveryDate,
                OrderItems = o.OrderItems.Select(oi => new OrderItemModel
                {
                    ProductId = oi.ProductId,
                    Product = new FilteredProductModel
                    { 
                        Id = oi.Product.Id, 
                        NameEN = oi.Product.NameEN, 
                        NameAR = oi.Product.NameAR, 
                        Price = oi.Product.Price,
                        Tax = oi.Product.Tax
                    },
                    Quantity = oi.Quantity,
                }).ToList(),
                CreatedOn = o.CreatedOn,
                UpdatedOn = o.UpdatedOn
            }).ToList();
        }

        public async Task<OrderModel> GetOrderById(int id)
        {
            Order order = await _repository.GetById(id);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {id} not found.");

            return new OrderModel
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                CustomerName = $"{order.Customer.FNameEN} {order.Customer.LNameEN}",
                CustomerPhone = order.Customer.Phone,
                TotalPrice = order.TotalPrice,
                Notes = order.Notes,
                Status = order.Status,
                AddressId = order.AddressId,
                Address = new AddressModel
                {
                    BuildingNo = order.Address.BuildingNo,
                    City= order.Address.City,
                    CreatedOn= order.Address.CreatedOn,
                    CustomerId= order.Address.CustomerId,
                    StreetName= order.Address.StreetName,
                    Id=order.Address.Id,
                    UpdatedOn=order.Address.UpdatedOn
                },
                DeliveryDate = order.DeliveryDate,
                Rate = order.Rate,
                Feedback = order.Feedback,
                OrderItems = order.OrderItems.Select(oi => new OrderItemModel
                {
                    ProductId = oi.ProductId,
                    Product = new FilteredProductModel
                    {
                        Id = oi.Product.Id,
                        NameEN = oi.Product.NameEN,
                        NameAR = oi.Product.NameAR,
                        Price = oi.Product.Price,
                        Tax = oi.Product.Tax
                    },
                    Quantity = oi.Quantity,
                }).ToList(),
                UpdatedOn = order.UpdatedOn,
                CreatedOn = order.CreatedOn
            };
        }

        public async Task ChangeStatus(ChangeStatusCommand command)
        {
            await _repository.ChangeStatus(command.Id, command.Status);
        }

        public async Task<IList<OrderModel>> GetCustomerOrders(int customerId)
        {
            var orders = await _repository.GetCustomerOrders(customerId);

            return orders.Select(o => new OrderModel
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                TotalPrice = o.TotalPrice,
                Status = o.Status,
                DeliveryDate = o.DeliveryDate,
                Notes = o.Notes,
                AddressId = (int)o.AddressId,
                CreatedOn = o.CreatedOn,
                UpdatedOn = o.UpdatedOn,
            }).ToList();
        }

        public async Task<IList<DailySalesModel>> GetDailySales()
        {
            DateTime endDate = DateTime.Now.Date;
            DateTime startDate = endDate.AddDays(-30);
            IList<Order> orders = await _repository.GetSalesByDeliveryDate(startDate, endDate);

            return orders.GroupBy(s => s.DeliveryDate!.Value.Date)
                         .Select(g => new DailySalesModel
                         {
                             Date = g.Key,
                             TotalSales = g.Sum(s => s.TotalPrice)
                         }).ToList();
        }

        public async Task<IList<DailyOrderCountModel>> GetDailyOrderCounts()
        {
            DateTime endDate = DateTime.Now.Date;
            DateTime startDate = endDate.AddDays(-30);
            IList<Order> orders = await _repository.GetSalesByDeliveryDate(startDate, endDate);

            return orders.GroupBy(s => s.DeliveryDate!.Value.Date)
                         .Select(g => new DailyOrderCountModel
                         {
                             Date = g.Key,
                             OrderCount = g.Count()
                         }).ToList();
        }

        public async Task<IList<MonthlySalesModel>> GetMonthlySales()
        {
            DateTime endDate = DateTime.Now.Date;
            DateTime startDate = endDate.AddMonths(-12);
            IList<Order> orders = await _repository.GetSalesByDeliveryDate(startDate, endDate);

            return orders.GroupBy(o => new { o.DeliveryDate!.Value.Year, o.DeliveryDate.Value.Month })
                         .Select(g => new MonthlySalesModel
                         {
                             YearMonth = new DateTime(g.Key.Year, g.Key.Month, 1),
                             TotalSales = g.Sum(o => o.TotalPrice)
                         }).ToList();
        }

        public async Task<IList<MonthlyOrderCountModel>> GetMonthlyOrders()
        {
            DateTime endDate = DateTime.Now.Date;
            DateTime startDate = endDate.AddDays(-30);
            IList<Order> orders = await _repository.GetSalesByDeliveryDate(startDate, endDate);

            return orders.GroupBy(o => new { o.DeliveryDate!.Value.Year, o.DeliveryDate.Value.Month })
                         .Select(g => new MonthlyOrderCountModel
                         {
                             Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                             Count = g.Count()
                         }).ToList();
        }

        public async Task<IList<OrderStatusCountModel>> GetOrderStatusCounts()
        {
            IList<Order> orders = await _repository.GetOrderStatus();

            return orders.GroupBy(o => o.Status.ToString())
                         .Select(g => new OrderStatusCountModel
                         {
                             Status = g.Key,
                             Count = g.Count()
                         }).ToList();
        }
    }
}