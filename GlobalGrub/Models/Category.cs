using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalGrub.Models
{
    public class Category
    {
        // all pk fields in ASP.NET MVC should be called either {Model}Id or Id
        // property names should always use PascalCase
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }
}
