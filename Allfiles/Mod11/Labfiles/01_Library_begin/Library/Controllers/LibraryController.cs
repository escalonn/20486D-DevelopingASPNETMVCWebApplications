﻿using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;


namespace Library.Controllers
{
    public class LibraryController : Controller
    {

        private readonly LibraryContext _context;

        public LibraryController(LibraryContext libraryContext)
        {
            _context = libraryContext;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var booksQuery = _context.Books.Where(b => b.Recommended).OrderBy(b => b.Author);

            return View(booksQuery);
        }

        [Authorize]
        public IActionResult GetBooksByGenre()
        {
            var booksGenreQuery = _context.Books.OrderBy(b => b.Genre.Name);

            return View(booksGenreQuery);
        }

        public IActionResult LendingBook(int id)
        {
            if (_context.Books.FirstOrDefault(b => b.Id == id) is Book book)
            {
                return View(book);
            }
            return NotFound();
        }

        [HttpPost, ActionName("LendingBook")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LendingBookPost(int id)
        {
            var bookToUpdate = _context.Books.FirstOrDefault(b => b.Id == id);
            bookToUpdate.Available = false;
            if (await TryUpdateModelAsync(
                model: bookToUpdate,
                prefix: "",
                includeExpressions: b => b.Available))
            {
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(bookToUpdate);
        }

        public IActionResult GetImage(int id, [FromServices] IHostingEnvironment environment)
        {
            if (_context.Books.FirstOrDefault(b => b.Id == id) is Book requestedBook)
            {
                string webRootPath = environment.WebRootPath;
                string folderPath = "\\images\\";
                string fullPath = webRootPath + folderPath + requestedBook.ImageName;
                if (System.IO.File.Exists(fullPath))
                {
                    var fileOnDisk = new FileStream(fullPath, FileMode.Open);
                    byte[] fileBytes;
                    using (var br = new BinaryReader(fileOnDisk))
                    {
                        fileBytes = br.ReadBytes((int)fileOnDisk.Length);
                    }
                    return File(fileBytes, requestedBook.ImageMimeType);
                }
                if (requestedBook.PhotoFile.Length > 0)
                {
                    return File(requestedBook.PhotoFile, requestedBook.ImageMimeType);
                }
            }
            return NotFound();
        }
    }
}
