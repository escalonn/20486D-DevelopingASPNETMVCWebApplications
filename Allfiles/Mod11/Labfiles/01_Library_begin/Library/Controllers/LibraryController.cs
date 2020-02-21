using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Library.Data;
using Library.Models;
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

        public IActionResult Index()
        {
            var booksQuery = from b in _context.Books
                             where b.Recommended == true
                             orderby b.Author
                             select b;

            return View(booksQuery);
        }

        public IActionResult GetBooksByGenre()
        {
            if (User.Identity.IsAuthenticated)
            {
                var booksGenreQuery = from b in _context.Books
                                      orderby b.Genre.Name
                                      select b;

                return View(booksGenreQuery);
            }
            return RedirectToAction("Login", "Account");
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
