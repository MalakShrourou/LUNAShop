using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Services
{
    public class PermissionService
    {
        private readonly IPermissionRepository _repository;

        public PermissionService(IPermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IList<Permission>> GetPermissions()
        {
            return await _repository.GetAll();
        }
    }
}
