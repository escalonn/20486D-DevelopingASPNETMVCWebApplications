using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using ZooSite.Data;
using ZooSite.Models;

namespace ZooSite.Controllers
{
    public class ZooController : Controller
    {
        private readonly ZooContext _context;

        public ZooController(ZooContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Photos.ToList());
        }

        public IActionResult VisitorDetails()
        {
            return View();
        }

        [HttpGet]
        public IActionResult BuyTickets()
        {
            return View();
        }

        [HttpPost, ActionName("BuyTickets")]
        public IActionResult BuyTicketsPost(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return RedirectToAction("ThankYou");
            }
            return View(customer);
        }

        public IActionResult ThankYou()
        {
            return View();
        }

        public IActionResult GetImage(int photoId, [FromServices] IHostingEnvironment environment)
        {
            if (_context.Photos.FirstOrDefault(p => p.PhotoID == photoId) is Photo requestedPhoto)
            {
                string webRootPath = environment.WebRootPath;
                string folderPath = @"\images\";
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
