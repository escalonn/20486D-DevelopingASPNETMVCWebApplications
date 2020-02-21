using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ElectricStore.Data;
using ElectricStore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace ElectricStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly StoreContext _context;
        private readonly IMemoryCache _memoryCache;

        private const string PRODUCT_KEY = "Products";

        public ProductsController(StoreContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            if (!_memoryCache.TryGetValue(PRODUCT_KEY, out List<Product> products))
            {
                products = _context.Products.ToList();
                products.ForEach(c => c.LoadedFromDatabase = DateTime.Now);
                var cacheOptions = new MemoryCacheEntryOptions { Priority = CacheItemPriority.High };
                _memoryCache.Set(PRODUCT_KEY, products, cacheOptions);
            }
            return View(products);
        }

        public IActionResult GetByCategory(int id)
        {
            var products = _context.Products.Where(c => c.CategoryId == id);
            var category = _context.MenuCategories.FirstOrDefault(c => c.Id == id);
            ViewBag.CategoryTitle = category.Name;
            return View(products);
        }

        [HttpGet]
        public IActionResult AddToShoppingList()
        {
            PopulateProductsList();
            if (HttpContext.Session.TryGetValue("CustomerProducts", out _))
            {
                var sessionCustomer = new Customer
                {
                    FirstName = HttpContext.Session.GetString("CustomerFirstName"),
                    LastName = HttpContext.Session.GetString("CustomerLastName"),
                    Email = HttpContext.Session.GetString("CustomerEmail"),
                    Address = HttpContext.Session.GetString("CustomerAddress"),
                    PhoneNumber = HttpContext.Session.GetInt32("CustomerPhoneNumber").Value
                };
                return View(sessionCustomer);
            }
            return View();
        }

        [HttpPost, ActionName("AddToShoppingList")]
        public IActionResult AddToShoppingListPost(Customer customer)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("CustomerFirstName", customer.FirstName);
                HttpContext.Session.SetString("CustomerLastName", customer.LastName);
                HttpContext.Session.SetString("CustomerEmail", customer.Email);
                HttpContext.Session.SetString("CustomerAddress", customer.Address);
                HttpContext.Session.SetInt32("CustomerPhoneNumber", customer.PhoneNumber);
                if (HttpContext.Session.GetString("CustomerProducts") is string customerProducts)
                {
                    var productsListId = JsonConvert.DeserializeObject<List<int>>(customerProducts);
                    customer.SelectedProductsList.AddRange(productsListId);
                }
                var serializedProducts = JsonConvert.SerializeObject(customer.SelectedProductsList);
                HttpContext.Session.SetString("CustomerProducts", serializedProducts);

                return RedirectToAction(nameof(Index));
            }
            PopulateProductsList(customer.SelectedProductsList);
            return View(customer);
        }

        private void PopulateProductsList(IEnumerable<int> selectedProducts = null)
        {
            var products = _context.Products.OrderBy(p => p.ProductName);

            ViewBag.ProductsList = new MultiSelectList(
                items: products.AsNoTracking(),
                dataValueField: "Id",
                dataTextField: "ProductName",
                selectedValues: selectedProducts);
        }

        public IActionResult GetImage(int productId, [FromServices] IHostingEnvironment environment)
        {
            if (_context.Products.FirstOrDefault(i => i.Id == productId) is Product requestedPhoto)
            {
                string webRootPath = environment.WebRootPath;
                string folderPath = "\\images\\";
                string fullPath = webRootPath + folderPath + requestedPhoto.PhotoFileName;

                byte[] fileBytes;
                using (var fileOnDisk = new FileStream(fullPath, FileMode.Open))
                using (var br = new BinaryReader(fileOnDisk))
                {
                    fileBytes = br.ReadBytes((int)fileOnDisk.Length);
                }
                return File(fileBytes, requestedPhoto.ImageMimeType);
            }
            return NotFound();
        }
    }
}