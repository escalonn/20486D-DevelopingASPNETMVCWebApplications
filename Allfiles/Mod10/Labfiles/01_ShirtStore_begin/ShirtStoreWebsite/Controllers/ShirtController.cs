using Microsoft.AspNetCore.Mvc;
using ShirtStoreWebsite.Models;
using ShirtStoreWebsite.Services;
using System.Collections.Generic;

namespace ShirtStoreWebsite.Controllers
{
    public class ShirtController : Controller
    {
        private readonly IShirtRepository _repository;

        public ShirtController(IShirtRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            IEnumerable<Shirt> shirts = _repository.GetShirts();
            return View(shirts);
        }

        public IActionResult AddShirt(Shirt shirt)
        {
            _repository.AddShirt(shirt);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _repository.RemoveShirt(id);
            return RedirectToAction("Index");
        }
    }
}
