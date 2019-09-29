using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolLibrary
{
    public static class CommandUtils
    {
        //REQ Commands
        
        public const int LOGIN = 1;
        public const int INSCRIPTION = 2;
        public const int AVAILABLE_COURSES = 3;
        public const int CALIFICATIONS = 5;
        public const int DISCONNECT = 6;

        //RES Commands
        public const int LOGIN_RESPONSE = 11;
        public const int SUCCESS_MESSAGE = 80;
        public const int SPLITTED_RESPONSE = 81;
        public const int ERROR = 99;



            
    }
}
