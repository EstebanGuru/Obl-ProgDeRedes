using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.BusinessLogic.Exceptions
{
    [Serializable]
    public class InvalidEmail : StudentException
    {
        private const string MESSAGE = "Email is not valid.";
        public InvalidEmail() : base(MESSAGE)
        {
        }
    }
}
