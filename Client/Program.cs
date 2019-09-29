﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ProtocolLibrary;
using System.Threading;
using ClientController;


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
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                notificationSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.44"), 0);
                clientSocket.Bind(ipEndPoint);
                clientSocket.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.44"), 6000));
                var ipNotificationEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.44"), 0);
                notificationSocket.Bind(ipNotificationEndPoint);
                notificationSocket.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.44"), 6000));
                new Thread(() => ShowMenu()).Start();
                new Thread(() => DisplayNotifications()).Start();
            }
            catch (Exception e)
            {

            }
        }

        private static void ShowMenu()
        {
            ClientMenuController menuController = new ClientMenuController(
                clientSocket, notificationSocket,studentNumber);
            menuController.Run();
        }

        private static void DisplayNotifications()
        {
            while (true)
            {
                try
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
                catch (Exception e)
                {

                }
            }
        }
    }
}
