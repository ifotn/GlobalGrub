using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalGrub.Models
{
    public class Product
    {
        [Required()]
        [Range(0, 9999)]
        [Display(Name = "Product ID")]
        public int ProductId { get; set; }

        [Required()]
        [MinLength(2)]
        public string Name { get; set; }

        [Required()]
        [Range(minimum: 0.01, maximum: 999)]
        public decimal Price { get; set; }

        [Required()]
        [Range(minimum: 0, maximum: 9999)]
        public decimal Weight { get; set; }

        [Required()]
        [Range(0, 9999)]
        public int CategoryId { get; set; }

        public string Photo { get; set; }

        public Category Category { get; set; } // ref to parent property

        // child ref's
        public List<CartItem> CartItems { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
