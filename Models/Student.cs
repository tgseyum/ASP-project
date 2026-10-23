// namespace myproject.Models
// {
//     public class Student
//     {
//         public int Id { get; set; }

//         public string Name { get; set; } = string.Empty;

//         public string Program { get; set; } = string.Empty;
//     }
// }

//Student info add Email and phone
//Course add creditHour
//

// using System.ComponentModel.DataAnnotations;

// namespace myproject.Models
// {
//     public class Student
//     {
//         public int Id { get; set; }

//         [Required(ErrorMessage = "Student name is required.")]
//         [RegularExpression()]
//         [StringLength(100,
//             ErrorMessage = "Name cannot exceed 100 characters.")]
//         [Display(Name = "Student Name")]
//         public string Name { get; set; } = string.Empty;

//         [Required(ErrorMessage = "Program is required.")]
//         [StringLength(100,
//             ErrorMessage = "Program cannot exceed 100 characters.")]
//         [Display(Name = "Program Name")]
//         public string Program { get; set; } = string.Empty;
//     }
// }

using System.ComponentModel.DataAnnotations;

namespace myproject.Models
{
    public class Student
    {
        public int Id { get; set; }

        // Required rejects null, empty, and whitespace-only input.
        [Required(ErrorMessage = "Please enter the student's name.")]
        // Only letters, spaces, hyphens, and apostrophes are permitted.
        [RegularExpression(@"^[a-zA-Z\s'-]+$",
            ErrorMessage = "Error going on ....")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the student's program.")]
        // Program must contain a letter and use only the allowed characters.
        [RegularExpression(@"^(?=.*[a-zA-Z])[a-zA-Z0-9\s&()'-]+$",
            ErrorMessage = "Program must contain at least one letter and only valid characters.")]
        public string Program { get; set; } = string.Empty;
    }
}