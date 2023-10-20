using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Application.ViewModels.ListViewModel
{
    public class AddListViewModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
