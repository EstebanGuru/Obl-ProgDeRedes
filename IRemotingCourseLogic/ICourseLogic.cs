using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRemotingCourseLogic
{
    public interface ICourseLogic
    {
        string AddCalificationRemoting(string courseName, int studentId, int calification);

        List<StudentCourse> GetTopCalifications();
    }

    [Serializable]
    public class StudentCourse
    {
        public int StudentId { get; set; }
        public string CourseName { get; set; }
        public int Calification { get; set; }

        public StudentCourse(int studentId, string courseName)
        {
            StudentId = studentId;
            CourseName = courseName;
            Calification = -1;
        }

        public void AddCalification(int calification)
        {
            Calification = calification;
        }
    }
}
