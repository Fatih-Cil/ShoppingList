namespace ShoppingList.WebMVC.Areas.AdminPanel.Models.ProductVM
{
    public class ProductDetailDTOVM
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string UrlImage { get; set; }
    }
}
