using Server.BusinessLogic;
using Server.Domain;
using System;
using DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ServerAPI
{
    class Program
    {

        static StudentLogic studentLogic;
        static CourseLogic courseLogic;
        static MessageQueue messageQueue;
        static void Main(string[] args)
        {
            studentLogic = new StudentLogic();
            courseLogic = new CourseLogic();
            string queueName = @".\Private$\adminQueue";
            Console.WriteLine("starting client api");
            if (MessageQueue.Exists(queueName))
            {
                MessageQueue.Delete(queueName);
            }
            messageQueue = MessageQueue.Create(queueName);

            // new Task(() => ListenClients()).Start();
            ListenClients();
        }

        private static void ListenClients()
        {
            Message receivedMsg;
            while (true)
            {
                receivedMsg = messageQueue.Receive();
                switch (receivedMsg.Label)
                {
                    case "RegisterTeacher":
                        Console.WriteLine("Registering teacher");
                        receivedMsg.Formatter = new BinaryMessageFormatter();
                        TeacherDTO teacherDTO = (TeacherDTO) receivedMsg.Body;
                        Console.WriteLine("teacher registered {0} : ", teacherDTO.Name);

                        break;
                    default:
                        break;
                }
                receivedMsg.Formatter = new XmlMessageFormatter(new Type[]
                    { typeof(string) });
                Console.WriteLine(receivedMsg.Body.ToString());
                Console.ReadLine();
            }
        }
    }
}
