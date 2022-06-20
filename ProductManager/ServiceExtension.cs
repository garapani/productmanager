using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManager.Data;
using ProductManager.Repositories;
using ProductManager.Repositories.Interfaces;
using System;
using System.Reflection;

namespace ProductManager
{
    public static class ServiceExtension
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutomapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            mapperConfig.AssertConfigurationIsValid();
            services.AddSingleton(mapper);            
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddHttpContextAccessor();
        }

        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<ProductDBContext>(options =>
            //        options.UseSqlServer(configuration.GetConnectionString("ProductDBContext")));
            var contentRoot = configuration.GetValue<string>(WebHostDefaults.ContentRootKey);
            string dbPath = contentRoot + @"\..\data\Database.sqlite";
            services.AddDbContext<ProductDBContext>(options => options.UseSqlite($"Data Source={dbPath}"));
        }

        public static void AddSharedInfrastructure(this IServiceCollection services)
        {
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient<IProductRepositoryAsync, ProductRepositoryAsync>();
            services.AddTransient<IProductAnalyticsRepositoryAsync, ProductAnalyticsRepositoryAsync>();
        }
    }
}
