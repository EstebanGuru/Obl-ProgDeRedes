using Server.BusinessLogic.Exceptions;
using Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.BusinessLogic
{
    public class CourseLogic
    {
        public IList<Course> Courses { get; set; }

        public CourseLogic()
        {
            Courses = new List<Course>();
        }
        public void AddCourse(int courseId, string courseName)
        {
            ValidateId(courseId);
            ValidateName(courseName);
            Course course = new Course(courseId, courseName);
            Courses.Add(course);
        }

        private void ValidateId(int courseId)
        {
            if (Courses.ToList().Exists(course => course.Id == courseId))
            {
                throw new InvalidCourseId();
            }
        }

        private void ValidateName(string courseName)
        {
            if (Courses.ToList().Exists(course => course.Name == courseName))
            {
                throw new InvalidCourseName();
            }
        }
    }
}
