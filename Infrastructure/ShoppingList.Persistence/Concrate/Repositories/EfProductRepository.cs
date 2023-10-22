using ShoppingList.Application.Abstractions.IRepositories;
using ShoppingList.Application.DTOs;
using ShoppingList.Domain.Entities;
using ShoppingList.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Persistence.Concrate.Repositories
{
    public class EfProductRepository : EfEntityRepositoryBase<Product, ShoppingListContext>, IProductRepository
    {
        public List<ProductDetailDTO> GetAllProductDetails()
        {
            using(var context = new ShoppingListContext())
            {
                var result = from product in context.Products
                             join catetory in context.Categories
                             on product.CategoryId equals catetory.Id
                             select new ProductDetailDTO
                             {
                                 Id=product.Id,
                                 CategoryId=catetory.Id,
                                 Name=product.Name,
                                 UrlImage=product.UrlImage,
                                 CategoryName=catetory.Name

                             };
                return result.ToList();
            }
        }

        public ProductDetailDTO GetProductDetail(int id)
        {
            using (var context = new ShoppingListContext())
            {
                var result = from product in context.Products
                             join catetory in context.Categories
                             on product.CategoryId equals catetory.Id
                             select new ProductDetailDTO
                             {
                                 Id = product.Id,
                                 CategoryId = catetory.Id,
                                 Name = product.Name,
                                 UrlImage = product.UrlImage,
                                 CategoryName = catetory.Name

                             };
                return result.Where(x => x.Id == id).SingleOrDefault();
            }
        }
    }
}
