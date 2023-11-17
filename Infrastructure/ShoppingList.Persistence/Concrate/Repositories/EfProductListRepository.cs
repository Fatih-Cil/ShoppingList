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
    public class EfProductListRepository : EfEntityRepositoryBase<ProductList, ShoppingListContext>, IProductListRepository
    {
        public List<ProductListDetailDTO> GetAllProductListDetails(int listId)
        {
            using (var context = new ShoppingListContext())
            {
                var result = from productlist in context.ProductLists
                             join product in context.Products
                             on productlist.ProductId equals product.Id
                             join list in context.Lists
                             on productlist.ListId equals list.Id
                             select new ProductListDetailDTO
                             {
                                 Id = productlist.Id,
                                 ProductId = productlist.ProductId,
                                 ListId = productlist.ListId,
                                 Description = productlist.Description,
                                 ProductName=product.Name,
                                 ProductImageUrl=product.UrlImage,
                                 Status = productlist.Status,
                                 ListName=list.Name
                                 

                             };
                return result.Where(x=>x.ListId==listId).ToList();
            }
        }
    }
}
