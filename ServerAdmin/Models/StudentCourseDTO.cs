using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerAdmin.Models
{
    public class StudentCourseDTO
    {
        public int StudentId { get; set; }
        public string CourseName { get; set; }
        public int Calification { get; set; }
    }
}