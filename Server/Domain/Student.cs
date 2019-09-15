using System.Collections.Generic;

namespace Server.Domain
{
    public class Student
    {
        public int StudentId{ get; set; }
        public string Email { get; set; }
        public IList<Course> Courses { get; set; }

        public Student(int studentId, string studentEmail)
        {
            Email = studentEmail;
            StudentId = studentId;
            Courses = new List<Course>();
        }
    }
}
