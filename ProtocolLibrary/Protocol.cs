using System;
using System.Net.Sockets;
using System.Text;

namespace ProtocolLibrary
{
    public class Protocol
    {
        public Protocol() { }

        public string ReceiveHeader(Socket socket)
        {
            try
            {
                var headerMessagge = new byte[3];
                var receive = socket.Receive(
                            headerMessagge, 0, 3, SocketFlags.None);
                if (receive == 0)
                {
                    Console.WriteLine("ERROR");
                    Console.ReadLine();
                }
                return Encoding.ASCII.GetString(headerMessagge);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR head {0}", e.Message);
                Send(socket, "RES", CommandUtils.ERROR, Encoding.ASCII.GetBytes(e.Message));
                return "";
            }

        }

        public string ReceiveCommand(Socket socket)
        {
            try
            {
                var commandInBytes = new byte[2];
                var receive = socket.Receive(
                            commandInBytes, 0, 2, SocketFlags.None);
                if (receive == 0)
                {
                    Console.WriteLine("ERROR");
                    Console.ReadLine();
                }
                string command = Encoding.ASCII.GetString(commandInBytes);
                return command;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR head {0}", e.Message);
                Send(socket, "RES", CommandUtils.ERROR, Encoding.ASCII.GetBytes(e.Message));
                return "";
            }
        }

        public void HandleResponse(string command)
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

        public void Send(Socket socket, string header, string command, byte[] data = null)
        {
            Console.WriteLine("Sending header {0} command {1} ", header, command);
            // Send header
            var headerInBytes = Encoding.ASCII.GetBytes(header);
            Console.WriteLine("headerInBytes {0} ", headerInBytes);
            socket.Send(headerInBytes);

            // Send command
            string commandString = command.ToString();
            Console.WriteLine("commandStrn {0}", commandString);
            var commandInBytes = Encoding.ASCII.GetBytes(commandString);
            socket.Send(commandInBytes);

            // Send data
            if (data != null)
            {
                var lenthOfData = data.Length;
                SendLenght(lenthOfData, socket);
                SendData(data, socket);
            }
        }

        public byte[] ReceiveData(Socket socket)
        {
            Console.WriteLine("Receiving data");
            var dataLength = ReceiveLenght(socket);
            Console.WriteLine("dataLength {0}", dataLength);
            var dataInBytes = new byte[dataLength];
            if (dataLength > 1048576)
            {
                var actuallyReceive = 0;
                var dataToReceive = 1048576;
                while (actuallyReceive < dataLength)
                {
                    if (dataLength - actuallyReceive < 1048576)
                    {
                        dataToReceive = dataLength - actuallyReceive;
                    }
                    var receive = socket.Receive(
                        dataInBytes, actuallyReceive, dataToReceive, SocketFlags.None);
                    if (receive == 0)
                    {
                        Console.WriteLine("ERROR");
                    }
                    actuallyReceive += receive;
                }
            }
            else
            {
                Console.WriteLine("receive simple {0}");
                var receive = socket.Receive(
                        dataInBytes, 0, dataLength, SocketFlags.None);
                if (receive == 0)
                {
                    Console.WriteLine("ERROR");
                }
            }
            return dataInBytes;
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
            var receive = socket.Receive(
                        dataLengthInBytes, 0, 4, SocketFlags.None);
            if (receive == 0)
            {
                Console.WriteLine("ERROR");
                Console.ReadLine();
            }
            return BitConverter.ToInt32(dataLengthInBytes, 0);
        }

        public void SendLenght(int lenthOfData, Socket socket)
        {
            var lenthOfDataInBytes = BitConverter.GetBytes(lenthOfData);
            var actuallySent = 0;
            Console.WriteLine("lenthOfDataInBytes {0}", lenthOfDataInBytes);
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
            var dataToSent = 1048576;
            while (actuallySent < lenthOfData)
            {
                if (lenthOfData - actuallySent < 1048576)
                {
                    dataToSent = lenthOfData - actuallySent;
                }
                Console.WriteLine("dataToSent {0}", dataToSent);
                var sent = socket.Send(
                    dataInBytes, actuallySent, dataToSent, SocketFlags.None);
                if (sent == 0)
                {
                    Console.WriteLine("ERROR");
                }
                actuallySent += sent;
            }
        }

    }
}
