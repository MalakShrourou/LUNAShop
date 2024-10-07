using eCommerceAPI.Application.Commands;
using eCommerceAPI.Application.Models;
using eCommerceAPI.Application.Queries;
using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Interfaces.Repository;
using eCommerceAPI.Domain.Interfaces.Service;
using Microsoft.Extensions.Configuration;
using NETCore.Encrypt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;
        private readonly IEmailService _emailService;

        public CustomerService(ICustomerRepository customerRepository, IConfiguration configuration, AuthService authService, IEmailService emailService)
        {
            _repository = customerRepository;
            _configuration = configuration;
            _authService = authService;
            _emailService = emailService;
        }

        public async Task<int> AddCustomer(CustomerCommand command)
        {
            string key = _configuration["EncryptionKey"];
            string encryptedPassword = EncryptProvider.DESEncrypt(command.Password, key);

            Customer customer = new Customer()
            {
                FNameAR = command.FNameAR,
                LNameAR = command.LNameAR,
                FNameEN = command.FNameEN,
                LNameEN = command.LNameEN,
                Email = command.Email,
                Password = encryptedPassword,
                Phone = command.Phone,
                IsActive = true, 
                LastLogin = DateTime.Now,
                RoleId = 1,
                VerificationCode = Guid.NewGuid().ToString(),
                IsVerified = false,
                CreatedOn = DateTime.Now
            };

            customer.Id = await _repository.Create(customer);
            string verificationLink = $"http://localhost:4200/customerVerify?code={customer.VerificationCode}";
          //  await _emailService.SendVerificationEmailAsync(customer.Email, verificationLink);
            return customer.Id;
        }

        public async Task UpdateCustomer(CustomerCommand command, int id)
        {
            Customer customer = await _repository.GetById(id);
            customer.FNameAR = command.FNameAR;
            customer.LNameAR = command.LNameAR;
            customer.FNameEN = command.FNameEN;
            customer.LNameEN = command.LNameEN;
            customer.Email = command.Email;
            customer.Password = customer.Password;
            customer.Phone = command.Phone;
            customer.UpdatedOn = DateTime.Now;
            await _repository.Update(customer);
        }
        public async Task<Customer> FindByVerificationCodeAsync(string code)
        {
            return await _repository.FindByVerificationCodeAsync(code);
        }

        public async Task DeleteCustomer(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<IList<CustomerModel>> GetAllCustomers()
        {
            IList<Customer> customers = await _repository.GetAll();
            return customers.Select(c => new CustomerModel
            {
                Id = c.Id,
                FNameAR = c.FNameAR,
                LNameAR = c.LNameAR,
                FNameEN = c.FNameEN,
                LNameEN = c.LNameEN,
                Email = c.Email,
                LastLogin = c.LastLogin,
                LastOrder = c.LastOrder,
                Phone=c.Phone,
                IsActive = c.IsActive,
                Addresses =c.Addresses.Select(a => new AddressModel
                {
                    Id= a.Id,
                    City =a.City,
                    StreetName = a.StreetName,
                    BuildingNo = a.BuildingNo
                }).ToList(),
                Orders = c.Orders.Select(o => new OrderModel
                {
                    Id = o.Id,
                    TotalPrice=o.TotalPrice,
                    DeliveryDate = o.DeliveryDate,
                    Feedback=o.Feedback,
                    Rate= o.Rate,
                    Status=o.Status,
                    Notes = o.Notes,
                    CreatedOn = o.CreatedOn
                }).ToList(),
                CreatedOn = c.CreatedOn,
                UpdatedOn = c.UpdatedOn
            }).ToList();
        }

        public async Task<CustomerModel> GetCustomerById(int id)
        {
            Customer customer = await _repository.GetById(id);
            if (customer == null)
                return null;
            return new CustomerModel
            {
                Id = customer.Id,
                FNameAR = customer.FNameAR,
                LNameAR = customer.LNameAR,
                FNameEN = customer.FNameEN,
                LNameEN = customer.LNameEN,
                Email = customer.Email,
                Phone=customer.Phone,
                LastOrder = customer.LastOrder,
                LastLogin = customer.LastLogin,
                UpdatedOn = customer.UpdatedOn,
                CreatedOn = customer.CreatedOn,
                IsActive = customer.IsActive,
                DefaultAddress = customer.DefaultAddress,
                Addresses = customer.Addresses.Select(a => new AddressModel
                {
                    Id = a.Id,
                    City = a.City,
                    StreetName = a.StreetName,
                    BuildingNo = a.BuildingNo,
                    CreatedOn=a.CreatedOn,
                }).ToList(),
                Orders = customer.Orders.Select(o => new OrderModel
                {
                    Id = o.Id,
                    TotalPrice = o.TotalPrice,
                    DeliveryDate = o.DeliveryDate,
                    Feedback = o.Feedback,
                    Rate = o.Rate,
                    Status = o.Status,
                    Notes = o.Notes,
                    CreatedOn=o.CreatedOn
                }).ToList()
            };
        }

        public async Task<AuthenticationModel> Login(AuthenticationQuery query)
        {
            Customer customer = await _repository.Login(query.Email);
            if (customer == null)
                return null;

            string key = _configuration["EncryptionKey"];
            string decrtyptedPassowrd = EncryptProvider.DESDecrypt(customer.Password, key);
            if (decrtyptedPassowrd != query.Password)
                throw new Exception("Wrong password");

            AuthenticationModel authenticationModel = new AuthenticationModel()
            {
                Id = customer.Id,
                AccessToken = await _authService.GenerateTokenAsync(customer.Role, customer.Id, query.RememberMe),
                ExpiresAt = DateTime.Now.AddHours(1)
            };

            return authenticationModel;
        }

        public async Task DefaultAddress(DefaultAddressCommand command)
        {
            await _repository.DefaultAddress(command.AddressId, command.CustomerId);
        }

        public async Task ChangeActiveStatus(int customerId)
        {
            await _repository.ChangeActiveStatus(customerId);
        }

        public async Task<bool> ChangePassword(ChangePasswordCommand command)
        {
            string key = _configuration["EncryptionKey"];

            Customer customer = await _repository.GetById(command.Id);
            string decryptedPassword = EncryptProvider.DESDecrypt(customer.Password, key);

            if (decryptedPassword == command.CurrentPassword)
            {
                string encryptedNewPassword = EncryptProvider.DESEncrypt(command.NewPassword, key);
                return await _repository.ChangePassword(command.Id, encryptedNewPassword);
            }

            return false;
        }
    }
}
