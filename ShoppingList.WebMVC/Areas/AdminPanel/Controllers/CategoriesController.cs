using Azure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppingList.Application.ViewModels.AuthViewModel;
using ShoppingList.Application.ViewModels.UserViewModel;
using ShoppingList.Domain.Entities;
using ShoppingList.WebMVC.Models;
using ShoppingList.WebMVC.Models.ViewModels.CategoryViewModel;
using System.Net;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;


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
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<Category>>(jsonString);
            return View(values);



        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCategoryViewModel updateCategoryViewModel)
        {
            string url= "https://localhost:44344/api/Categories/" + updateCategoryViewModel.Id;
            HttpClient client = new HttpClient();
            var jsonCategory=JsonConvert.SerializeObject(updateCategoryViewModel);
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
                return View(updateCategoryViewModel);
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
               
                return View(updateCategoryViewModel);
            }
            return View(updateCategoryViewModel);

        }

        
        [HttpGet("Admin/Categories/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {

            TempData["id"]=id;
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetStringAsync("https://localhost:44344/api/Categories/"+id);
            var values = JsonConvert.DeserializeObject<UpdateCategoryViewModel>(responseMessage);



            return View(values);
        }

    }
}
