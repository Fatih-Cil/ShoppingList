using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShoppingList.WebMVC.Areas.AdminPanel.Models.ProductVM
{
    public class AddProductVM
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string UrlImage { get; set; }
        public List<SelectListItem> CategoryList { get; set; }
    }

    
}
