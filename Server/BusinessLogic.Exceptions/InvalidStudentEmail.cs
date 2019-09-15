using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.BusinessLogic.Exceptions
{
    [Serializable]
    public class InvalidStudentEmail : StudentException
    {
        private const string MESSAGE = "Email is already registered.";
        public InvalidStudentEmail() : base(MESSAGE)
        {
        }
    }
}
