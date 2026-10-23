using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myproject.Data;
using myproject.Models;

namespace myproject.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }
        // NEW CODE: Add inside CourseController.

        private bool IsDemoUserLoggedIn()
        {
            return !string.IsNullOrEmpty(
                HttpContext.Session.GetString("DemoUser"));
        }

        // Display the course form
        [HttpGet]
        public IActionResult Create()
        {
            // NEW CODE: Check login before showing courses.
            if (!IsDemoUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // Save a new course
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course course)
        {
            // NEW CODE: Check login before showing courses.
            if (!IsDemoUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                _context.Courses.Add(course);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(course);
        }

        // Display all courses
        public IActionResult Index()
        {
            // NEW CODE: Check login before showing courses.
            if (!IsDemoUserLoggedIn())
            {
                return RedirectToAction("Login", "Account");
            }
            var courses = _context.Courses
                .AsNoTracking()
                .ToList();

            return View(courses);
        }
    }
}