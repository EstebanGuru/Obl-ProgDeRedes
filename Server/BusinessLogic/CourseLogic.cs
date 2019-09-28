using Server.BusinessLogic.Exceptions;
using Server.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Server.BusinessLogic
{
    public struct InscriptionDetail
    {
        public string CourseName;
        public string Status;
        
        public InscriptionDetail(string courseName, string status)
        {
            CourseName = courseName;
            Status = status;
        }
    }

    public struct InscriptionCalification
    {
        public string CourseName;
        public int Calification;

        public InscriptionCalification(string courseName, int calification)
        {
            CourseName = courseName;
            Calification = calification;
        }
    }
    public class CourseLogic
    {
        public IList<Course> Courses { get; set; }
        public List<StudentCourse> Inscriptions { get; set; }

        public StudentLogic studentLogic = new StudentLogic();

        public CourseLogic()
        {
            Courses = new List<Course>();
            Inscriptions = new List<StudentCourse>();
        }
        public void AddCourse(int courseId, string courseName)
        {
            ValidateId(courseId);
            ValidateName(courseName);
            Course course = new Course(courseId, courseName);
            Courses.Add(course);
        }

        public void DeleteCourse(string courseName)
        {
            Courses = Courses.Where(course => course.Name != courseName).ToList();
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

        public void AddStudent(int studentId, string courseName)
        {
            if (!Courses.ToList().Exists(course => course.Name == courseName))
            {
                throw new InvalidCourseName();
            }
            if (studentLogic.GetStudent(studentId) == null)
            {
                throw new InvalidStudentId();
            }
            StudentCourse newInscription = new StudentCourse(studentId, courseName);
            Inscriptions.Add(newInscription);
        }

        public IList<InscriptionDetail> ListCourses(int studentId)
        {
            IList<InscriptionDetail> ret = new List<InscriptionDetail>();
            foreach (var course in Courses)
            {
                string courseName = course.Name;
                string status = "No anotado";
                StudentCourse inscription = Inscriptions.Find(i => i.CourseName == courseName && i.StudentId == studentId);
                if (inscription != null)
                {
                    status = "Anotado";
                    if (inscription.Calification != -1)
                    {
                        status = "Con nota asociada";
                    }
                }
                ret.Add(new InscriptionDetail(courseName, status));
            }
            return ret;
        }

        public void AddCalification(string courseName, int studentId, int calification)
        {
            StudentCourse inscription = Inscriptions.Find(i => i.CourseName == courseName && i.StudentId == studentId);
            if (inscription != null)
            {
                inscription.AddCalification(calification);
            }
        }

        public IList<InscriptionCalification> ListCalifications(int studentId)
        {
            IList<InscriptionCalification> ret = new List<InscriptionCalification>();
            List<StudentCourse> inscriptions = Inscriptions.Where(i => i.StudentId == studentId).ToList();
            foreach (var inscription in inscriptions)
            {
                if (inscription.Calification != -1)
                {
                    ret.Add(new InscriptionCalification(inscription.CourseName, inscription.Calification));
                }
            }
            return ret;
        }
    }
}
