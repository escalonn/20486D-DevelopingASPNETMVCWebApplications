using System.IO;
using System.Linq;
using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library.Controllers
{
    [Authorize(Policy = "RequireEmail")]
    [Authorize(Roles = "Administrator")]
    public class LibrarianController : Controller
    {
        private readonly LibraryContext _context;

        public LibrarianController(LibraryContext libraryContext)
        {
            _context = libraryContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddBook()
        {
            PopulateGenreDropDownList();
            return View();
        }

        [HttpPost, ActionName("AddBook")]
        [ValidateAntiForgeryToken]
        public IActionResult AddBookPost(Book book)
        {
            if (ModelState.IsValid)
            {
                if (book.PhotoAvatar != null && book.PhotoAvatar.Length > 0)
                {
                    book.ImageMimeType = book.PhotoAvatar.ContentType;
                    book.ImageName = Path.GetFileName(book.PhotoAvatar.FileName);
                    using (var memoryStream = new MemoryStream())
                    {
                        book.PhotoAvatar.CopyTo(memoryStream);
                        book.PhotoFile = memoryStream.ToArray();
                    }
                    book.Available = true;
                    _context.Add(book);
                    _context.SaveChanges();
                }
                return RedirectToAction(nameof(ThankYou));
            }
            PopulateGenreDropDownList(book.Genre.Id);
            return View();
        }

        public IActionResult ThankYou()
        {
            return View();
        }

        private void PopulateGenreDropDownList(int? selectedGenre = null)
        {
            var genres = _context.Genres.OrderBy(b => b.Name);

            ViewBag.GenreList = new SelectList(items: genres.AsNoTracking(), dataValueField: "Id", dataTextField: "Name",
                selectedValue: selectedGenre);
        }
    }
}
