namespace ShoppingList.WebMVC.Areas.UserPanel.Models.ListsVM
{
    public class ProductListsVM
    {
        public int id { get; set; }
        public int ProductId { get; set; }
        public int ListId { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }
        public string ListName { get; set; }
    }
}
