// Import Entity Framework Core to configure the database connection.
using Microsoft.EntityFrameworkCore;

// Import ASP.NET Core Identity classes for password hashing.
using Microsoft.AspNetCore.Identity;

// Import the ApplicationDbContext class from the Data folder.
using myproject.Data;

// Import the LoginAccount model from the Models folder.
using myproject.Models;


// Create the application builder.
// The builder is used to register services and configure the application.
var builder = WebApplication.CreateBuilder(args);


// --------------------------------------------------
// 1. Register MVC Services
// --------------------------------------------------

// Enable MVC functionality, including Controllers and Views.
builder.Services.AddControllersWithViews();


// --------------------------------------------------
// 2. Configure Cookie Authentication
// --------------------------------------------------

// Register cookie-based authentication.
// "ApplicationCookie" is the name of the authentication scheme.
builder.Services.AddAuthentication("ApplicationCookie")
    .AddCookie("ApplicationCookie", options =>
    {
        // Redirect unauthenticated users to the Login page
        // when they try to access a protected page.
        options.LoginPath = "/Account/Login";

        // Prevent client-side JavaScript from accessing the authentication cookie.
        options.Cookie.HttpOnly = true;

        // Allow the authentication cookie to be stored
        // even when the user has not explicitly accepted cookie consent.
        options.Cookie.IsEssential = true;
    });


// --------------------------------------------------
// 3. Register Password Hashing Service
// --------------------------------------------------

// Register the password hashing service using dependency injection.
// PasswordHasher hashes passwords during registration
// and verifies passwords during login.
//
// IPasswordHasher<LoginAccount> is the interface.
// PasswordHasher<LoginAccount> is the implementation.
builder.Services.AddScoped<IPasswordHasher<LoginAccount>,
    PasswordHasher<LoginAccount>>();


// --------------------------------------------------
// 4. Configure Database Connection
// --------------------------------------------------

// Register ApplicationDbContext with dependency injection.
// Configure Entity Framework Core to use SQLite.
//
// "student.db" is the SQLite database file.
// Entity Framework Core uses this context to interact with the database.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=student.db"));


// --------------------------------------------------
// 5. Build the Application
// --------------------------------------------------

// Build the application using the registered services and configurations.
var app = builder.Build();


// --------------------------------------------------
// 6. Configure Error Handling and Security
// --------------------------------------------------

// Check whether the application is running outside Development mode.
if (!app.Environment.IsDevelopment())
{
    // Redirect users to the custom Error page when an exception occurs.
    app.UseExceptionHandler("/Home/Error");

    // Add HTTP Strict Transport Security (HSTS).
    // This tells browsers to use HTTPS for future requests.
    app.UseHsts();
}


// --------------------------------------------------
// 7. Configure Middleware
// --------------------------------------------------

// Redirect HTTP requests to HTTPS.
app.UseHttpsRedirection();

// Enable serving static files such as CSS, JavaScript and images.
app.UseStaticFiles();

// Enable routing to match incoming requests to controller actions.
app.UseRouting();

// Identify the currently logged-in user using the authentication cookie.
// This must run before authorization.
app.UseAuthentication();

// Check whether the authenticated user has permission
// to access the requested resource.
app.UseAuthorization();


// --------------------------------------------------
// 8. Configure Default MVC Route
// --------------------------------------------------

// Define the default route for the application.
//
// Default URL:
// /Student/Index
//
// Controller = Student
// Action = Index
// id = Optional parameter
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Student}/{action=Index}/{id?}")
    .WithStaticAssets();


// --------------------------------------------------
// 9. Run the Application
// --------------------------------------------------

// Start the ASP.NET Core application and begin listening for requests.
app.Run();