using ProtocolLibrary;
using Server.BusinessLogic;
using Server.BusinessLogic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class ClientMenuHandler
    {
        private Socket ClientSocket;
        private Socket ServerSocket;
        private Protocol Protocol;
        private StudentLogic studentLogic;

        public ClientMenuHandler(Socket clientSocket, Socket serverSocket, StudentLogic studentLogicHandler)
        {
            Protocol = new Protocol();
            ClientSocket = clientSocket;
            ServerSocket = serverSocket;
            studentLogic = studentLogicHandler;
        }

        public void Run()
        {
            while (true)
            {
                string messageType = Protocol.RecieveHeader(ClientSocket);
                int command = Protocol.RecieveCommand(ClientSocket);
                if (messageType.Equals("REQ"))
                {
                    HandleRequest(command);
                }
                else if (messageType.Equals("RES"))
                {
                    HandleResponse(command);
                }
                else
                {
                    Console.WriteLine("Something went wrong");
                }
            }
        }

        private void HandleResponse(int command)
        {
            throw new NotImplementedException();
        }

        private void HandleRequest(int command)
        {
            switch (command)
            {
                case 1:
                    HandleLogin();
                    break;
                default:
                    break;
            }
        }

        private void HandleLogin()
        {
            string stringId = ReceiveString();
            int id = int.Parse(stringId);
            string password = ReceiveString();
            try
            {
                studentLogic.ValidateCredentials(id, password);
                Protocol.Send(ClientSocket, "RES", 80);
            }
            catch (StudentException e)
            {
                Protocol.Send(ClientSocket, "RES", 99, e.Message);
            }
        }

        private void HandleLoginError(string message)
        {
            Protocol.SendResponse(ClientSocket);
            Protocol.SendCommand(ClientSocket, "99");
            SendString(message);
        }

        private void SendString(string data)
        {
            var lenthOfData = data.Length;
            Protocol.SendLenght(lenthOfData, ClientSocket);
            byte[] dataInBytes = new byte[lenthOfData];
            dataInBytes = Encoding.ASCII.GetBytes(data);
            Protocol.SendData(dataInBytes, ClientSocket);
        }

        private string ReceiveString()
        {
            var dataLength = Protocol.ReceiveLenght(ClientSocket);
            var dataInBytes = new byte[dataLength];
            ClientSocket.Receive(dataInBytes);
            return Encoding.ASCII.GetString(dataInBytes);
        }



    }
}
