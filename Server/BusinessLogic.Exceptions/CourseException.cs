using System;

namespace Server.BusinessLogic.Exceptions
{
    [Serializable]
    public class CourseException : Exception
    {
        public override string Message { get; }

        public CourseException(string message)
        {
            Message = message;
        }
    }
}
