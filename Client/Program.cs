using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ProtocolLibrary;

namespace Client
{
    class Program
    {
        private static Socket clientSocket;
        private static Protocol Protocol;
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
            var ipEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.44"), 0);
            clientSocket.Bind(ipEndPoint);
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.44"), 6000));
            Login();
        }

        private static void Login()
        {
            while (true)
            {
                Console.WriteLine("");
                Console.WriteLine("Login");
                Console.Write("Student number: ");
                string studentNumber = Console.ReadLine();
                Console.Write("Password: ");
                string password = Console.ReadLine();
                try
                {
                    Protocol.SendRequest(clientSocket);
                    Protocol.SendCommand(clientSocket, "1");
                    SendString(studentNumber);
                    SendString(password);
                    string header = Protocol.RecieveHeader(clientSocket);
                    int command = Protocol.RecieveCommand(clientSocket);
                    if (header.Equals("REQ"))
                    {
                       
                    }
                    else if (header.Equals("RES"))
                    {
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
            
        }

        private static void SendString(string data)
        {
            var lenthOfData = data.Length;
            Protocol.SendLenght(lenthOfData, clientSocket);
            byte[] dataInBytes = new byte[lenthOfData];
            dataInBytes = Encoding.ASCII.GetBytes(data);
            Protocol.SendData(dataInBytes, clientSocket);
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
