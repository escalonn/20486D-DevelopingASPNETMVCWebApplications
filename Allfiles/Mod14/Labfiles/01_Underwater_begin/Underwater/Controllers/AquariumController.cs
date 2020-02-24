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

        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetFishesAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            if (await _repository.GetFishByIdAsync(id) is Fish fish)
            {
                return View(fish);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateAquariumsDropDownListAsync();
            return View();
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> CreatePostAsync(Fish fish)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddFishAsync(fish);
                return RedirectToAction(nameof(Index));
            }
            await PopulateAquariumsDropDownListAsync(fish.AquariumId);
            return View(fish);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (await _repository.GetFishByIdAsync(id) is Fish fish)
            {
                await PopulateAquariumsDropDownListAsync(fish.AquariumId);
                return View(fish);
            }
            return NotFound();
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(int id)
        {
            Fish fishToUpdate = await _repository.GetFishByIdAsync(id);
            bool isUpdated = await TryUpdateModelAsync(fishToUpdate,
                "",
                f => f.AquariumId,
                f => f.Name,
                f => f.ScientificName,
                f => f.CommonName);
            if (isUpdated)
            {
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await PopulateAquariumsDropDownListAsync(fishToUpdate.AquariumId);
            return View(fishToUpdate);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _repository.GetFishByIdAsync(id) is Fish fish)
            {
                return View(fish);
            }
            return NotFound();
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _repository.RemoveFishAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateAquariumsDropDownListAsync(int? selectedAquarium = null)
        {
            var aquariums = _repository.GetAquariums();
            var items = await aquariums.AsNoTracking().ToListAsync();
            ViewBag.AquariumId = new SelectList(items: items, dataValueField: "AquariumId", dataTextField: "Name",
                selectedValue: selectedAquarium);
        }

        public async Task<IActionResult> GetImage(int id, [FromServices] IHostingEnvironment environment)
        {
            if (await _repository.GetFishByIdAsync(id) is Fish requestedFish)
            {
                string webRootPath = environment.WebRootPath;
                string folderPath = @"\images\";
                string fullPath = webRootPath + folderPath + requestedFish.ImageName;
                if (System.IO.File.Exists(fullPath))
                {
                    byte[] fileBytes;
                    using (var fileOnDisk = new FileStream(fullPath, FileMode.Open))
                    using (var memoryStream = new MemoryStream())
                    {
                        await fileOnDisk.CopyToAsync(memoryStream);
                        fileBytes = memoryStream.ToArray();
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
