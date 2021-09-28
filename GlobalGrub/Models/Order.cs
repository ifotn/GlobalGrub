using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalGrub.Models
{
    public class Order
    {
        [Display(Name = "Order ID")] // Changes the name of the field.
        public int OrderId { get; set; }

        [Range(0, 99999999)]
        [Required(ErrorMessage = "Please make sure the order total was entered.")]
        public double Total { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please make sure the order date was entered.")]
        public DateTime OrderDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please make sure a first name was entered.")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please make sure a last name was entered.")]
        public string LastName { get; set; }        
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please the address was entered.")]
        public string Address { get; set; }

        //Foreign key
        [Required]
        public string UserId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please make sure the city was entered.")]
        public string City { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please make sure the province was entered.")]
        public string Province { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please make sure the postal code was entered.")]
        public string PostalCode { get; set; }

        [Required]
        public string Phone { get; set; }

        public string PaymentCode { get; set; }
        
        // child ref
        public List<OrderItem> OrderItems { get; set; }

    }
}
