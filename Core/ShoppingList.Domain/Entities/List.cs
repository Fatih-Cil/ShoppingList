using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Domain.Entities
{
    public class List
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }

        public User User { get; set; }
        public ICollection<ProductList> ProductLists { get; set; }
    }
}
