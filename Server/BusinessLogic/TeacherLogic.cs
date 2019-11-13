using Server.BusinessLogic.Exceptions;
using Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.BusinessLogic
{
    public class TeacherLogic
    {
        public static IList<Teacher> Teachers { get; set; } = new List<Teacher>();

        public TeacherLogic() { }

        public void AddTeacher(int id, string email, string name, string lastName)
        {
            lock (Teachers)
            {
                ValidateId(id);
                Teacher teacher = new Teacher(id, email, name, lastName);
                Teachers.Add(teacher);
            }
        }

        public Teacher GetTeacher(int id)
        {
            return Teachers.ToList().Find(teacher => teacher.Id == id);
        }

        public void ValidateId(int id)
        {
            if (Teachers.ToList().Exists(teacher => teacher.Id == id))
            {
                throw new InvalidId();
            }
        }

        public void ValidateCredentials(int id, string password)
        {
            lock (Teachers)
            {
                Teacher teacher =  Teachers.ToList().Find(s => s.Id == id && s.Password == password);
                if (teacher == null)
                {
                    throw new InvalidCredentials();
                }
            }
        }
    }
}
