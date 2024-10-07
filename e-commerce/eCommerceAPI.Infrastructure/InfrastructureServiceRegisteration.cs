using eCommerceAPI.Domain.Entities;
using eCommerceAPI.Domain.Interfaces.Repository;
using eCommerceAPI.Infrastructure.Authorization;
using eCommerceAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Infrastructure
{
    public static class InfrastructureServiceRegisteration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRepository<Address>, AddressRepository>();
            services.AddScoped<IRepository<Article>, ArticleRepository>();
            services.AddScoped<IRepository<Request>, RequestRepository>();
            services.AddScoped<IRepository<Policy>, PolicyRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IRepository<Review>, ReviewRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRepository<Career>, CareerRepository>();
            services.AddScoped<ISliderRepository, SliderRepository>();
            services.AddScoped<IWishlistRepository, WishlistRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICompanyInfoRepository, CompanyInfoRepository>();

            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        }
    }
}
