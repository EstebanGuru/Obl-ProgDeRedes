using System;
using System.Net.Sockets;
using System.Text;

namespace ProtocolLibrary
{
    public class Protocol
    {
        public Protocol() { }

        public string RecieveHeader(Socket socket)
        {
            var headerMessagge = new byte[3];
            socket.Receive(headerMessagge);
            return Encoding.ASCII.GetString(headerMessagge);
        }

        public int RecieveCommand(Socket socket)
        {

            var commandInBytes = new byte[2];
            socket.Receive(commandInBytes);
            string commandValue = Encoding.ASCII.GetString(commandInBytes);
            int value = int.Parse(commandValue);
            return value;
        }

        public void HandleResponse(int command)
        {
            throw new NotImplementedException();
        }

        public void SendResponse(Socket socket)
        {
            var messagge = Encoding.ASCII.GetBytes("RES");
            socket.Send(messagge);
        }

        public void SendRequest(Socket socket)
        {
            var messagge = Encoding.ASCII.GetBytes("REQ");
            socket.Send(messagge);
        }

        public void SendCommand(Socket socket, string command)
        {
            var messagge = Encoding.ASCII.GetBytes(command);
            socket.Send(messagge);
        }

        public void Send (Socket socket, string header, int command, string data = null)
        {
            // Send header
            var headerInBytes = Encoding.ASCII.GetBytes(header);
            socket.Send(headerInBytes);

            // Send command
            var commandInBytes = Encoding.ASCII.GetBytes(command.ToString());
            socket.Send(commandInBytes);

            // Send data
            if (data != null)
            {
                var lenthOfData = data.Length;
                SendLenght(lenthOfData, socket);
                byte[] dataInBytes = new byte[lenthOfData];
                dataInBytes = Encoding.ASCII.GetBytes(data);
                SendData(dataInBytes, socket);
            }
        }

        public string ReceiveData(Socket socket)
        {
            var dataLength = ReceiveLenght(socket);
            var dataInBytes = new byte[dataLength];
            socket.Receive(dataInBytes);
            return Encoding.ASCII.GetString(dataInBytes);
        }

        public string ReceiveString(Socket socket)
        {
            var dataLength = ReceiveLenght(socket);
            var dataInBytes = new byte[dataLength];
            socket.Receive(dataInBytes);
            return Encoding.ASCII.GetString(dataInBytes);
        }

        public int ReceiveLenght(Socket socket)
        {
            var dataLengthInBytes = new byte[4];
            socket.Receive(dataLengthInBytes);
            return BitConverter.ToInt32(dataLengthInBytes, 0);
        }

        public void SendLenght(int lenthOfData, Socket socket)
        {
            var lenthOfDataInBytes = BitConverter.GetBytes(lenthOfData);
            var actuallySent = 0;
            while (actuallySent < lenthOfDataInBytes.Length)
            {
                var sent = socket.Send(
                    lenthOfDataInBytes, actuallySent, lenthOfDataInBytes.Length - actuallySent, SocketFlags.None);
                if (sent == 0)
                {
                    Console.WriteLine("ERROR");
                }
                actuallySent += sent;
            }
        }

        public void SendData(byte[] dataInBytes, Socket socket)
        {
            var lenthOfData = dataInBytes.Length;
            var actuallySent = 0;
            while (actuallySent < lenthOfData)
            {
                var sent = socket.Send(
                    dataInBytes, actuallySent, lenthOfData - actuallySent, SocketFlags.None);
                if (sent == 0)
                {
                    Console.WriteLine("ERROR");
                }
                actuallySent += sent;
            }
        }

    }
}
