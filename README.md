# Student Registration System

This ASP.NET Core MVC project uses SQLite for student, course, enrollment, and login account data.

## Database-backed login changes

The login implementation was updated from a hard-coded demo username and password to accounts stored in the SQLite database:

- Users can register with a username, email address, and password.
- Passwords are stored as ASP.NET Core password hashes, not as plain text.
- Login verifies the submitted password against the stored hash and issues an authentication cookie.
- Protected student, course, and enrollment pages check the authenticated user instead of checking a demo session value.
- Usernames and email addresses are normalized and have unique database indexes.
- The SQLite `Logins` table was added through an EF Core migration.

## Files created

- `Models/LoginAccount.cs` — database entity for a login account, including its password hash and normalized username/email.
- `Views/Account/Register.cshtml` — registration form and validation messages.
- `Migrations/20261025031828_AddLoginAccounts.cs` — creates the `Logins` table and unique indexes.
- `Migrations/20261025031828_AddLoginAccounts.Designer.cs` — EF Core migration metadata.
- `README.md` — project and change summary.

## Files updated

- `Controllers/AccountController.cs` — looks up accounts in SQLite, verifies hashed passwords, issues and clears authentication cookies, and processes account registrations.
- `Controllers/StudentController.cs` — protects student actions using cookie-authenticated user state.
- `Controllers/CoursesController.cs` — protects course actions using cookie-authenticated user state.
- `Controllers/EnrollmentController.cs` — protects enrollment actions using cookie-authenticated user state.
- `Data/ApplicationDbContext.cs` — exposes the login accounts table and configures unique indexes for normalized usernames and email addresses.
- `Migrations/ApplicationDbContextModelSnapshot.cs` — includes the login account entity in the EF Core model snapshot.
- `Models/LoginViewModel.cs` — adds username length validation and a return URL field for login.
- `Models/RegisterViewModel.cs` — validates username/email lengths and requires a password of at least eight characters.
- `Program.cs` — configures cookie authentication and password hashing, replacing session-based demo login configuration.
- `Views/Account/Login.cshtml` — uses the project login view model and includes remember-me and registration links.
- `Views/Shared/_Layout.cshtml` — shows the authenticated username and login/logout/register links.
- `Views/Student/Index.cshtml` — displays the authenticated username instead of demo-session messaging.
- `student.db` — updated with the applied `Logins` table migration.

## Run the project

From the project directory:

```powershell
dotnet watch
```

To apply pending EF Core migrations to the configured SQLite database:

```powershell
dotnet ef database update
```

Register an account from the **Register** link before signing in. The former hard-coded demo credentials are no longer used.

## Validation performed

- `dotnet build` completed successfully with no warnings or errors.
- Login and registration pages returned HTTP 200 in a local smoke test.
- A temporary test account was registered and used to sign in and access the protected student page; the test account was removed afterward.
