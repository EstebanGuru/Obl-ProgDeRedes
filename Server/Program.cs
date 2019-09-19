using Server.BusinessLogic;
using Server.BusinessLogic.Exceptions;
using Server.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class Program
    {
        static StudentLogic studentLogic;
        static CourseLogic courseLogic;
        static void Main(string[] args)
        {
            Socket serverSocket = ConfigServer();
            new Thread(() => ListenClients(serverSocket)).Start();
            studentLogic = new StudentLogic();
            courseLogic = new CourseLogic();
            new Thread(() => ShowMenu()).Start();
        }

        private static Socket ConfigServer()
        {
            var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ipEndPoint = new IPEndPoint(IPAddress.Parse("172.29.2.255"), 6000);
            serverSocket.Bind(ipEndPoint);
            serverSocket.Listen(1000);
            return serverSocket;
        }

        private static void ListenClients(Socket serverSocket)
        {
            Console.WriteLine("Waiting for client to connect... ");
            while (true)
            {
                var clientSocket = serverSocket.Accept();
                new Thread(() => ClientHandler(clientSocket)).Start();
            }
        }

        private static void ClientHandler(Socket clientSocket)
        {
            Console.WriteLine("Client connected!");
            Console.ReadLine();
            while (true) ;
        }

        private static void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("*******************************");
                Console.WriteLine("*******************************");
                Console.WriteLine("Menu:");
                Console.WriteLine("1- Create student");
                Console.WriteLine("2- Create course");
                Console.WriteLine("3- Delete course");
                Console.WriteLine("4- Assign result to student");
                Console.WriteLine("4- Send result to student");
                Console.WriteLine("*******************************");
                Console.WriteLine("*******************************");
                MenuInterface();
            }
        }

        private static void MenuInterface()
        {
            string option;
            option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    CreateStudent();
                    break;
                case "2":
                    CreateCourse();
                        break;
                default:
                    break;
            }

        }
        private static void CreateStudent()
        {
            Console.WriteLine("*********  Create student  *********");
            int studentId = UI.Menu.ReadNumber("Student Id: ");
            string studentEmail = UI.Menu.ReadEmail();
            try
            {
                studentLogic.AddStudent(studentId, studentEmail);
                Console.WriteLine("Student created correctly");
            }
            catch (StudentException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Try again please.");
                CreateStudent();
            }
        }

        private static void CreateCourse()
        {
            Console.WriteLine("*********  Create course  *********");
            int courseId = UI.Menu.ReadNumber("Course Id: ");
            Console.WriteLine("Course name: ");
            string courseName = Console.ReadLine();
            try
            {
                courseLogic.AddCourse(courseId, courseName);
                Console.WriteLine("Course created correctly");
            }
            catch (CourseException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Try again please.");
                CreateCourse();
            }
        }
    }
}
