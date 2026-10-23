using Microsoft.AspNetCore.Mvc;
using myproject.Data;
using myproject.Models;

namespace myproject.Controllers
{
    public class StudentController : Controller
    {
        // Store the database context in a private variable
        private readonly ApplicationDbContext _context;

        // ASP.NET Core provides ApplicationDbContext
        // through Dependency Injection
        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Student/Index
        public IActionResult Index()
        {
            // Get all students from the database
            var students = _context.Students.ToList();

            // Send the students to Index.cshtml
            return View(students);
        }

        // GET: http://localhost:port/Student/Create
        public IActionResult Create()
        {
            // Display the empty Create form
            return View();
        }

        // POST: /Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            // Check whether the submitted data is valid
            if (ModelState.IsValid)
            {
                // Add the student to the Students table
                _context.Students.Add(student);

                // Save the changes to the database
                _context.SaveChanges();

                // Return to the student list
                return RedirectToAction(nameof(Index));
            }

            // If validation fails, display the form again
            return View(student);
        }
        // GET: /Student/Edit/1
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = _context.Students.Find(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
        // POST: /Student/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Students.Update(student);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(student);
        }
        // GET: /Student/Delete/1
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = _context.Students.Find(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
        // POST: /Student/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = _context.Students.Find(id);

            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}