using ShoppingList.Application.DTOs;
using ShoppingList.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Application.Abstractions.IRepositories
{
    public interface IProductRepository : IEntityRepository<Product>
    {
        List<ProductDetailDTO> GetAllProductDetails();
        ProductDetailDTO GetProductDetail(int id);
    }
}
