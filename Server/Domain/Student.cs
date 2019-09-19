using System.Collections.Generic;

namespace Server.Domain
{
    public class Student
    {
        public int Id{ get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public IList<Course> Courses { get; set; }

        public Student(int studentId, string studentEmail)
        {
            Email = studentEmail;
            Id = studentId;
            Courses = new List<Course>();
            Password = "password";
        }
    }
}
