using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Application.ViewModels.ProductListViewModel
{
    public class AddProductListViewModel
    {
        public int ProductId { get; set; }
        public int ListId { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
    }
}
