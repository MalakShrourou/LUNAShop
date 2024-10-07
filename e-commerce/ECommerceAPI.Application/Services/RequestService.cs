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
    public class RequestService
    {
        private readonly IRepository<Request> _repository;

        public RequestService(IRepository<Request> repository)
        {
            _repository = repository;
        }

        public async Task<int> AddRequest(RequestCommand command)
        {
            Request request = new Request()
            {
                CustomerId = command.CustomerId,
                Message = command.Message,
                Title= command.Title,
                CreatedOn = DateTime.Now
            };
            return await _repository.Create(request);
        }

        public async Task UpdateRequest(RequestCommand command, int id)
        {
            Request request = await _repository.GetById(id);
            request.Title = command.Title;
            request.CustomerId = command.CustomerId;
            request.Message = command.Message;
            request.UpdatedOn = DateTime.Now;
            await _repository.Update(request);
        }

        public async Task DeleteRequest(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<IList<RequestModel>> GetAllRequests()
        {
            IList<Request> requests = await _repository.GetAll();
            return requests.Select(a => new RequestModel
            {
                Id = a.Id,
                CustomerId = a.CustomerId,
                Title = a.Title,
                Message = a.Message,
                CreatedOn = a.CreatedOn,
                UpdatedOn = a.UpdatedOn
            }).ToList();
        }

        public async Task<RequestModel> GetRequestById(int id)
        {
            Request request = await _repository.GetById(id);
            if (request == null)
                return null;
            return new RequestModel
            {
                Id = request.Id,
                CustomerId = request.CustomerId,
                Title = request.Title,
                Message = request.Message,
                UpdatedOn = request.UpdatedOn,
                CreatedOn = request.CreatedOn
            };
        }
    }
}
