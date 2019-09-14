using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket serverSocket = ConfigServer();
            new Thread(() => ListenClients(serverSocket)).Start();
            ShowMenu();
        }

        private static Socket ConfigServer()
        {
            var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.103"), 6000);
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
            Console.WriteLine("Menu:");
            Console.WriteLine("1- Create student");
            Console.WriteLine("2- Create course");
            Console.WriteLine("3- Delete course");
            Console.WriteLine("4- Assign result to student");
            Console.WriteLine("4- Send result to student");
            Console.ReadLine();
        }
    }
}
