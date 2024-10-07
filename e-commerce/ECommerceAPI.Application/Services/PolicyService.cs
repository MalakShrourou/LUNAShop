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
    public class PolicyService
    {
        private readonly IRepository<Policy> _repository;
        private readonly IImageService _imageService;

        public PolicyService(IRepository<Policy> repository,IImageService imageService)
        {
            _repository = repository;
            _imageService = imageService;
        }

        public async Task<int> AddPolicy(PolicyCommand command)
        {
            string fileEN = await _imageService.SaveImage(command.FileBase64EN);
            string fileAR = await _imageService.SaveImage(command.FileBase64AR);
            Policy policy = new Policy()
            {
                TitleEN = command.TitleEN,
                FilePathEN = fileEN,
                FilePathAR = fileAR,
                CreatedOn = DateTime.Now
            };
            return await _repository.Create(policy);
        }

        public async Task UpdatePolicy(PolicyCommand command, int id)
        {
            string fileEN = await _imageService.SaveImage(command.FileBase64EN);
            string fileAR = await _imageService.SaveImage(command.FileBase64AR);
            Policy policy = await _repository.GetById(id);
            policy.TitleEN = command.TitleEN;
            policy.FilePathEN = fileEN; 
            policy.TitleAR = command.TitleAR;
            policy.FilePathAR = fileAR;
            policy.UpdatedOn = DateTime.Now;
            await _repository.Update(policy);
        }

        public async Task DeletePolicy(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<IList<PolicyModel>> GetAllPolicies()
        {
            IList<Policy> policies = await _repository.GetAll();
            IList<PolicyModel> models = new List<PolicyModel>();
            foreach (Policy policy in policies)
            {
                string fileEN = await _imageService.GetImageBase64(policy.FilePathEN);
                string fileAR = await _imageService.GetImageBase64(policy.FilePathAR);
                models.Add(new PolicyModel
                {
                    Id = policy.Id,
                    TitleEN = policy.TitleEN,
                    FileBase64EN = fileEN,
                    TitleAR = policy.TitleAR,
                    FileBase64AR = fileAR,
                    CreatedOn = policy.CreatedOn,
                    UpdatedOn = policy.UpdatedOn
                });
            }
            return models;
        }

        public async Task<PolicyModel> GetPolicyById(int id)
        {
            Policy policy = await _repository.GetById(id);
            if (policy == null)
                return null;

            string fileEN = await _imageService.GetImageBase64(policy.FilePathEN);
            string fileAR = await _imageService.GetImageBase64(policy.FilePathAR);
            return new PolicyModel
            {
                Id = policy.Id,
                FileBase64EN = fileEN,
                TitleEN = policy.TitleEN, 
                FileBase64AR = fileAR,
                TitleAR = policy.TitleAR,
                UpdatedOn = policy.UpdatedOn,
                CreatedOn = policy.CreatedOn
            };
        }
    }
}
