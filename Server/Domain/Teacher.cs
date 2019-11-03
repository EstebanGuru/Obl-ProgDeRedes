using System.Collections.Generic;

namespace Server.Domain
{
    public class Teacher
    {
        public int Id{ get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public IList<Course> Courses { get; set; }

        public Teacher(int id, string email, string name, string lastName)
        {
            Email = email;
            Id = id;
            Name = name;
            LastName = lastName;
            Courses = new List<Course>();
            Password = "password";
        }
    }
}
