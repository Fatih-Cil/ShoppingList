using ShoppingList.Domain.Entities;

namespace ShoppingList.WebMVC.Areas.AdminPanel.Models.ProductVM
{
    public class UpdateProductVM
    {
        public int id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string UrlImage { get; set; }
        public List<Category> CategoryList { get; set; }
        public Category Category { get; set; }
    }
}
