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
    public class AddressService
    {
        private readonly IRepository<Address> _repository;

        public AddressService(IRepository<Address> repository)
        {
            _repository = repository;
        }

        public async Task<int> AddAddress(AddressCommand command)
        {
            Address address = new Address()
            {
                CustomerId = command.CustomerId,
                BuildingNo = command.BuildingNo,
                City = command.City,
                StreetName = command.StreetName,
                CreatedOn = DateTime.Now
            };
            return await _repository.Create(address);
        }

        public async Task UpdateAddress(AddressCommand command, int id)
        {
            Address address = await _repository.GetById(id);
            address.StreetName = command.StreetName;
            address.CustomerId = command.CustomerId;
            address.BuildingNo = command.BuildingNo;
            address.City = command.City;
            address.UpdatedOn = DateTime.Now;
            await _repository.Update(address);
        }

        public async Task DeleteAddress(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<IList<AddressModel>> GetAllAddresss()
        {
            IList<Address> addresses = await _repository.GetAll();
            return addresses.Select(a => new AddressModel
            {
                Id = a.Id,
                CustomerId = a.CustomerId,
                City = a.City,
                StreetName = a.StreetName,
                BuildingNo = a.BuildingNo,
                CreatedOn = a.CreatedOn,
                UpdatedOn = a.UpdatedOn
            }).ToList();
        }

        public async Task<AddressModel> GetAddressById(int id)
        {
            Address address = await _repository.GetById(id);
            if (address == null)
                return null;
            return new AddressModel
            {
                Id = address.Id,
                CustomerId = address.CustomerId,
                City = address.City,
                StreetName = address.StreetName,
                BuildingNo = address.BuildingNo,
                UpdatedOn = address.UpdatedOn,
                CreatedOn = address.CreatedOn
            };
        }
    }
}
