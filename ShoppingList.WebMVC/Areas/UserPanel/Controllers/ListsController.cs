using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppingList.Domain.Entities;
using ShoppingList.WebMVC.Areas.AdminPanel.Models.CategoryVM;
using ShoppingList.WebMVC.Areas.AdminPanel.Models.ProductVM;
using ShoppingList.WebMVC.Areas.UserPanel.Models.HomeVM;
using ShoppingList.WebMVC.Areas.UserPanel.Models.ListsVM;
using ShoppingList.WebMVC.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using static System.Net.WebRequestMethods;

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
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Index", new { name, updateProductListVM.listid, updateProductListVM.listname });

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
                        TempData["ModelError"]=error;
                    }

                }

                return RedirectToAction("Index", new { name, updateProductListVM.listid, updateProductListVM.listname });
            }
            return RedirectToAction("Index", new { name, updateProductListVM.listid, updateProductListVM.listname });
          
        }

        [HttpPost]
        public async Task<IActionResult> ShoppingGo(int listid, string listname)
        {
            if (!SessionCheck())
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            string name = null;
            string url = "https://localhost:44344/api/Lists/" + listid;

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(responsejwt);
            var roleSid = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid);
                    
            UpdateListVM updateListVM = new UpdateListVM();
            updateListVM.Userid =Convert.ToInt32(roleSid.Value);
            updateListVM.Name = listname;
            updateListVM.Status = false;
           

                


            HttpClient client = new HttpClient();
            var jsonCategory = JsonConvert.SerializeObject(updateListVM);
            var content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(url, content);

           
            return RedirectToAction("Listview", new { name, listid, listname });
            


        }


        public async Task<IActionResult> ListView(int listid,string listname)
        {
            ViewBag.listid = listid;
            ViewBag.listname = listname;
           

            string url = "https://localhost:44344/api/ProductLists/ProductListDetail/" + listid;
            HttpClient client = new HttpClient();

            var responseMessage = await client.GetAsync(url);

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {

                return RedirectToAction("Index", "Home");

            }
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            List<ListViewVM> values = JsonConvert.DeserializeObject<List<ListViewVM>>(jsonString);


            return View(values);

        }

        [HttpPost]
        public async Task<IActionResult> ShoppingFinish(int listid, string listname)
        {
            if (!SessionCheck())
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            string name = null;
            string url = "https://localhost:44344/api/Lists/" + listid;

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(responsejwt);
            var roleSid = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid);

            UpdateListVM updateListVM = new UpdateListVM();
            updateListVM.Userid = Convert.ToInt32(roleSid.Value);
            updateListVM.Name = listname;
            updateListVM.Status = true;

            HttpClient client = new HttpClient();
            var jsonCategory = JsonConvert.SerializeObject(updateListVM);
            var content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(url, content);



            string urlupdate = "https://localhost:44344/api/ProductLists/ProductListDetail/" + listid;
            

            var responseMessage = await client.GetAsync(urlupdate);

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {

                return RedirectToAction("Index", "Home");

            }
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonString = await responseMessage.Content.ReadAsStringAsync();
                List<ListViewVM> values2 = JsonConvert.DeserializeObject<List<ListViewVM>>(jsonString);

                foreach (var item in values2)
                {
                    if (item.Status != true)
                    {
                        item.Status = true;
                       


                        string url2 = "https://localhost:44344/api/ProductLists/" + item.Id;
                        HttpClient client2 = new HttpClient();
                        var jsonCategory2 = JsonConvert.SerializeObject(item);
                        var content2 = new StringContent(jsonCategory2, Encoding.UTF8, "application/json");
                        var response2 = await client2.PutAsync(url2, content2);

                        
                    }

                }
            }
           
                        return RedirectToAction("Index", "Home", new { name, listid, listname });

        }

        //[HttpPost("user/lists/listupdate")]
        //public async List<IActionResult> ListUpdate(int id, int listid, string listname)
        //{
        //    string name = "";

        //    //string url2 = "https://localhost:44344/api/ProductLists/" + id;
        //    //HttpClient client2 = new HttpClient();
        //    //var responseMessage = await client2.GetAsync(url2);

        //    //var jsonString = await responseMessage.Content.ReadAsStringAsync();
        //    //UpdateProductListStatusVM values2 = JsonConvert.DeserializeObject<UpdateProductListStatusVM>(jsonString);

        //    //values2.Status = false;
        //    //var jsonCategory = JsonConvert.SerializeObject(values2);
        //    //var content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
        //    //var response = await client2.PutAsync(url2, content);

        //    return Ok();
        //}


        [HttpPost]
        public async Task<IActionResult> UpdateList(int id, string listname, string listid)
        {

            if (!SessionCheck())
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            string name = "";

            string url2 = "https://localhost:44344/api/ProductLists/" + id;
            HttpClient client2 = new HttpClient();
            var responseMessage = await client2.GetAsync(url2);

            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            UpdateProductListStatusVM values2 = JsonConvert.DeserializeObject<UpdateProductListStatusVM>(jsonString);

            values2.Status = false;


            values2.Status = false;
            var jsonCategory = JsonConvert.SerializeObject(values2);
            var content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
            var response = await client2.PutAsync(url2, content);


            return RedirectToAction("Listview", new { name, listid, listname });
        }

    }
}
