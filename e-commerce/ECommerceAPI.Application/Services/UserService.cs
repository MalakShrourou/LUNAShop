using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eCommerceAPI.Application.Commands;
using eCommerceAPI.Application.Models;
using eCommerceAPI.Application.Queries;
using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Interfaces.Repository;
using Microsoft.Extensions.Configuration;
using NETCore.Encrypt;

namespace eCommerceAPI.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repository;
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;

        public UserService(IUserRepository userRepository, IConfiguration configuration, AuthService authService)
        {
            _repository = userRepository;
            _configuration = configuration;
            _authService = authService;
        }

        public async Task<int> AddUser(UserCommand command)
        {
            string key = _configuration["EncryptionKey"];
            string encryptedPassword = EncryptProvider.DESEncrypt(command.Password, key);
            User user = new User()
            {
                FNameAR = command.FNameAR,
                LNameAR = command.LNameAR,
                FNameEN = command.FNameEN,
                LNameEN = command.LNameEN,
                Email = command.Email,
                Password = encryptedPassword,
                Phone = command.Phone,
                RoleId = command.RoleId,
                CreatedOn = DateTime.Now
            };
            return await _repository.Create(user);
        }

        public async Task UpdateUser(UserCommand command, int id)
        {
            User user = await _repository.GetById(id);
            if (user == null)
                throw new Exception("User not found");
            user.FNameAR = command.FNameAR;
            user.LNameAR = command.LNameAR;
            user.FNameEN = command.FNameEN;
            user.LNameEN = command.LNameEN;
            user.Email = command.Email;
            user.Password = user.Password;
            user.Phone = command.Phone;
            user.RoleId = command.RoleId;
            user.UpdatedOn = DateTime.Now;
            await _repository.Update(user);
        }

        public async Task DeleteUser(int id)
        {
            await _repository.Delete(id);
        }

        public async Task<IList<UserModel>> GetAllUsers()
        {
            IList<User> users = await _repository.GetAll();
            return users.Select(u => new UserModel
            {
                Id = u.Id,
                FNameAR = u.FNameAR,
                LNameAR = u.LNameAR,
                FNameEN = u.FNameEN,
                LNameEN = u.LNameEN,
                Phone = u.Phone,
                Email = u.Email,
                RoleId = u.RoleId,
                RoleName = u.Role.NameEN,
                CreatedOn = u.CreatedOn,
                UpdatedOn = u.UpdatedOn
            }).ToList();
        }

        public async Task<UserModel> GetUserById(int id)
        {
            User user = await _repository.GetById(id);
            if (user == null)
                return null;
            return new UserModel
            {
                Id = user.Id,
                FNameAR = user.FNameAR,
                LNameAR = user.LNameAR,
                FNameEN = user.FNameEN,
                LNameEN = user.LNameEN,
                Phone = user.Phone,
                Email = user.Email,
                RoleId = user.RoleId,
                RoleName = user.Role != null ? user.Role.NameEN : null, // Access role name directly
                UpdatedOn = user.UpdatedOn,
                CreatedOn = user.CreatedOn
            };
        }

        public async Task<AuthenticationModel> Login(AuthenticationQuery query)
        {
            User user = await _repository.Login(query.Email);
            if (user == null)
                return null;
            string key = _configuration["EncryptionKey"];
            string decrtyptedPassowrd = EncryptProvider.DESDecrypt(user.Password, key);
            if (decrtyptedPassowrd != query.Password)
                throw new Exception("Wrong password");
            AuthenticationModel authenticationModel = new AuthenticationModel()
            {
                Id = user.Id,
                AccessToken = await _authService.GenerateTokenAsync(user.Role, user.Id, query.RememberMe),
                ExpiresAt = DateTime.Now.AddHours(1)
            };
            return authenticationModel;
        }

        public async Task<bool> ChangePassword(ChangePasswordCommand command)
        {
            string key = _configuration["EncryptionKey"];

            User user = await _repository.GetById(command.Id);
            string decryptedPassword = EncryptProvider.DESDecrypt(user.Password, key);

            if (decryptedPassword == command.CurrentPassword)
            {
                string encryptedNewPassword = EncryptProvider.DESEncrypt(command.NewPassword, key);
                return await _repository.ChangePassword(command.Id, encryptedNewPassword);
            }

            return false;
        }

        public async Task<IList<UserModel>> GetCustomerServiceTeam()
        {
            IList<User> users = await _repository.GetCustomerServiceTeam();
            return users.Select(u => new UserModel
            {
                Id = u.Id,
                FNameAR = u.FNameAR,
                LNameAR = u.LNameAR,
                FNameEN = u.FNameEN,
                LNameEN = u.LNameEN,
                Phone = u.Phone,
                Email = u.Email,
                RoleId = u.RoleId,
                RoleName = u.Role.NameEN,
                CreatedOn = u.CreatedOn,
                UpdatedOn = u.UpdatedOn
            }).ToList();
        }

    }
}

        