namespace ShoppingList.WebMVC.Areas.UserPanel.Models.ListsVM
{
    public class ListViewVM
    {
        public int Id { get; set; }
        public int ListId { get; set; }
        public string ListName { get; set; }
        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }

    }
}
