using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ProtocolLibrary;
using System.Threading;


namespace Client
{
    class Program
    {
        private static Socket clientSocket;
        private static Socket notificationSocket;
        private static Protocol Protocol;
        private static int studentNumber = -1;
        static void Main(string[] args)
        {
            Protocol = new Protocol();
            Console.WriteLine("Press any key to connect");
            Console.ReadKey();
            Connect();
        }

        private static void Connect()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            notificationSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.101"), 0);
            clientSocket.Bind(ipEndPoint);
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse("192.168.0.101"), 6000));
            var ipNotificationEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.101"), 0);
            notificationSocket.Bind(ipNotificationEndPoint);
            notificationSocket.Connect(new IPEndPoint(IPAddress.Parse("192.168.0.101"), 6000));
            new Thread(() => ShowMenu()).Start();
            new Thread(() => DisplayNotifications()).Start();
        }

        private static void Login()
        {
            Console.WriteLine("");
            Console.WriteLine("Login");
            Console.Write("Student number: ");
            string pStudentNumber = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();
            string credentials = string.Join("#", pStudentNumber, password);
            try
            {
                Protocol.Send(clientSocket, "REQ", 1, credentials);
                string header = Protocol.ReceiveHeader(clientSocket);
                int command = Protocol.ReceiveCommand(clientSocket);
                if (header.Equals("RES"))
                {
                    if (command == 80)
                    {
                        studentNumber = Int32.Parse(pStudentNumber);
                    }
                    if (command == 99)
                    {
                        string mesage = Protocol.ReceiveData(clientSocket);
                        Console.WriteLine("serivdor responde {0}", mesage);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void ShowMenu()
        {
            while (true)
            {
                if (studentNumber != -1)
                {
                    Console.WriteLine("1- Inscription");
                    Console.WriteLine("2- Available courses");
                    Console.WriteLine("3- Upload");
                    Console.WriteLine("4- Display results");
                    Console.WriteLine("5- Disconnect");
                    HandleMenu();
                }
                else
                {
                    Login();
                }
            }
        }

        private static void DisplayNotifications()
        {
            while (true)
            {
                string messageType = Protocol.ReceiveHeader(notificationSocket);
                int command = Protocol.ReceiveCommand(notificationSocket);
                if (messageType.Equals("RES"))
                {
                    if (command == 10)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Calification added");
                        Console.WriteLine("");

                    }
                }
            }
        }

        private static void HandleInscription()
        {
            Console.WriteLine("");
            Console.WriteLine("Inscription");
            Console.Write("Course name: ");
            string courseName = Console.ReadLine();
            string data = string.Join("#", studentNumber, courseName);
            try
            {
                Protocol.Send(clientSocket, "REQ", 2, data);
                string header = Protocol.ReceiveHeader(clientSocket);
                int command = Protocol.ReceiveCommand(clientSocket);
                if (header.Equals("RES"))
                {
                    if (command == 80)
                    {
                        Console.WriteLine("Inscription created successfully.");
                    }
                    if (command == 99)
                    {
                        string mesage = Protocol.ReceiveData(clientSocket);
                        Console.WriteLine("serivdor responde {0}", mesage);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void HandleAvailableCourses()
        {
            Console.WriteLine("");
            Console.WriteLine("Courses:");
            string data = studentNumber.ToString();
            try
            {
                Protocol.Send(clientSocket, "REQ", 3, data);
                string header = Protocol.ReceiveHeader(clientSocket);
                int command = Protocol.ReceiveCommand(clientSocket);
                if (header.Equals("RES"))
                {
                    if (command == 80)
                    {
                        string response = Protocol.ReceiveData(clientSocket);
                        var courses = response.Split('#');
                        foreach (var course in courses)
                        {
                            Console.WriteLine(course);
                        }
                        Console.WriteLine("");
                    }
                    if (command == 99)
                    {
                        string mesage = Protocol.ReceiveData(clientSocket);
                        Console.WriteLine("serivdor responde {0}", mesage);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private static void HandleCalifications()
        {
            Console.WriteLine("");
            Console.WriteLine("Califications");
            string data = studentNumber.ToString();
            try
            {
                Protocol.Send(clientSocket, "REQ", 5, data);
                string header = Protocol.ReceiveHeader(clientSocket);
                int command = Protocol.ReceiveCommand(clientSocket);
                if (header.Equals("RES"))
                {
                    if (command == 80)
                    {
                        string response = Protocol.ReceiveData(clientSocket);
                        var califications = response.Split('#');
                        foreach (var calification in califications)
                        {
                            Console.WriteLine(calification);
                        }
                        Console.WriteLine("");
                    }
                    if (command == 99)
                    {
                        string mesage = Protocol.ReceiveData(clientSocket);
                        Console.WriteLine("serivdor responde {0}", mesage);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void HandleMenu()
        {
            try
            {
                int option = Int32.Parse(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        HandleInscription();
                        break;
                    case 2:
                        HandleAvailableCourses();
                        break;
                    case 4:
                        HandleCalifications();
                        break;
                    default:
                        Console.WriteLine("hola");
                        break;
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
