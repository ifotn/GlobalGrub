using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalGrub.Models
{
    public class Category
    {
        // all pk fields in ASP.NET MVC should be called either {Model}Id or Id
        // property names should always use PascalCase
        [Display(Name = "Category Id")] // this sets an alias for all labels globally
        public int CategoryId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "And no darn empty strings!")]
        [MaxLength(100)]
        public string Name { get; set; }

        // child ref
        public List<Product> Products { get; set; }
    }
}
