using ShoppingList.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Application.Abstractions.IServices
{
    public interface IProductService
    {
        List<Product> GetAll();
        Product GetById(int Id);
        Product GetByName(string name);
        List<Product> GetProductByCategoryId(int Id);
        (Product, int kod, string message) Add(Product product);
        bool Delete(Product product);
        (Product, int kod, string message) Update(Product product);
    }
}
