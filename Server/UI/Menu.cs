using System;
using Server.BusinessLogic.Exceptions;

namespace Server.UI
{
    public class Menu
    {
        public static string ReadEmail()
        {
            Console.WriteLine("Email: ");
            string studentEmail = Console.ReadLine();
            try
            {
                Utils.ValidateEmailFormat(studentEmail);
                return studentEmail;
            }
            catch (InvalidEmail e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Enter a valid Email: ");
                return ReadEmail();
            }
        }

        public static int ReadNumber(string field)
        {
            Console.WriteLine(field);
            string stringStudentId = Console.ReadLine();
            int studentId;
            if (Int32.TryParse(stringStudentId, out studentId))
            {
                return studentId;
            }
            else
            {
                Console.WriteLine("Please enter a valid number.");
                return ReadNumber(field);
            }
        }
    }

}
