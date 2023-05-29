using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseWork.Models;

namespace CourseWork.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly LibraryDbContext _context;

        public ApplicationsController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: Applications
        public async Task<IActionResult> Index()
        {
            var libraryDbContext = _context.Applications.Include(a => a.Book).Include(a => a.Student);
            return View(await libraryDbContext.ToListAsync());
        }

        // GET: Applications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Applications == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .Include(a => a.Book)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.ApplicationId == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // GET: Applications/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookId");
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId");
            return View();
        }

        // POST: Applications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApplicationId,StudentId,BookId,IssueDate,ReturnDate")] Application application)
        {
            if (ModelState.IsValid)
            {
                _context.Add(application);
                if (application.ReturnDate == null)
                {
                    Student studentToUpdate = _context.Students.Single(s => s.StudentId == application.StudentId);
                    if (studentToUpdate.BooksCount >= 10)
                    {
                        var error = new ErrorViewModel();
                        return View(error);
                        //return Problem("Студент не може взяти більше 10 книг");
                    }
                    studentToUpdate.BooksCount++;
                    _context.Update(studentToUpdate);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookId", application.BookId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", application.StudentId);
            return View(application);
        }

        // GET: Applications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Applications == null)
            {
                return NotFound();
            }

            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookId", application.BookId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", application.StudentId);
            return View(application);
        }

        // POST: Applications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ApplicationId,StudentId,BookId,IssueDate,ReturnDate")] Application application)
        {
            if (id != application.ApplicationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(application);
                    var studentToUpdate = _context.Students.Single(s => s.StudentId == application.StudentId);
                    if (application.ReturnDate == null || studentToUpdate.BooksCount == 0)
                    {
                        var error = new ErrorViewModel();
                        return View(error);
                        //"Студент не має книг";                        
                    }
                    studentToUpdate.BooksCount--;
                    _context.Update(studentToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationExists(application.ApplicationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "BookId", "BookId", application.BookId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", application.StudentId);
            return View(application);
        }

        // GET: Applications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Applications == null)
            {
                return NotFound();
            }

            var application = await _context.Applications
                .Include(a => a.Book)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.ApplicationId == id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Applications == null)
            {
                return Problem("Entity set 'LibraryDbContext.Applications'  is null.");
            }
            var application = await _context.Applications.FindAsync(id);
            if (application != null)
            {
                var studentToUpdate = _context.Students.Single(s => s.StudentId == application.StudentId);
                if (application != null)
                {
                    var studentToUpdate = _context.Students.Single(s => s.StudentId == application.StudentId);
                    if (studentToUpdate.BooksCount != 0)
                    {
                        studentToUpdate.BooksCount--;
                        _context.Update(studentToUpdate);
                    }
                    _context.Applications.Remove(application);
                }
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationExists(int id)
        {
          return (_context.Applications?.Any(e => e.ApplicationId == id)).GetValueOrDefault();
        }
    }
}
