using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingList.Application.Abstractions.IServices;
using ShoppingList.Domain.Entities;

namespace ShoppingList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {

            List<Product> result = _productService.GetAll();

            return (result.Count == 0)
                 ? NotFound("Ürün bulunamadı")
                 : Ok(result);

        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {

            Product result = _productService.GetById(id);

            return (result == null)
                 ? NotFound("Ürün bulunamadı")
                 : Ok(result);

        }

        

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            Product product = _productService.GetById(id);
            if (product == null) return NotFound("Ürün bulunamadı");

            return (_productService.Delete(product))
                ? NoContent()
                : StatusCode(StatusCodes.Status500InternalServerError);

        }
    }
}
