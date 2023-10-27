using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingList.Application.Abstractions.IServices;
using ShoppingList.Application.DTOs;
using ShoppingList.Application.ViewModels.CategoryViewModel;
using ShoppingList.Application.ViewModels.ProductViewModel;
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

        //[Authorize(Roles = "Admin,User")]
        [HttpGet]
        public IActionResult GetAll()
        {

            List<ProductDetailDTO> result = _productService.GetAll();

            return (result.Count == 0)
                 ? NotFound("Ürün bulunamadı")
                 : Ok(result);

        }

        //[Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {

            Product result = _productService.GetById(id);

            return (result == null)
                 ? NotFound("Ürün bulunamadı")
                 : Ok(result);

        }

        //[Authorize(Roles = "Admin,User")]
        [HttpGet("ProductDatail/{id}")]
        public IActionResult GetBydDetailId(int id)
        {

            ProductDetailDTO result = _productService.GetByDetailId(id);

            return (result == null)
                 ? NotFound("Ürün bulunamadı")
                 : Ok(result);

        }


        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Product product = _productService.GetById(id);
            if (product == null) return NotFound("Ürün bulunamadı");

            return (_productService.Delete(product))
                ? NoContent()
                : StatusCode(StatusCodes.Status500InternalServerError);

        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Add(AddProductViewModel addProductViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product product = new Product();
            product.CategoryId = addProductViewModel.CategoryId;
            product.Name = addProductViewModel.Name;
            product.UrlImage = addProductViewModel.UrlImage;

            var result = _productService.Add(product);

            if (result.kod == 0) return NotFound(result.message);

            if (result.kod == 500) return StatusCode(StatusCodes.Status500InternalServerError);

            return CreatedAtAction(nameof(GetById), new { id = result.Item1.Id }, addProductViewModel);
        }

     //[Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateProductViewModel updateProductViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Product product = _productService.GetById(id);
            if (product == null) return NotFound("Ürün Bulunamadı");

            product.Id = id;
            product.CategoryId = updateProductViewModel.CategoryId;
            product.Name = updateProductViewModel.Name;
            product.UrlImage = updateProductViewModel.UrlImage;

            var result = _productService.Update(product);
            if (result.kod == 0) return NotFound(result.message);

            if (result.kod == 500) return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok();
        }
    }
}
