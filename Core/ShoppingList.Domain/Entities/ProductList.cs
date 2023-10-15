using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Entities
{
    public class ProductList
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ListId { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }

        public Product Product { get; set; }
        public List List { get; set; }
    }
}
