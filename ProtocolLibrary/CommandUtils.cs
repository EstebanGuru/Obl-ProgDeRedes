using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolLibrary
{
    public static class CommandUtils
    {
        //REQ Commands
        
        public const string LOGIN = "01";
        public const string INSCRIPTION = "02";
        public const string AVAILABLE_COURSES = "03";
        public const string CALIFICATIONS = "05";
        public const string DISCONNECT = "06";
        public const string SEND_FILE_REQUEST = "20";
        public const string SEND_FILE_PROCEED = "21";
        public const string SEND_FILE = "22";

        //RES Commands
        public const string CALIFICATION_ADDED_RESPONSE = "11";
        public const string LOGIN_RESPONSE = "11";
        public const string SUCCESS_MESSAGE = "80";
        public const string SPLITTED_RESPONSE = "81";
        public const string ERROR = "99";



            
    }
}
