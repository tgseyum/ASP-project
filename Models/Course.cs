namespace myproject.Models
{
    public class Course
    {
        public int Id { get; set; } //int  5, 6 "five" "5" string->number or int or double 

        public string CourseCode { get; set; } = string.Empty; //"abc", "sodv3201"  "456"

        public string CourseName { get; set; } = string.Empty;  //string no number 

        public string Instructor { get; set; } = string.Empty; //"abc"
    }
}