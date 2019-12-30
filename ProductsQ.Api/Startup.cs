using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Products.DAL.Abstraction;
using Products.DAL.Data;
using Products.DAL.Mapper;
using Products.DAL.Repositories;
using Products.ServiceBusMessaging;
using AutoMapper;
using Microsoft.OpenApi.Models;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ProductsQ.Api.Controllers;

namespace ProductsQ.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IContainer ApplicationContainer { get; private set; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson();
            services.AddDbContext<DataContext>(x => x.UseSqlServer(Configuration.GetConnectionString("ProductsDatabase")), ServiceLifetime.Singleton);

            services.AddAutoMapper(typeof(AutomapperProfile));

            services.AddScoped<ServiceBusSender>();
            services.AddSingleton<IProductsRepository, ProductRepository>();
            services.AddSingleton<IVendorRepository, VendorRepository>();
            services.AddSingleton<IProcessData, ProcessData>();
            services.AddSingleton<IServiceBusConsumer, ServiceBusConsumer>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Products API",
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            UpdateDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products Management API V1");
            });

            //var bus = app.ApplicationServices.GetService<IServiceBusConsumer>();
            //bus.RegisterOnMessageHandlerAndReceiveMessages();
            InvokeServiceBus(app);
        }
        private void InvokeServiceBus(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                var bus = serviceScope.ServiceProvider.GetService<IServiceBusConsumer>();
                bus.RegisterOnMessageHandlerAndReceiveMessages();
            }
        }
        private void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<DataContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
