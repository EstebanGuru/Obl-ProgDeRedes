using Server.BusinessLogic.Exceptions;
using Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Server.BusinessLogic
{
    public class StudentLogic
    {
        public static IList<Student> Students { get; set; }
        public void AddStudent(int studentId, string studentEmail)
        {
            ValidateId(studentId);
            ValidateEmail(studentEmail);
            Student student = new Student(studentId, studentEmail);
            Students.Add(student);
        }

        private void ValidateId(int studentId)
        {
            if (Students.ToList().Exists(student => student.StudentId == studentId))
            {
                throw new InvalidStudentId();
            }
        }

        private void ValidateEmail(string studentEmail)
        {
            string email = ValidateEmailFormat(studentEmail);
            if (Students.ToList().Exists(student => student.Email == email))
            {
                throw new InvalidStudentEmail();
            }
        }
        public string ValidateEmailFormat(string email)
        {
            Regex regex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
            if (regex.IsMatch(email))
            {
                return email;
            }
            throw new InvalidEmail();
        }
    }
}
