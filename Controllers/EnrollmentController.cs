using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using myproject.Models;
using myproject.Data;

namespace myproject.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentController(ApplicationDbContext context)
        {
            _context = context;
        }
// NEW CODE: Add inside EnrollmentController.

private bool IsDemoUserLoggedIn()
{
    return !string.IsNullOrEmpty(
        HttpContext.Session.GetString("DemoUser"));
}
        // Display all enrollment records
        public async Task<IActionResult> Index()
        {

              // NEW CODE: Require a valid demo session.
    if (!IsDemoUserLoggedIn())
    {
        return RedirectToAction("Login", "Account");
    }
            var enrollments = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ToListAsync();

            return View(enrollments);
        } 

        // Display the enrollment form
        [HttpGet]
        public async Task<IActionResult> Create()
        {
              // NEW CODE: Require a valid demo session.
    if (!IsDemoUserLoggedIn())
    {
        return RedirectToAction("Login", "Account");
    }
            await LoadDropdowns();
            return View(new Enrollment());
        }

        // Process the enrollment form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Enrollment enrollment)
        {
              // NEW CODE: Require a valid demo session.
    if (!IsDemoUserLoggedIn())
    {
        return RedirectToAction("Login", "Account");
    }
            // Verify that the selected student exists
            bool studentExists = await _context.Students
                .AnyAsync(s => s.Id == enrollment.StudentId);

            // Verify that the selected course exists
            bool courseExists = await _context.Courses
                .AnyAsync(c => c.Id == enrollment.CourseId);

            if (!studentExists)
            {
                ModelState.AddModelError(
                    "StudentId",
                    "Please select a valid student.");
            }

            if (!courseExists)
            {
                ModelState.AddModelError(
                    "CourseId",
                    "Please select a valid course.");
            }

            // Prevent duplicate enrollment
            if (studentExists && courseExists)
            {
                bool alreadyEnrolled = await _context.Enrollments
                    .AnyAsync(e =>
                        e.StudentId == enrollment.StudentId &&
                        e.CourseId == enrollment.CourseId);

                if (alreadyEnrolled)
                {
                    ModelState.AddModelError(
                        "",
                        "This student is already enrolled in this course.");
                }
            }

            // Return to the form if validation fails
            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(enrollment);
            }

            // Save the enrollment
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Student enrolled successfully.";

            return RedirectToAction(nameof(Index));
        }

        // Load students and courses into dropdown lists
        private async Task LoadDropdowns()
        {
            ViewBag.Students = new SelectList(
                await _context.Students.ToListAsync(),
                "Id",
                "Name");

            ViewBag.Courses = new SelectList(
                await _context.Courses.ToListAsync(),
                "Id",
                "CourseName");
        }
    }
}