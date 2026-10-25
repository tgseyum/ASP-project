using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using myproject.Data;
using myproject.Models;

namespace myproject.Controllers;

public class AccountController : Controller
{
    private const string AuthenticationScheme = "ApplicationCookie";
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<LoginAccount> _passwordHasher;

    public AccountController(
        ApplicationDbContext context,
        IPasswordHasher<LoginAccount> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    // Displays the login form.
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    // Processes the submitted login form.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var normalizedUsername = model.Username.Trim().ToUpperInvariant();
        var account = await _context.Logins
            .SingleOrDefaultAsync(login => login.NormalizedUsername == normalizedUsername);

        var passwordVerification = account is null
            ? PasswordVerificationResult.Failed
            : _passwordHasher.VerifyHashedPassword(account, account.PasswordHash, model.Password);

        if (account is null || passwordVerification == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError(
                string.Empty,
                "Invalid username or password.");

            return View(model);
        }

        if (passwordVerification == PasswordVerificationResult.SuccessRehashNeeded)
        {
            account.PasswordHash = _passwordHasher.HashPassword(account, model.Password);
            await _context.SaveChangesAsync();
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
            new Claim(ClaimTypes.Name, account.Username),
            new Claim(ClaimTypes.Email, account.Email)
        };
        var identity = new ClaimsIdentity(claims, AuthenticationScheme);
        await HttpContext.SignInAsync(
            AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties { IsPersistent = model.RememberMe });

        if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        {
            return LocalRedirect(model.ReturnUrl);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var normalizedUsername = model.Username.Trim().ToUpperInvariant();
        var normalizedEmail = model.Email.Trim().ToUpperInvariant();
        if (await _context.Logins.AnyAsync(account =>
                account.NormalizedUsername == normalizedUsername ||
                account.NormalizedEmail == normalizedEmail))
        {
            ModelState.AddModelError(string.Empty, "That username or email is already registered.");
            return View(model);
        }

        var account = new LoginAccount
        {
            Username = model.Username.Trim(),
            NormalizedUsername = normalizedUsername,
            Email = model.Email.Trim(),
            NormalizedEmail = normalizedEmail
        };
        account.PasswordHash = _passwordHasher.HashPassword(account, model.Password);

        _context.Logins.Add(account);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException exception) when (
            exception.InnerException is SqliteException { SqliteExtendedErrorCode: 2067 })
        {
            ModelState.AddModelError(string.Empty, "That username or email is already registered.");
            return View(model);
        }

        return RedirectToAction(nameof(Login));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }
}