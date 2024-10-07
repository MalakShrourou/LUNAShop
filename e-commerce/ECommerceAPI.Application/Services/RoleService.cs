using eCommerceAPI.Application.Commands;
using eCommerceAPI.Application.Models;
using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Services
{
    public class RoleService
    {
        private readonly IRoleRepository _repository;

        public RoleService(IRoleRepository repository) 
        {
            _repository = repository;
        }

        public async Task<int> AddRole(RoleCommand command)
        {           
            Role role = new Role()
            {
                NameAR = command.NameAR,
                NameEN = command.NameEN,
                Permissions = command.Permissions.Select(p => new Permission
                {
                    Action = p.Action
                }).ToList(),
                CreatedOn = DateTime.Now
            };

            return await _repository.Create(role);
        }

        public async Task UpdateRole(RoleCommand command, int id)
        {
            Role role = await _repository.GetById(id);
            if (role == null) 
                throw new Exception("Role not found");

            role.NameAR = command.NameAR;
            role.NameEN = command.NameEN;
            role.Permissions = command.Permissions.Select(p => new Permission
            {
                Action = p.Action
            }).ToList();
            role.UpdatedOn = DateTime.Now;
            await _repository.Update(role);
        }

        public async Task DeleteRole(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<IList<RoleModel>> GetAllRoles()
        {
            IList<Role> roles = await _repository.GetAll();
            return roles.Select(r => new RoleModel
            {
                Id = r.Id,
                NameAR = r.NameAR,
                NameEN = r.NameEN,
                Permissions = r.Permissions.Select(p => new PermissionModel
                {
                    Id = p.Id,
                    Action = p.Action
                }).ToList(),
                CreatedOn = r.CreatedOn,
                UpdatedOn = r.UpdatedOn
            }).ToList();
        }

        public async Task<RoleModel> GetRoleById(int id) 
        {
            Role role = await _repository.GetById(id);
            if (role == null)
                return null;
            return new RoleModel
            {
                Id = role.Id,
                NameAR = role.NameAR,
                NameEN = role.NameEN,
                Permissions = role.Permissions.Select( p =>new PermissionModel
                {
                    Id= p.Id,
                    Action = p.Action
                }).ToList(),
                UpdatedOn = role.UpdatedOn,
                CreatedOn = role.CreatedOn
            };
        }

        public async Task AssignPermissions(RolePermissionsCommand command)
        {
            await _repository.AssignPermissions(command.PermissionIds, command.RoleId);
        }
    }
}
