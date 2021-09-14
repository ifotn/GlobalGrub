using GlobalGrub.Models;
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
            // use the Category model to create 10 categories objects in memory
            var categories = new List<Category>();

            for (var i = 1; i < 11; i++)
            {
                categories.Add(new Category() { CategoryId = i, Name = "Category " + i.ToString() });
            }

            // pass the categories list as strongly-typed data to the view for display
            return View(categories);
        }

        // /categories/browse?category=abc - show selected category 
        public IActionResult Browse(string category)
        {
            // ensure we have a category value
            if (category == null)
            {
                // redirect the user to Index so they can choose a category
                return RedirectToAction("Index");
            }


            // store the input parameter in a var inside the ViewBag so we can display the user's selection
            ViewBag.category = category;

            return View();
        }
    }
}
