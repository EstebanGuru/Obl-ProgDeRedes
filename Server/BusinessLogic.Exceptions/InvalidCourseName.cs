using System;

namespace Server.BusinessLogic.Exceptions
{
    [Serializable]
    public class InvalidCourseName : CourseException
    {
        private const string MESSAGE = "Course name is already registered.";
        public InvalidCourseName() : base(MESSAGE)
        {
        }
    }
}
