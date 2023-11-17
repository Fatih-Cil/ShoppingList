using ShoppingList.WebMVC.Areas.AdminPanel.Models.ProductVM;

namespace ShoppingList.WebMVC.Areas.UserPanel.Models.ListsVM
{
    public class MyListViewModel
    {
        public List<ProductListsVM> ProductListsVM { get; set; }
        public List<ProductDetailDTOVM> ProductDetailDTOVM { get; set;}
    }
}
