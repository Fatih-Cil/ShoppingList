using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using ShoppingList.WebMVC.Areas.UserPanel.Models.HomeVM;
using ShoppingList.WebMVC.Models;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace ShoppingList.WebMVC.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    public class HomeController : Controller
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
        public async Task<IActionResult> Index()
        {

            if (!SessionCheck())
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(responsejwt);
            var idClaim = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Sid);

            var httpClient = new HttpClient();
            AddJwtTokenToRequest(httpClient, responsejwt);

            ViewBag.Id = idClaim.Value.ToString();


            var responseList = await httpClient.GetAsync("https://localhost:44344/api/Lists?UserID=" + idClaim.Value);

            if (responseList.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TempData["Error"] = "Henüz bir liste oluşturmadınız.";

            }



            if (responseList.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonStringList = await responseList.Content.ReadAsStringAsync();
                var List = JsonConvert.DeserializeObject<List<ListViewModel>>(jsonStringList);

                foreach (var item in List)
                {
                    var responseProductList = await httpClient.GetAsync("https://localhost:44344/api/ProductLists?listid=" + item.Id.ToString());
                    if (responseProductList.StatusCode != System.Net.HttpStatusCode.NotFound)
                    {
                        var jsonStringProductList = await responseProductList.Content.ReadAsStringAsync();
                        var productList = JsonConvert.DeserializeObject<List<ProductViewModel>>(jsonStringProductList);
                        item.ProductCount = productList.Count();
                    }


                }

                return View(List);
            }


            List<ListViewModel> listViewModel = new List<ListViewModel>();
            ListViewModel list = new ListViewModel();

            list.status = true;
            list.Name = "";
            list.ProductCount = 0;
            listViewModel.Add(list);
            return View(listViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddListViewModel addListViewModel)
        {

            if (!SessionCheck())
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
            addListViewModel.Status = true;
            string url = "https://localhost:44344/api/Lists/";
            HttpClient client = new HttpClient();
            var jsonList = JsonConvert.SerializeObject(addListViewModel);
            var content = new StringContent(jsonList, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");

            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                TempData["Message"] = errorMessage;
                return RedirectToAction("Index", "Home");
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
                        TempData["Message"] = error;
                    }

                }



            }
            return RedirectToAction("Index", "Home");

        }



        [HttpGet("User/Home/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            if (!SessionCheck())
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }

            

            var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetStringAsync("https://localhost:44344/api/Lists/" + id);
            var values = JsonConvert.DeserializeObject<ListViewModel>(responseMessage);

            if (values.UserId !=Convert.ToInt32(TempData[id.ToString()]))
            {
                return RedirectToAction("Index", "Home");
            }


            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ListViewModel listViewModel)
        {

            if (!SessionCheck())
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }

            string url = "https://localhost:44344/api/Lists/" +listViewModel.Id;
            HttpClient client = new HttpClient();



            var response = await client.DeleteAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return RedirectToAction("Index", "Home");

            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();
                TempData["Error"] = errorMessage;
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
    }

}
