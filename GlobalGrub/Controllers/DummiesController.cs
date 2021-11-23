using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalGrub.Controllers
{
    public class DummiesController : Controller
    {
        public IActionResult Index()
        {
            // put a var in ViewData (similar to ViewBag)
            ViewData["Message"] = "This is a viewdata message";
            return View("Index");
        }
    }
}
