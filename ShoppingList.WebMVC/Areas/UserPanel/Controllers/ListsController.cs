using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppingList.Domain.Entities;
using ShoppingList.WebMVC.Areas.AdminPanel.Models.CategoryVM;
using ShoppingList.WebMVC.Areas.AdminPanel.Models.ProductVM;
using ShoppingList.WebMVC.Areas.UserPanel.Models.HomeVM;
using ShoppingList.WebMVC.Areas.UserPanel.Models.ListsVM;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace ShoppingList.WebMVC.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    public class ListsController : Controller
    {
        string responsejwt = "";
        private void AddJwtTokenToRequest(HttpClient httpClient, string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private bool SessionCheck()
        {
            responsejwt = HttpContext.Session.GetString("SessionUser");
            if (string.IsNullOrEmpty(responsejwt))
            {
                return false;
            }
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(responsejwt);
            var roleClaim = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);
            if (roleClaim == null || roleClaim.Value != "User")
            {

                return false;

            }
            return true;

        }
        
        public async Task<IActionResult> Index(string name = null,string listid=null, string listname=null)
        {

            MyListViewModel model = new MyListViewModel();
            var httpClient2 = new HttpClient();

            AddJwtTokenToRequest(httpClient2, responsejwt);

            var responseMessage2 = await httpClient2.GetAsync("https://localhost:44344/api/productlists/ProductListDetail/" + listid);
            if (responseMessage2.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TempData["ProductListMessage"] = "Listeye henüz bir ürün eklemediniz!";
                model.ProductListsVM = null;

            }
            else {
            var jsonString2 = await responseMessage2.Content.ReadAsStringAsync();
            List<ProductListsVM> values2 = JsonConvert.DeserializeObject<List<ProductListsVM>>(jsonString2);
                

                model.ProductListsVM = values2;
                
            }

            var httpClient = new HttpClient();

            AddJwtTokenToRequest(httpClient, responsejwt);

            var responseMessage = await httpClient.GetAsync("https://localhost:44344/api/Products");

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {

                return RedirectToAction("Index", "Home");

            }
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            List<ProductDetailDTOVM> values = JsonConvert.DeserializeObject<List<ProductDetailDTOVM>>(jsonString);

            
            
            model.ProductDetailDTOVM = values;


            if (name != null)
            {
                var resultData = values.Where(x => x.Name.ToLower().Contains(name.ToLower()));
                ViewBag.Name = name;
                ViewBag.ListName = listname;
                ViewBag.ListId = listid;
                model.ProductDetailDTOVM = resultData.ToList();
                return View(model);
                
            }

            ViewBag.ListName = listname;
            ViewBag.ListId = listid;

            return View(model);
           
            
        }

        [HttpPost]
        public async Task<ActionResult> Add(int id, int listid, string listname) 
        {
            string name = "";
            ProductListsVM list = new ProductListsVM();
            list.ProductId = id;
            list.ListId = listid;
            list.Description = "";
            list.Status = true;

            string url = "https://localhost:44344/api/ProductLists";

            string controlurl = "https://localhost:44344/api/ProductLists?listid=" + listid;

            HttpClient client = new HttpClient();

            var responseMessage = await client.GetAsync(controlurl);

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString2 = await responseMessage.Content.ReadAsStringAsync();
                List<ProductListsVM> values2 = JsonConvert.DeserializeObject<List<ProductListsVM>>(jsonString2);
                var productlist = values2.FirstOrDefault(x => x.ProductId == id);
                if (productlist is not null)
                {
                    TempData["ProductAddError"] = "Bu ürün listenizde zaten ekli.";
                    return RedirectToAction("Index", new { name, listid, listname });
                }
            }

            var jsonList = JsonConvert.SerializeObject(list);
            var content = new StringContent(jsonList, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
               
                return RedirectToAction("Index", new { name, listid, listname });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                TempData["Message"] = errorMessage;
                return RedirectToAction("Index", new { name, listid, listname });
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                TempData["Message"] = errorMessage;
                return RedirectToAction("Index", new { name, listid, listname });
            }

            return RedirectToAction("Index", new { name, listid, listname });

        }




        [HttpPost]
        public async Task<IActionResult> Delete(int id, string listname, string listid)
        {

            if (!SessionCheck())
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            string name = "";
            string url = "https://localhost:44344/api/ProductLists/" + id;
            HttpClient client = new HttpClient();

            var response = await client.DeleteAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return RedirectToAction("Index", new { name, listid, listname });

            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                TempData["Error"] = errorMessage;
                return RedirectToAction("Index", new { name, listid, listname });
            }
            return RedirectToAction("Index", new { name, listid, listname });
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductListVM updateProductListVM)
        {
            if (!SessionCheck())
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }

            string name = "";
            
            
                
                string url = "https://localhost:44344/api/ProductLists/" + updateProductListVM.id;
                
               
                

                HttpClient client = new HttpClient();
                var jsonCategory = JsonConvert.SerializeObject(updateProductListVM);
                var content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(url, content);

                return RedirectToAction("Index", new { name, updateProductListVM.listid, updateProductListVM.listname });
          

           ;

        }

    }
}
