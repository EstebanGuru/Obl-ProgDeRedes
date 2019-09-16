using Server.BusinessLogic.Exceptions;
using Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.BusinessLogic
{
    public class StudentLogic
    {
        public IList<Student> Students { get; set; }

        public StudentLogic()
        {
            Students = new List<Student>();
        }
        public void AddStudent(int studentId, string studentEmail)
        {
            ValidateId(studentId);
            ValidateEmail(studentEmail);
            Student student = new Student(studentId, studentEmail);
            Students.Add(student);
        }

        private void ValidateId(int studentId)
        {
            if (Students.ToList().Exists(student => student.Id == studentId))
            {
                throw new InvalidStudentId();
            }
        }

        private void ValidateEmail(string studentEmail)
        {
            if (Students.ToList().Exists(student => student.Email == studentEmail))
            {
                throw new InvalidStudentEmail();
            }
        }
    }
}
