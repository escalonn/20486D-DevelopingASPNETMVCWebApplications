using Cupcakes.Models;
using Cupcakes.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Cupcakes.Controllers
{
    public class CupcakeController : Controller
    {
        private readonly ICupcakeRepository _repository;

        public CupcakeController(ICupcakeRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View(_repository.GetCupcakes());
        }

        public IActionResult Details(int id)
        {
            if (_repository.GetCupcakeById(id) is Cupcake cupcake)
            {
                PopulateBakeriesDropDownList(cupcake.BakeryId);
                return View(cupcake);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult Create()
        {
            PopulateBakeriesDropDownList();
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        public IActionResult CreatePost(Cupcake cupcake)
        {
            if (ModelState.IsValid)
            {
                _repository.CreateCupcake(cupcake);
                return RedirectToAction(nameof(Index));
            }
            PopulateBakeriesDropDownList(cupcake.BakeryId);
            return View(cupcake);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (_repository.GetCupcakeById(id) is Cupcake cupcake)
            {
                PopulateBakeriesDropDownList(cupcake.BakeryId);
                return View(cupcake);
            }
            return NotFound();
        }

        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> EditPost(int id, Cupcake cupcake)
        {
            if (_repository.GetCupcakeById(id) is Cupcake cupcakeToUpdate)
            {
                if (await TryUpdateModelAsync(cupcakeToUpdate, prefix: "",
                    c => c.BakeryId, c => c.CupcakeType, c => c.Description, c => c.GlutenFree, c => c.Price))
                {
                    _repository.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                PopulateBakeriesDropDownList(cupcakeToUpdate.BakeryId);
                return View(cupcakeToUpdate);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (_repository.GetCupcakeById(id) is Cupcake cupcake)
            {
                return View(cupcake);
            }
            return NotFound();
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _repository.DeleteCupcake(id);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult GetImage(int id, [FromServices] IHostingEnvironment environment)
        {
            if (_repository.GetCupcakeById(id) is Cupcake requestedCupcake)
            {
                string webRootpath = environment.WebRootPath;
                string folderPath = @"\images\";
                string fullPath = webRootpath + folderPath + requestedCupcake.ImageName;
                if (System.IO.File.Exists(fullPath))
                {
                    var fileOnDisk = new FileStream(fullPath, FileMode.Open);
                    byte[] fileBytes;
                    using (var br = new BinaryReader(fileOnDisk))
                    {
                        fileBytes = br.ReadBytes((int)fileOnDisk.Length);
                    }
                    return File(fileBytes, requestedCupcake.ImageMimeType);
                }
                if (requestedCupcake.PhotoFile.Length > 0)
                {
                    return File(requestedCupcake.PhotoFile, requestedCupcake.ImageMimeType);
                }
                return NotFound();
            }
            return NotFound();
        }

        private void PopulateBakeriesDropDownList(int? selectedBakery = null)
        {
            var bakeries = _repository.PopulateBakeriesDropDownList();
            ViewBag.BakeryId = new SelectList(bakeries.AsNoTracking(), "BakeryId", "BakeryName", selectedBakery);
        }
    }
}
