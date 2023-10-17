using ShoppingList.Application.Abstractions.IRepositories;
using ShoppingList.Application.Abstractions.IServices;
using ShoppingList.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Persistence.Concrate.Managers
{
    public class ProductManager: IProductService
    {
        IProductRepository _productRepository;

        public ProductManager(IProductRepository repository)
        {
            _productRepository = repository;
        }

        public List<Product> GetAll()
        {
           return _productRepository.GetAll();
        }

        public Product GetById(int Id)
        {
            return _productRepository.Get(x=> x.Id == Id);
        }

        public List<Product> GetByCategoryId(int Id)
        {
            return _productRepository.GetAll(x => x.CategoryId == Id);
        }

        public (Product, int kod, string message) Add(Product product)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Product product)
        {
            return _productRepository.Delete(product);
        }

       

        public (Product, int kod, string message) Update(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
