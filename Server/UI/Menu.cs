using System;
using Server.BusinessLogic.Exceptions;

namespace Server.UI
{
    public class Menu
    {
        public static string ReadEmail()
        {
            Console.WriteLine("Email: ");
            while (true)
            {
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
                }
            }
        }

        public static int ReadNumber()
        {
            while (true)
            {
                Console.WriteLine("StudentId: ");
                string stringStudentId = Console.ReadLine();
                int studentId;
                if (Int32.TryParse(stringStudentId, out studentId))
                {
                    return studentId;
                }
                else
                {
                    Console.WriteLine("Please enter a valid number.");
                }
            }
        }
    }

}
