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
            var ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.101"), 0);
            clientSocket.Bind(ipEndPoint);
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse("192.168.0.101"), 6000));
            new Thread(() => ShowMenu()).Start();
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
                string header = Protocol.RecieveHeader(clientSocket);
                int command = Protocol.RecieveCommand(clientSocket);
                if (header.Equals("REQ"))
                {

                }
                else if (header.Equals("RES"))
                {
                    if (command == 80)
                    {
                        studentNumber = Int32.Parse(pStudentNumber);
                    }
                    if (command == 99)
                    {
                        string mesage = Protocol.ReceiveString(clientSocket);
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
                    Console.ReadLine();
                    HandleMenu();
                }
                else
                {
                    Login();
                }
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
                        Console.WriteLine("hola");
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
