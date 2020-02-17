using IceCreamCompany.Models;
using IceCreamCompany.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace IceCreamCompany.Controllers
{
    public class IceCreamController : Controller
    {
        private readonly IRepository _repository;

        public IceCreamController(IRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View(_repository.GetIceCreamFlavors());
        }

        [HttpGet]
        public IActionResult Buy()
        {
            return View();
        }

        [HttpPost, ActionName("Buy")]
        public IActionResult BuyPost(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _repository.BuyIceCreamFlavor(customer);
                return RedirectToAction(nameof(ThankYou));
            }
            return View(customer);
        }

        public IActionResult ThankYou()
        {
            return View();
        }

        public IActionResult GetImage(int iceCreamId, [FromServices] IHostingEnvironment environment)
        {
            if (_repository.GetIceCreamFlavorById(iceCreamId) is IceCream requestedPhoto)
            {
                string webRootPath = environment.WebRootPath;
                string folderPath = "/images/";
                string fullPath = webRootPath + folderPath + requestedPhoto.PhotoFileName;

                var fileOnDisk = new FileStream(fullPath, FileMode.Open);
                byte[] fileBytes;
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