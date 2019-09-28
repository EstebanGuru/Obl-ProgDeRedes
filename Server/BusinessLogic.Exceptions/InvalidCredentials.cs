using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.BusinessLogic.Exceptions
{
    [Serializable]
    public class InvalidCredentials : StudentException
    {
        private const string MESSAGE = "Incorrect student id or password.";
        public InvalidCredentials() : base(MESSAGE)
        {
        }
    }
}
