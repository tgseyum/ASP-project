using Microsoft.AspNetCore.Mvc;
using myproject.Data;
using myproject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

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


        // Check whether the demo user is logged in.
        private bool IsDemoUserLoggedIn()
        {
            return !string.IsNullOrEmpty(
                HttpContext.Session.GetString("DemoUser"));
        }
        // // GET: /Student/Index
        // public IActionResult Index()
        // {
        //     // Get all students from the database
        //     var students = _context.Students.ToList();

        //     // Send the students to Index.cshtml
        //     return View(students);
        // }

        // Display the student list.
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Step 1: Check the session for a demo username.
            var demoUser = HttpContext.Session.GetString("DemoUser");

            // Step 2: Redirect to Login if no session exists.
            if (string.IsNullOrEmpty(demoUser))
            {
                return RedirectToAction("Login", "Account");
            }

            // Step 3: Retrieve students from the SQLite database.
            var students = await _context.Students.ToListAsync();

            // Step 4: Pass the student records to the Index view.
            return View(students);
        }



        // GET: http://localhost:port/Student/Create
        public IActionResult Create()
        {
            // Require login before displaying the Create form.
            if (!IsDemoUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }
            // Display the empty Create form
            return View();
        }

        // POST: /Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {

            // NEW CODE: Check the session before processing submitted data.
            if (!IsDemoUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }
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
            // NEW CODE: Check the session before processing submitted data.
            if (!IsDemoUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }
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
            // NEW CODE: Check the session before processing submitted data.
            if (!IsDemoUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }
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
            // NEW CODE: Check the session before processing submitted data.
            if (!IsDemoUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }
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
            // NEW CODE: Check the session before processing submitted data.
            if (!IsDemoUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }
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