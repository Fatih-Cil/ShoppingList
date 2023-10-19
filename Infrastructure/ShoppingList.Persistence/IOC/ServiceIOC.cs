using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ShoppingList.Application.Abstractions.IRepositories;
using ShoppingList.Application.Abstractions.IServices;
using ShoppingList.Persistence.Concrate.Managers;
using ShoppingList.Persistence.Concrate.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Persistence.IOC
{
    public static class ServiceIOC
    {
        public static void AddPersistenceServiceIOC(this IServiceCollection services)
        {
            services.AddSingleton<IAuthService, AuthManager>();

            services.AddSingleton<ICategoryService, CategoryManager>();
            services.AddSingleton<ICategoryRepository, EfCategoryRepository>();

            services.AddSingleton<IUserService, UserManager>();
            services.AddSingleton<IUserRepository, EfUserRepository>();

            services.AddSingleton<IProductService, ProductManager>();
            services.AddSingleton<IProductRepository, EfProductRepository>();
        }
    }
}
