using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RazorSyntaxExample.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.ProductPrices = new Dictionary<string, int>
            {
                { "Bread", 5 },
                { "Rice", 3 }
            };
            return View();
        }
    }
}