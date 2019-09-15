using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.BusinessLogic.Exceptions
{
    [Serializable]
    public class InvalidStudentId : StudentException
    {
        private const string MESSAGE = "Student id is already registered.";
        public InvalidStudentId() : base(MESSAGE)
        {
        }
    }
}