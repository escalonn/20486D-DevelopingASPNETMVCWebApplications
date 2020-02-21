using System.Collections.Generic;
using System.Linq;
using ElectricStore.Data;
using ElectricStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ElectricStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly StoreContext _context;

        public ShoppingCartController(StoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var customerProducts = HttpContext.Session.GetString("CustomerProducts");
            var customerFirstName = HttpContext.Session.GetString("CustomerFirstName");
            if (!string.IsNullOrEmpty(customerFirstName) && !string.IsNullOrEmpty(customerProducts))
            {
                var productsListId = JsonConvert.DeserializeObject<List<int>>(customerProducts);
                var products = productsListId.Select(i => _context.Products.FirstOrDefault(p => p.Id == i)).ToList();
                var sessionModel = new SessionStateViewModel
                {
                    CustomerName = customerFirstName,
                    SelectedProducts = products
                };
                return View(sessionModel);
            }
            return View();
        }
    }
}
