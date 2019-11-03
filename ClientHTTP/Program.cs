using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ClientHTTP
{
    class Program
    {
        static MessageQueue messageQueue;
        static MessageQueue responseQueue;
        static string queueName = @".\Private$\adminQueue";
        static string responseQueueName = @".\Private$\responseQueue";
        static void Main(string[] args)
        {

            if (MessageQueue.Exists(queueName))
            {
                messageQueue = new MessageQueue(queueName);
            }
            else
            {
                return;
            }
            if (MessageQueue.Exists(responseQueueName))
            {
                MessageQueue.Delete(responseQueueName);
            }
            responseQueue = MessageQueue.Create(responseQueueName);
            Run();
            // new Task(() => Run()).Start();
        }

        private static void Run()
        {
            while (true)
            {
                Console.WriteLine("**************  Registar docente ***************");
                Console.WriteLine("Nombre: ");
                string name = Console.ReadLine();
                Console.WriteLine("Apellido : ");
                string lastName = Console.ReadLine();
                Console.WriteLine("Id : ");
                string stringId = Console.ReadLine();
                int id;
                Int32.TryParse(stringId, out id);
                Console.WriteLine("Email : ");
                string email = Console.ReadLine();
                Console.WriteLine("Password : ");
                string password = Console.ReadLine();
                TeacherDTO teacherDTO = new TeacherDTO(id, email, name, lastName, password);

                Message msg = new Message(teacherDTO);
                msg.Label = "RegisterTeacher";
                msg.Formatter = new BinaryMessageFormatter();
                try
                {
                    messageQueue.Send(msg);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
                Console.WriteLine("Mensaje enviado");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
            }
        }
    }
}
