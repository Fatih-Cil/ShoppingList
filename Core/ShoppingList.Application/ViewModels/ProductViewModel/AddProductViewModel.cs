using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Application.ViewModels.ProductViewModel
{
    public class AddProductViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string UrlImage { get; set; }
    }
}
