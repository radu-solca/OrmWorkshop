using eShop.DataAccess.EntityFramework;
using eShop.DataAccess.EntityFramework.Context;
using eShop.Domain;
using eShop.Domain.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace eShop.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // EF
            services.AddDbContext<OnlineShopContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));
            services.AddTransient(typeof(IRepository<Customer>), typeof(EFCustomerRepository));
            services.AddTransient(typeof(IRepository<Product>), typeof(EFProductRepository));
            services.AddTransient(typeof(IRepository<Order>), typeof(EFOrderRepository));
            services.AddTransient(typeof(IRepository<OrderItem>), typeof(EFOrderItemRepository));

            // Dapper
            // services.AddTransient(typeof(IRepository<Customer>), typeof(CustomerRepository));
            // services.AddTransient(typeof(IRepository<Order>), typeof(OrderRepository));

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc();
        }
    }
}
