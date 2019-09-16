using Server.BusinessLogic.Exceptions;
using System;
using System.Text.RegularExpressions;


namespace Server
{
    public class Utils
    {
        public static string ValidateEmailFormat(string email)
        {
            Regex regex = new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
            if (regex.IsMatch(email))
            {
                return email;
            }
            throw new InvalidEmail();
        }
    }
}
