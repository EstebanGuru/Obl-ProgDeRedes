using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Domain
{
    public class Course
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public IList<Student> Students { get; set; }
    }
}
