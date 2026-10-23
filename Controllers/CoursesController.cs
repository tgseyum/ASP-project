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

        // Display the course form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Save a new course
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course course)
        {
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
            var courses = _context.Courses
                .AsNoTracking()
                .ToList();

            return View(courses);
        }
    }
}