using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ShoppingList.Domain.Entities;
using ShoppingList.WebMVC.Areas.AdminPanel.Models.CategoryVM;
using ShoppingList.WebMVC.Areas.AdminPanel.Models.ProductVM;
using ShoppingList.WebMVC.Models;
using System.Net;
using System.Net.Http;
using System.Text;

namespace ShoppingList.WebMVC.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ProductsController : Controller
    {
        IWebHostEnvironment _env;

        public ProductsController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var httpClient = new HttpClient();

            var responseMessage = await httpClient.GetAsync("https://localhost:44344/api/Products");

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TempData["Error"] = "Hüç ürün eklenmediği için lütfen ürün ekleyiniz";
                return RedirectToAction("Add", "Products");

            }
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ProductDetailDTOVM>>(jsonString);
            
            return View(values);

        }

        [HttpGet("Admin/Products/Add")]
        public async Task<IActionResult> Add()
        {

            var httpClient = new HttpClient();

            var responseMessage = await httpClient.GetAsync("https://localhost:44344/api/Categories");
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<Category>>(jsonString);

            AddProductVM addProductVM = new AddProductVM();
            addProductVM.CategoryList = values;

            //  TempData["CategoryList"] = new SelectList(values, "Id", "Name");

            return View(addProductVM);

        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProductVM addProductVM, IFormFile uploadPhoto)
        {
         
            addProductVM.CategoryId = addProductVM.Category.Id;

            if (uploadPhoto != null && uploadPhoto.Length > 0)
            {
                addProductVM.UrlImage = Path.Combine("/Images", uploadPhoto.FileName);
                addProductVM.UrlImage = addProductVM.UrlImage.Replace("\\", "/");

            }
            else addProductVM.UrlImage = "";

            

            string url = "https://localhost:44344/api/Products/";
            HttpClient client = new HttpClient();
            var jsonCategory = JsonConvert.SerializeObject(addProductVM);
            var content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {

                if (uploadPhoto != null)
                {

                    string uploadPath = Path.Combine(_env.WebRootPath, "Images", uploadPhoto.FileName);

                    using (var stream = new FileStream(uploadPath, FileMode.Create))
                    {
                        await uploadPhoto.CopyToAsync(stream);

                    }

                }

                return RedirectToAction("Index", "Products");

            }

            else if (response.StatusCode == HttpStatusCode.BadRequest || addProductVM.CategoryId == 0)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var validation = JsonConvert.DeserializeObject<ErrorResponse>(jsonString);
                foreach (var key in validation.errors.Keys)
                {
                    foreach (var error in validation.errors[key])
                    {
                        ModelState.AddModelError(key, error);

                    }

                }

                var httpClient = new HttpClient();

                var responseMessage = await httpClient.GetAsync("https://localhost:44344/api/Categories");
                var jsonString2 = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<Category>>(jsonString2);

                addProductVM.CategoryList = values;

                return View(addProductVM);

            }

            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound || response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                TempData["Error"] = errorMessage;
                return RedirectToAction("Add", "Products");
            }

            return RedirectToAction("Add", "Products");
        }

        [HttpGet("Admin/Products/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetStringAsync("https://localhost:44344/api/Products/" + id);
            var values = JsonConvert.DeserializeObject<ProductDetailDTOVM>(responseMessage);

            return View(values);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductDetailDTOVM productDetailDTOVM)
        {


            string url = "https://localhost:44344/api/Products/" + productDetailDTOVM.Id;
            HttpClient client = new HttpClient();
            var jsonCategory = JsonConvert.SerializeObject(productDetailDTOVM);
            var content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
            var response = await client.DeleteAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return RedirectToAction("Index", "Products");

            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                TempData["Error"] = errorMessage;
                return RedirectToAction("Index", "Products");
            }

            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                TempData["Error"] = errorMessage;
                return View(productDetailDTOVM);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var validation = JsonConvert.DeserializeObject<ErrorResponse>(jsonString);
                foreach (var key in validation.errors.Keys)
                {
                    foreach (var error in validation.errors[key])
                    {
                        ModelState.AddModelError(key, error);
                    }

                }

                return View(productDetailDTOVM);
            }
            return View(productDetailDTOVM);

        }


        [HttpGet("Admin/Products/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var httpClient = new HttpClient();

            var responseMessage = await httpClient.GetAsync("https://localhost:44344/api/Categories/");
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<Category>>(jsonString);

            var responseMessage2 = await httpClient.GetAsync("https://localhost:44344/api/Products/" + id);
            var jsonString2 = await responseMessage2.Content.ReadAsStringAsync();
            var values2 = JsonConvert.DeserializeObject<AddProductVM>(jsonString2);

            UpdateProductVM updateProductVM = new UpdateProductVM();
            updateProductVM.Name = values2.Name;
            updateProductVM.UrlImage = values2.UrlImage;
            updateProductVM.id = id;
            updateProductVM.CategoryList = values;

            //  TempData["Error"] =values2.UrlImage;

            return View(updateProductVM);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateProductVM updateProductVM, IFormFile uploadPhoto)
        {
            
            string url = "https://localhost:44344/api/products/" + updateProductVM.id;

            if (uploadPhoto!=null && uploadPhoto.Length>0)
            {
                updateProductVM.UrlImage = Path.Combine("/Images", uploadPhoto.FileName);
                updateProductVM.UrlImage = updateProductVM.UrlImage.Replace("\\", "/");

            }
            else updateProductVM.UrlImage = "";
            

            HttpClient client = new HttpClient();
            var jsonProduct = JsonConvert.SerializeObject(updateProductVM);
            var content = new StringContent(jsonProduct, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {

                if (uploadPhoto != null)
                {

                    string uploadPath = Path.Combine(_env.WebRootPath, "Images", uploadPhoto.FileName);

                    using (var stream = new FileStream(uploadPath, FileMode.Create))
                    {
                        await uploadPhoto.CopyToAsync(stream);

                    }

                }
                return RedirectToAction("Index", "Products");

            }



            var httpClient = new HttpClient();

            var responseMessage2 = await httpClient.GetAsync("https://localhost:44344/api/Categories");
            var jsonString2 = await responseMessage2.Content.ReadAsStringAsync();
            var values2 = JsonConvert.DeserializeObject<List<Category>>(jsonString2);

            //var responseMessage3 = await httpClient.GetAsync("https://localhost:44344/api/Products/" + updateProductVM.id);
            //var jsonString3 = await responseMessage3.Content.ReadAsStringAsync();
            //var values3= JsonConvert.DeserializeObject<AddProductVM>(jsonString3);

            //UpdateProductVM updateProductVM2 = new UpdateProductVM();
            //updateProductVM2.Name = values3.Name;
            //updateProductVM2.UrlImage = values3.UrlImage;
            //updateProductVM2.id = updateProductVM.id;
            updateProductVM.CategoryList = values2;


            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                TempData["Error"] = errorMessage;
                return View(updateProductVM);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var validation = JsonConvert.DeserializeObject<ErrorResponse>(jsonString);
                foreach (var key in validation.errors.Keys)
                {
                    foreach (var error in validation.errors[key])
                    {
                        ModelState.AddModelError(key, error);
                    }

                }

                return View(updateProductVM);
            }
            return View(updateProductVM);
        }
    }
}
