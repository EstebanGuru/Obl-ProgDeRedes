using System;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to connect");
            Console.ReadKey();
            Connect();
            Login();
        }

        private static void Connect()
        {
            var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.103"), 0);
            clientSocket.Bind(ipEndPoint);
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse("192.168.0.103"), 6000));
        }

        private static void Login()
        {
            Console.WriteLine("");
            Console.WriteLine("Login");
            Console.Write("Student number: ");
            var studentNumber = Console.ReadLine();
            Console.Write("Password: ");
            var password = Console.ReadLine();
            ShowMenu();
        }

        private static void ShowMenu()
        {
            Console.WriteLine("1- Inscription");
            Console.WriteLine("2- Available courses");
            Console.WriteLine("3- Upload");
            Console.WriteLine("4- Display results");
            Console.ReadLine();
        }
    }
}
