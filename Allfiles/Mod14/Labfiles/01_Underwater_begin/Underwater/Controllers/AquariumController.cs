using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Underwater.Models;
using Underwater.Repositories;

namespace Underwater.Controllers
{
    public class AquariumController : Controller
    {
        private readonly IUnderwaterRepository _repository;

        public AquariumController(IUnderwaterRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View(_repository.GetFishes());
        }

        public IActionResult Details(int id)
        {
            if (_repository.GetFishById(id) is Fish fish)
            {
                return View(fish);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult Create()
        {
            PopulateAquariumsDropDownList();
            return View();
        }

        [HttpPost, ActionName("Create")]
        public IActionResult CreatePost(Fish fish)
        {
            if (ModelState.IsValid)
            {
                _repository.AddFish(fish);
                return RedirectToAction(nameof(Index));
            }
            PopulateAquariumsDropDownList(fish.AquariumId);
            return View(fish);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (_repository.GetFishById(id) is Fish fish)
            {
                PopulateAquariumsDropDownList(fish.AquariumId);
                return View(fish);
            }
            return NotFound();
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(int id)
        {
            Fish fishToUpdate = _repository.GetFishById(id);
            bool isUpdated = await TryUpdateModelAsync(fishToUpdate,
                "",
                f => f.AquariumId,
                f => f.Name,
                f => f.ScientificName);
            if (isUpdated)
            {
                _repository.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            PopulateAquariumsDropDownList(fishToUpdate.AquariumId);
            return View(fishToUpdate);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (_repository.GetFishById(id) is Fish fish)
            {
                return View(fish);
            }
            return NotFound();
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _repository.RemoveFish(id);
            return RedirectToAction(nameof(Index));
        }

        private void PopulateAquariumsDropDownList(int? selectedAquarium = null)
        {
            var aquariums = _repository.GetAquariums();
            ViewBag.AquariumId = new SelectList(items: aquariums.AsNoTracking(), dataValueField: "AquariumId",
                dataTextField: "Name", selectedValue: selectedAquarium);
        }

        public IActionResult GetImage(int id, [FromServices] IHostingEnvironment environment)
        {
            if (_repository.GetFishById(id) is Fish requestedFish)
            {
                string webRootPath = environment.WebRootPath;
                string folderPath = @"\images\";
                string fullPath = webRootPath + folderPath + requestedFish.ImageName;
                if (System.IO.File.Exists(fullPath))
                {
                    var fileOnDisk = new FileStream(fullPath, FileMode.Open);
                    byte[] fileBytes;
                    using (var br = new BinaryReader(fileOnDisk))
                    {
                        fileBytes = br.ReadBytes((int)fileOnDisk.Length);
                    }
                    return File(fileBytes, contentType: requestedFish.ImageMimeType);
                }
                if (requestedFish.PhotoFile.Length > 0)
                {
                    return File(requestedFish.PhotoFile, contentType: requestedFish.ImageMimeType);
                }
            }
            return NotFound();
        }
    }
}
