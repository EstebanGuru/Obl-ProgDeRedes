using System;

namespace Server.BusinessLogic.Exceptions
{
    [Serializable]
    public class StudentException : Exception
    {
        public override string Message { get; }

        public StudentException(string message)
        {
            Message = message;
        }
    }
}
