using ShoppingList.Application.Abstractions.IRepositories;
using ShoppingList.Application.Abstractions.IServices;
using ShoppingList.Application.DTOs;
using ShoppingList.Domain.Entities;
using ShoppingList.Persistence.Concrate.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Persistence.Concrate.Managers
{
    public class ProductListManager : IProductListService
    {
        IProductListRepository _productListRepository;
        IProductService _productService;
        IListService _listService;

        public ProductListManager(IProductListRepository productListRepository, IProductService productService, IListService listService)
        {
            _productListRepository = productListRepository;
            _productService = productService;
            _listService = listService;
        }

        public (ProductList, int kod, string message) Add(ProductList productList)
        {
            List list = _listService.GetById(productList.ListId);
            Product product= _productService.GetById(productList.ProductId);
            if (list==null || product==null)
            {
                return (productList, 0, "Ürün veya liste bilgisi eşleşmiyor");
            }

            if (!_productListRepository.Add(productList))
            {
                return (productList, 500, "Sunucu hatası! Güncelleme yapılamadı");
            }
            else
            {
                return (productList, 201, "Kayıt başarılı");

            }
        }

        public bool Delete(ProductList productList)
        {
            return _productListRepository.Delete(productList);
        }

        public List<ProductList> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<ProductList> GetAllProductListByListId(int listId)
        {
            return _productListRepository.GetAll(x => x.ListId == listId);
        }


        public List<ProductListDetailDTO> GetAllProductListDetailId(int listId)
        {
            return _productListRepository.GetAllProductListDetails(listId);
        }

        public ProductList GetById(int Id)
        {
            return _productListRepository.Get(x => x.Id == Id);
        }



        public (ProductList, int kod, string message) Update(ProductList productList)
        {

            List list = _listService.GetById(productList.ListId);
            Product product = _productService.GetById(productList.ProductId);
            if (list == null || product == null)
            {
                return (productList, 0, "Ürün veya liste bilgisi eşleşmiyor");
            } 

            if (!_productListRepository.Update(productList))
            {
                return (productList, 500, "Sunucu hatası! Güncelleme yapılamadı");
            }
            else
            {
                return (productList, 201, "Kayıt başarılı");

            }
        }
    }
}
