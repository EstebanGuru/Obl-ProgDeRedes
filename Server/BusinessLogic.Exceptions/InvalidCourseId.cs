using System;

namespace Server.BusinessLogic.Exceptions
{
    [Serializable]
    public class InvalidCourseId : CourseException
    {
        private const string MESSAGE = "Course Id is already registered.";
        public InvalidCourseId() : base(MESSAGE)
        {
        }
    }
}
