using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingList.Domain.Entities;

namespace ShoppingList.WebMVC.Areas.AdminPanel.Models.ProductVM
{
    public class AddProductVM
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string UrlImage { get; set; }
        public List<Category> CategoryList { get; set; }
        public Category Category { get; set; }
    }

    
}
