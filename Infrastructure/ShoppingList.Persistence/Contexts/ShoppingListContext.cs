using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShoppingList.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Persistence.Contexts
{
    public class ShoppingListContext : DbContext
    {
        public ShoppingListContext()
        {

        }
        public ShoppingListContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //microsoft.extensions.configuration ve microsoft extensions configuration json paketlerini yüklemek gerekiyor.
            ConfigurationManager configurationManager = new();
            configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/ShoppingList.WebApi"));
            configurationManager.AddJsonFile("appsettings.json");
            optionsBuilder.UseSqlServer(configurationManager.GetConnectionString("SqlCon"));

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProductList> ProductLists { get; set; }
    }
}
