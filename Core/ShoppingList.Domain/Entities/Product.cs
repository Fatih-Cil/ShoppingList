using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string UrlImage { get; set; }

        public Category Category { get; set; }

        public ICollection<ProductList> ProductLists { get; set; }
    }
}
