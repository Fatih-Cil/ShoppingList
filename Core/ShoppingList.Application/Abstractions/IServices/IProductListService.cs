using ShoppingList.Application.DTOs;
using ShoppingList.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Application.Abstractions.IServices
{
    public interface IProductListService
    {
        List<ProductList> GetAll();
        ProductList GetById(int Id);
        List<ProductList> GetAllProductListByListId(int listId);

        public List<ProductListDetailDTO> GetAllProductListDetailId(int listId);
        (ProductList, int kod, string message) Add(ProductList productList);
        bool Delete(ProductList productList);
        (ProductList, int kod, string message) Update(ProductList productList);
    }
}
