using Server.BusinessLogic;
using Server.BusinessLogic.Exceptions;
using Server.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ProtocolLibrary;
using System.Collections.Generic;

namespace Server
{
    class Program
    {

        static StudentLogic studentLogic;
        static CourseLogic courseLogic;
        static Socket serverSocket;
        static Protocol Protocol;
        static List<Utils.StudentSocket> clients;
        static void Main(string[] args)
        {
            studentLogic = new StudentLogic();
            courseLogic = new CourseLogic();
            Protocol = new Protocol();
            serverSocket = ConfigServer();
            clients = new List<Utils.StudentSocket>();
            new Thread(() => ListenClients(serverSocket)).Start();
            new Thread(() => ShowMenu()).Start();
        }

        private static Socket ConfigServer()
        {
            var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.101"), 6000);
            serverSocket.Bind(ipEndPoint);
            serverSocket.Listen(1000);
            return serverSocket;
        }

        private static void ListenClients(Socket serverSocket)
        {
            while (true)
            {
                var clientSocket = serverSocket.Accept();
                var notificationSocket = serverSocket.Accept();
                new Thread(() => ClientHandler(clientSocket, notificationSocket)).Start();
            }
        }

        private static void ClientHandler(Socket clientSocket, Socket notificationSocket)
        {
            Console.WriteLine("Client connected!");
            ClientMenuHandler clientHandler = new ClientMenuHandler(clientSocket, serverSocket, notificationSocket, studentLogic, courseLogic, ref clients);
            clientHandler.Run();
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
                case "3":
                    DeleteCourse();
                    break;
                case "4":
                    AddCalification();
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

        private static void DeleteCourse()
        {
            Console.WriteLine("*********  Delete course  *********");
            Console.WriteLine("Course name: ");
            string courseName = Console.ReadLine();
            courseLogic.DeleteCourse(courseName);
            Console.WriteLine("Course deleted correctly");
        }

        private static void AddCalification()
        {
            Console.WriteLine("*********  Add calification  *********");
            try
            {
                Console.WriteLine("Course name: ");
                string courseName = Console.ReadLine();
                Console.WriteLine("Student id: ");
                int studentId = int.Parse(Console.ReadLine());
                Console.WriteLine("Calification: ");
                int calification = int.Parse(Console.ReadLine());
                courseLogic.AddCalification(courseName, studentId, calification);
                Utils.StudentSocket clientSocket = clients.Find(studentSocket => studentSocket.StudentId == studentId);
                Protocol.Send(clientSocket.ClientSocket, "RES", 10);
                Console.WriteLine("Calification added correctly");
            }
            catch (CourseException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
