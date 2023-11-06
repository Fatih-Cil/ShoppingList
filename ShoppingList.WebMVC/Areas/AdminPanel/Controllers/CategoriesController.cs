using Azure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using ShoppingList.Domain.Entities;
using ShoppingList.WebMVC.Areas.AdminPanel.Models.CategoryVM;
using ShoppingList.WebMVC.Models;

using System.Net;
using System.Text;



namespace ShoppingList.WebMVC.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class CategoriesController : Controller
    {

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var httpClient = new HttpClient();

            var responseMessage = await httpClient.GetAsync("https://localhost:44344/api/Categories");

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TempData["Error"] = "Daha önce kategori eklenmediği için lütfen önce kategori ekleyiniz";
                return RedirectToAction("Add", "Categories");

            }
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<Category>>(jsonString);
            return View(values);

        }


        [HttpGet("Admin/Categories/Add")]
        public async Task<IActionResult> Add()
        {

            
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCategoryVM addCategoryVM)
        {

            string url = "https://localhost:44344/api/Categories/";
            HttpClient client = new HttpClient();
            var jsonCategory = JsonConvert.SerializeObject(addCategoryVM);
            var content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Categories");

            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                TempData["Error"] = errorMessage;
                return View(addCategoryVM);
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

                return View(addCategoryVM);
            }
            


            return View(addCategoryVM);

        }


        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCategoryVM updateCategoryVM)
        {
            string url= "https://localhost:44344/api/Categories/" + updateCategoryVM.Id;
            HttpClient client = new HttpClient();
            var jsonCategory=JsonConvert.SerializeObject(updateCategoryVM);
            var content=new StringContent(jsonCategory,Encoding.UTF8,"application/json");
            var response= await client.PutAsync(url, content);
            

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index","Categories");

            }
            else if (response.StatusCode==System.Net.HttpStatusCode.NotFound)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                TempData["Error"] = errorMessage;
                return View(updateCategoryVM);
            }

            else if (response.StatusCode==HttpStatusCode.BadRequest)
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
               
                return View(updateCategoryVM);
            }
            return View(updateCategoryVM);

        }
        
        
        [HttpGet("Admin/Categories/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {

            TempData["id"]=id;
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetStringAsync("https://localhost:44344/api/Categories/"+id);
            var values = JsonConvert.DeserializeObject<UpdateCategoryVM>(responseMessage);

            return View(values);
        }

        [HttpGet("Admin/Categories/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetStringAsync("https://localhost:44344/api/Categories/" + id);
            var values = JsonConvert.DeserializeObject<DeleteCategoryVM>(responseMessage);

            return View(values);

          

        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteCategoryVM deleteCategoryVM)
        {

            
            string url = "https://localhost:44344/api/Categories/"+deleteCategoryVM.Id;
            HttpClient client = new HttpClient();
            var jsonCategory = JsonConvert.SerializeObject(deleteCategoryVM);
            var content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
            var response = await client.DeleteAsync(url);
            

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return RedirectToAction("Index", "Categories");

            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                TempData["Error"] = errorMessage;
                return RedirectToAction("Index", "Categories");
            }

            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                TempData["Error"] = errorMessage;
                return View(deleteCategoryVM);
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

                return View(deleteCategoryVM);
            }
            return View(deleteCategoryVM);

            

        }
    }
}
