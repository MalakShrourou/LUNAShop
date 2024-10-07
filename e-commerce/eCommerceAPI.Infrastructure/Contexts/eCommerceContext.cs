using eCommerceAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Infrastructure.Contexts
{
    public class eCommerceContext : DbContext
    {
        private readonly IConfiguration configuration;
        public eCommerceContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Permission>().HasData(Enum.GetValues<Domain.Enums.Permission>()
                .Select(p => new Permission
                {
                    Id = (int)p,
                    Action = p.ToString()
                }));
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set;}
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Career> Careers {  get; set; }
        public DbSet<CompanyInfo> CompanyInfo { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Policy> Policies { get; set; }
    }
}
