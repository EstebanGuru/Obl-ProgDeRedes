using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ProtocolLibrary;
using System.Threading;
using ClientController;
using System.IO;

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
                if (File.Exists(@"configFile.txt")) { 
                string[] ipAddress = File.ReadAllText(@"configFile.txt").Split('-');
                string ipAddressClient = ipAddress[0];
                string ipAddressServer = ipAddress[1];
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    notificationSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    var ipEndPoint = new IPEndPoint(IPAddress.Parse(ipAddressClient), 0);
                    clientSocket.Bind(ipEndPoint);
                    clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ipAddressServer), 6000));
                    var ipNotificationEndPoint = new IPEndPoint(IPAddress.Parse(ipAddressClient), 0);
                    notificationSocket.Bind(ipNotificationEndPoint);
                    notificationSocket.Connect(new IPEndPoint(IPAddress.Parse(ipAddressServer), 6000));
                    new Thread(() => ShowMenu()).Start();
                    new Thread(() => DisplayNotifications()).Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.Message);
                Console.ReadLine();
            }
        }

        private static void ShowMenu()
        {
            ClientMenuController menuController = new ClientMenuController(
                clientSocket, studentNumber);
            menuController.Run();
        }

        private static void DisplayNotifications()
        {
            while (true)
            {
                try
                {
                    string messageType = Protocol.ReceiveHeader(notificationSocket);
                    string command = Protocol.ReceiveCommand(notificationSocket);
                    if (messageType.Equals("RES"))
                    {
                        if (command.Equals(CommandUtils.CALIFICATION_ADDED_RESPONSE))
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
