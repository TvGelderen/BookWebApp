using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookWebApp.Data;
using BookWebApp.Models;

namespace BookWebApp.Controllers
{
    public class BookController : Controller
    {
        private readonly DatabaseContext _context;

        public BookController(DatabaseContext db)
        {
            _context = db;
        }

        public async Task<IActionResult> Index(string? currentSearchString, string? searchString, string? sortOrder)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParam"] = String.IsNullOrEmpty(sortOrder) ? "titleDesc" : "";
            ViewData["AuthorSortParam"] = sortOrder == "Author" ? "authorDesc" : "Author";
            ViewData["YearSortParam"] = sortOrder == "Year" ? "yearDesc" : "Year";

            if (searchString == null)
                searchString = currentSearchString;

            ViewData["CurrentSearchString"] = searchString;

            var bookList = from book in _context.Book_db
                           select book;

            if (!String.IsNullOrEmpty(searchString))
                bookList = _context.Book_db.Where(x => x.Title.Contains(searchString));

            if (bookList == null)
                return NotFound();            

            switch (sortOrder)
            {
                case "titleDesc":
                    bookList = bookList.OrderByDescending(x => x.Title);
                    break;
                case "authorDesc":
                    bookList = bookList.OrderByDescending(x => x.Author);
                    break;
                case "Author":
                    bookList = bookList.OrderBy(x => x.Author);
                    break;
                case "yearDesc":
                    bookList = bookList.OrderByDescending(x => x.Year);
                    break;
                case "Year":
                    bookList = bookList.OrderBy(x => x.Year);
                    break;
                default:
                    bookList = bookList.OrderBy(x => x.Title);
                    break;
            }

            return View(await bookList.ToListAsync());
        }

        // GET
        public IActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Book_db.AddAsync(book);
                _context.SaveChanges();

                TempData["success"] = "Book added successfully!";

                return RedirectToAction("Index");
            }

            return View(book);
        }

        // GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var book = _context.Book_db.FirstOrDefault(x => x.Id == id);

            if (book == null)
                return NotFound();

            return View(book);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Book_db.Update(book);
                _context.SaveChanges();

                TempData["success"] = "Book edited successfully!";

                return RedirectToAction("Index");
            }

            return View(book);
        }

        // GET
        public IActionResult Details(int? id)
        {
            var book = _context.Book_db.FirstOrDefault(x => x.Id == id);

            if (book == null)
                return NotFound();

            return View(book);
        }

        // GET
        public IActionResult Delete(int? id)
        {
            var book = _context.Book_db.FirstOrDefault(x => x.Id == id);

            if (book == null)
                return NotFound();

            _context.Book_db.Remove(book);
            _context.SaveChanges();

            TempData["success"] = "Book deleted successfully!";

            return RedirectToAction("Index");
        }
    }
}
