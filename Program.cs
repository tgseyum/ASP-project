 using Microsoft.EntityFrameworkCore;
using myproject.Data;

var builder = WebApplication.CreateBuilder(args);

// Add MVC services
builder.Services.AddControllersWithViews();

//==============================================
// Register an in-memory cache required by session state.
builder.Services.AddDistributedMemoryCache();

// Register session services and configure session options.
builder.Services.AddSession(options =>
{
    // Set the session idle timeout to 20 minutes.
    options.IdleTimeout = TimeSpan.FromMinutes(20);

    // Prevent client-side JavaScript from accessing the session cookie.
    options.Cookie.HttpOnly = true;

    // Allow the session cookie to be stored when consent is required.
    options.Cookie.IsEssential = true;
});
//================================================
// Configure Entity Framework Core to use SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=student.db"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

// Enable session state before controller actions access it.
app.UseSession();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Student}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();


