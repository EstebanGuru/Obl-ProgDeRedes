using Server.BusinessLogic.Exceptions;
using Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.BusinessLogic
{
    public class StudentLogic
    {
        public static IList<Student> Students { get; set; } = new List<Student>();

        public StudentLogic() { }

        public void AddStudent(int studentId, string studentEmail)
        {
            ValidateId(studentId);
            ValidateEmail(studentEmail);
            Student student = new Student(studentId, studentEmail);
            Students.Add(student);
        }

        public Student GetStudent(int studentId)
        {
            return Students.ToList().Find(student => student.Id == studentId);
        }

        public void ValidateId(int studentId)
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

        public void ValidateCredentials(int studentId, string password)
        {
            Student student =  Students.ToList().Find(s => s.Id == studentId && s.Password == password);
            if (student == null)
            {
                throw new InvalidCredentials();
            }
        }
    }
}
