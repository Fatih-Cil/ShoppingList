using ShoppingList.Application.Abstractions.IRepositories;
using ShoppingList.Application.Abstractions.IServices;
using ShoppingList.Domain.Entities;
using ShoppingList.Persistence.Concrate.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Persistence.Concrate.Managers
{
    public class ProductManager : IProductService
    {
        IProductRepository _productRepository;
        ICategoryService _categoryService;

        public ProductManager(IProductRepository repository, ICategoryService categoryService)
        {
            _productRepository = repository;
            _categoryService = categoryService;
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product GetById(int Id)
        {
            return _productRepository.Get(x => x.Id == Id);
        }

        public Product GetByName(string name)
        {
            return _productRepository.Get(x => x.Name.ToUpper() == name.ToUpper());
        }

        public List<Product> GetProductByCategoryId(int Id)
        {
            return _productRepository.GetAll(x => x.CategoryId == Id);
        }

        public (Product, int kod, string message) Add(Product product)
        {
            var result = _productRepository.Get(x =>x.Name.ToUpper()==product.Name.ToUpper());

            if (result != null)
            {
                return (result, 0, "Bu ürün adı daha önce kullanılmış.");
            }

            Category category = _categoryService.GetById(product.CategoryId);
            if (category == null) return (product, 0, "Bu id'ye ait bir kategori bulunamadı");

            if (!_productRepository.Add(product))
            {
                return (product, 500, "Sunucu hatası! Güncelleme yapılamadı");
            }
            else
            {
                return (product, 201, "Kayıt başarılı");

            }
        }

        public bool Delete(Product product)
        {
            return _productRepository.Delete(product);
        }



        public (Product, int kod, string message) Update(Product product)
        {
            Product result = GetByName(product.Name);
            if (result != null && result.Id != product.Id)
            {
                return (product, 0, "Bu ürün adı daha önce kullanılmıştır");
            }


            Category category = _categoryService.GetById(product.CategoryId);

            if (category == null)
            {
                return (product, 0, "Bu id'ye ait bir kategori bulunamadı");
            }

            if (!_productRepository.Update(product))
            {
                return (product, 500, "Sunucu hatası! Güncelleme yapılamadı");
            }
            else
            {
                return (product, 200, "Güncelleme başarılı");

            }
        }
    }
}
