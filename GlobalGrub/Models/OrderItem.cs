using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalGrub.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        [Range(1, 999999)]
        [Required]
        public int Quantity { get; set; }

        [Range(0.01, 999999)]
        [Required]
        public double Price { get; set; }

        // fk's
        public int ProductId { get; set; }
        public int OrderId { get; set; }

        // parent refs
        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}
