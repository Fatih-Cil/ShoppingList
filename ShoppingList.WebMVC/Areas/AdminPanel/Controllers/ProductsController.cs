using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ShoppingList.Domain.Entities;
using ShoppingList.WebMVC.Areas.AdminPanel.Models.CategoryVM;
using ShoppingList.WebMVC.Areas.AdminPanel.Models.ProductVM;
using ShoppingList.WebMVC.Models;
using System.Net;
using System.Text;

namespace ShoppingList.WebMVC.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ProductsController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var httpClient = new HttpClient();

            var responseMessage = await httpClient.GetAsync("https://localhost:44344/api/Products");
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
        public async Task<IActionResult> Add(AddProductVM addProductVM)
        {
            // TempData["Error"] = addProductVM.Category.Id + addProductVM.Name + addProductVM.UrlImage;

            addProductVM.CategoryId = addProductVM.Category.Id;
            string url = "https://localhost:44344/api/Products/";
            HttpClient client = new HttpClient();
            var jsonCategory = JsonConvert.SerializeObject(addProductVM);
            var content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
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

            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                TempData["Error"] = errorMessage;
                return RedirectToAction("Add", "Products");
            }

            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
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

            var responseMessage2 = await httpClient.GetAsync("https://localhost:44344/api/Products/"+id);
            var jsonString2 = await responseMessage2.Content.ReadAsStringAsync();
            var values2 = JsonConvert.DeserializeObject<AddProductVM>(jsonString2);

            UpdateProductVM updateProductVM = new UpdateProductVM();
            updateProductVM.Name = values2.Name;
            updateProductVM.UrlImage=values2.UrlImage;
            updateProductVM.id=id;
            updateProductVM.CategoryList = values;

           //  TempData["Error"] =values2.UrlImage;

            return View(updateProductVM);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateProductVM updateProductVM)
        {
            updateProductVM.CategoryId = updateProductVM.Category.Id;
            string url = "https://localhost:44344/api/products/" + updateProductVM.id;
            HttpClient client = new HttpClient();
            var jsonCategory = JsonConvert.SerializeObject(updateProductVM);
            var content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Products");

            }
            var httpClient = new HttpClient();

            var responseMessage = await httpClient.GetAsync("https://localhost:44344/api/Categories");
            var jsonString2 = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<Category>>(jsonString2);

            updateProductVM.CategoryList = values;

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
