# Student and Course Management System

## Project Overview

This is a basic Student and Course Management System developed using ASP.NET Core MVC. The application allows users to register and manage basic student information and course information. It uses SQLite as the database for storing application data.

## Features

- Register and manage basic student information.
- Add and view course information.
- Store student and course data in an SQLite database.
- Display course records using the jQuery DataTables plug-in.
- Apply Bootstrap for styling and layout.
- Use custom CSS to improve the appearance of application pages.
- Implement partial front-end and back-end data validation and verification.

## Technologies Used

| Technology | Purpose |
|---|---|
| ASP.NET Core MVC | Application structure and request handling |
| C# | Server-side programming |
| Razor Views | Generate dynamic HTML pages |
| SQLite | Database storage |
| Entity Framework Core | Database access and data management |
| Bootstrap | Responsive layout and styling |
| Custom CSS | Additional page styling |
| jQuery | JavaScript library used by DataTables |
| jQuery DataTables | Table searching, sorting, and pagination |
| HTML | Page structure |

## Database

The application uses **SQLite** to store student and course information. Student and course records are maintained in separate database tables within the database file.

Entity Framework Core is used to interact with the database.

## Data Validation and Verification

Basic data validation and verification are partially implemented on both the front end and the back end.

- **Front-end validation:** Provides basic input validation and user feedback.
- **Back-end validation:** Uses server-side validation to check submitted data before processing it.

Validation and verification are not yet comprehensive and may require further improvements.

## Security

**Security features have not yet been implemented comprehensively.** This application is intended as a basic learning project and should not be used in a production environment without additional security measures.

Potential future improvements include authentication, authorization, comprehensive input validation, and protection against common web application vulnerabilities.

## Future Improvements

- Implement user authentication and authorization.
- Improve front-end and back-end validation.
- Add edit and delete functionality for student and course records.
- Improve error handling and logging.
- Strengthen application security.
- Enhance the user interface and accessibility.

## Purpose

This project demonstrates the fundamentals of building a database-driven web application using ASP.NET Core MVC, Entity Framework Core, SQLite, Bootstrap, and jQuery DataTables.
