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
            string credentials = Protocol.ReceiveData(ClientSocket);
            var arrayCredentials = credentials.Split('#');
            int id = Int32.Parse(arrayCredentials[0]);
            string password = arrayCredentials[1];
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
    }
}
