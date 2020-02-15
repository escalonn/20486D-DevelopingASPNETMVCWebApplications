using ButterfliesShop.Models;
using ButterfliesShop.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ButterfliesShop.Controllers
{
    public class ButterflyController : Controller
    {
        private readonly IDataService _data;
        private readonly IHostingEnvironment _environment;
        private readonly IButterfliesQuantityService _butterfliesQuantityService;

        public ButterflyController(IDataService data, IHostingEnvironment environment, IButterfliesQuantityService butterfliesQuantityService)
        {
            _data = data;
            _environment = environment;
            _butterfliesQuantityService = butterfliesQuantityService;
            InitializeButterfliesData();
        }

        private void InitializeButterfliesData()
        {
            if (_data.ButterfliesList == null)
            {
                List<Butterfly> butterflies = _data.ButterfliesInitializeData();
                foreach (var butterfly in butterflies)
                {
                    _butterfliesQuantityService.AddButterfliesQuantityData(butterfly);
                }
            }
        }

        public IActionResult Index()
        {
            var indexViewModel = new IndexViewModel { Butterflies = _data.ButterfliesList };
            return View(indexViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Butterfly butterfly)
        {
            var lastButterfly = _data.ButterfliesList.LastOrDefault();

            butterfly.CreatedDate = DateTime.Today;

            if (butterfly.PhotoAvatar != null && butterfly.PhotoAvatar.Length > 0)
            {
                butterfly.ImageMimeType = butterfly.PhotoAvatar.ContentType;
                butterfly.ImageName = Path.GetFileName(butterfly.PhotoAvatar.FileName);

                butterfly.Id = lastButterfly.Id + 1;
                _butterfliesQuantityService.AddButterfliesQuantityData(butterfly);

                using (var memoryStream = new MemoryStream())
                {
                    butterfly.PhotoAvatar.CopyTo(memoryStream);
                    butterfly.PhotoFile = memoryStream.ToArray();
                }

                _data.AddButterfly(butterfly);

                return RedirectToAction("Index");
            }

            return View(butterfly);
        }

        public IActionResult GetImage(int id)
        {
            Butterfly requestedButterfly = _data.GetButterflyById(id);
            if (requestedButterfly != null)
            {
                var webRootPath = _environment.WebRootPath;
                var folderPath = @"\images\";
                var fullPath = webRootPath + folderPath + requestedButterfly.ImageName;
                if (System.IO.File.Exists(fullPath))
                {
                    var fileOnDisk = new FileStream(fullPath, FileMode.Open);
                    byte[] fileBytes;
                    using (var br = new BinaryReader(fileOnDisk))
                    {
                        fileBytes = br.ReadBytes((int)fileOnDisk.Length);
                    }
                    return File(fileBytes, requestedButterfly.ImageMimeType);
                }
                else
                {
                    if (requestedButterfly.PhotoFile.Length > 0)
                    {
                        return File(requestedButterfly.PhotoFile, requestedButterfly.ImageMimeType);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}