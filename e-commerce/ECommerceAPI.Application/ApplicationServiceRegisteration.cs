using eCommerceAPI.Application.Services;
using eCommerceAPI.Domain.Interfaces.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application
{
    public static class ApplicationServiceRegisteration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        { 
            services.AddScoped<UserService>();
            services.AddScoped<CareerService>();
            services.AddScoped<RoleService>();
            services.AddScoped<AddressService>();
            services.AddScoped<BrandService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<CustomerService>();
            services.AddScoped<OrderItemService>();
            services.AddScoped<OrderService>();
            services.AddScoped<ProductService>();
            services.AddScoped<ReviewService>();
            services.AddScoped<WishlistService>();
            services.AddScoped<CompanyInfoService>();
            services.AddScoped<AuthService>();
            services.AddScoped<PermissionService>();
            services.AddScoped<ArticleService>();
            services.AddScoped<PolicyService>();
            services.AddScoped<RequestService>();
            services.AddScoped<SliderService>();

            services.AddScoped<IImageService, ImageService>();

            services.AddTransient<IEmailService, EmailService>();
        }

    }
}
