using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalGrub.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // /categories/browse?category=abc - show selected category 
        public IActionResult Browse(string category)
        {
            // store the input parameter in a var inside the ViewBag so we can display the user's selection
            ViewBag.category = category;

            return View();
        }
    }
}
