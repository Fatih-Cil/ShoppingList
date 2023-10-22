using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Application.DTOs
{
    public class ProductListDetailDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }
        public int ListId { get; set; }
        public string ListName { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
    }
}
