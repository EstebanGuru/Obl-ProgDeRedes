using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using ProtocolLibrary;

namespace ClientController
{
    public class ClientMenuController
    {
        private Socket ClientSocket;
        private Protocol Protocol;
        private int StudentNumber;

        public ClientMenuController(Socket clientSocket, int studentNumber)
        {
            Protocol = new Protocol();
            ClientSocket = clientSocket;
            StudentNumber = studentNumber;
        }

        public void Run()
        {
            while (true)
            {

                if (StudentNumber != -1)
                {
                    Console.WriteLine("1- Inscription");
                    Console.WriteLine("2- Available courses");
                    Console.WriteLine("3- Upload file to course");
                    Console.WriteLine("4- Display results");
                    Console.WriteLine("5- Disconnect");
                    HandleMenu();
                }
                else
                {
                    Login();
                }
            }
        }

        private void Login()
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
                Protocol.Send(ClientSocket, "REQ", CommandUtils.LOGIN, Encoding.ASCII.GetBytes(credentials));
                HandleCommunication();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void HandleMenu()
        {
            try
            {
                string strOption = Console.ReadLine();
                int option = Int32.Parse(strOption);
                switch (option)
                {
                    case 1:
                        HandleInscription();
                        break;
                    case 2:
                        HandleAvailableCourses();
                        break;
                    case 3:
                        UploadFilePreparation();
                        break;
                    case 4:
                        HandleCalifications();
                        break;
                    case 5:
                        HandleDisconnect();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong: {0}", e.Message);
            }
        }

        private void HandleInscription()
        {
            Console.WriteLine("");
            Console.WriteLine("Inscription");
            Console.Write("Course name: ");
            string courseName = Console.ReadLine();
            string data = string.Join("#", StudentNumber, courseName);
            try
            {
                Protocol.Send(ClientSocket, "REQ", CommandUtils.INSCRIPTION, Encoding.ASCII.GetBytes(data));
                HandleCommunication();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void HandleAvailableCourses()
        {
            Console.WriteLine("");
            Console.WriteLine("Courses:");
            string data = StudentNumber.ToString();
            try
            {
                Protocol.Send(ClientSocket, "REQ", CommandUtils.AVAILABLE_COURSES, Encoding.ASCII.GetBytes(data));
                HandleCommunication();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void UploadFilePreparation()
        {
            try
            {
                Console.WriteLine("");
                Console.Write("Course name: ");
                string courseName = Console.ReadLine();
                Console.WriteLine("File name with extention: ");
                string fileName = Console.ReadLine();
                string fileInfo = string.Join("#", courseName, fileName);
                Protocol.Send(ClientSocket, "REQ", CommandUtils.SEND_FILE_REQUEST, Encoding.ASCII.GetBytes(fileInfo));
                HandleCommunication();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error sending file");
                Console.WriteLine(e.Message);
            }
        }
        private void HandleCalifications()
        {
            Console.WriteLine("");
            Console.WriteLine("Califications");
            try
            {
                string data = StudentNumber.ToString();
                Protocol.Send(ClientSocket, "REQ", CommandUtils.CALIFICATIONS, Encoding.ASCII.GetBytes(data));
                HandleCommunication();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void HandleDisconnect()
        {
            try
            {
                Protocol.Send(ClientSocket, "REQ", CommandUtils.DISCONNECT);
                Console.WriteLine("Disconnected");
                Environment.Exit(0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void HandleCommunication()
        {
            string header = Protocol.ReceiveHeader(ClientSocket);
            string command = Protocol.ReceiveCommand(ClientSocket);
            if (header.Equals("RES"))
            {
                HandleResponse(command);
            }
            else
            {
                Console.WriteLine("Something went wrong");
            }
        }
        private void HandleResponse(string command)
        {
            string response = Encoding.ASCII.GetString(Protocol.ReceiveData(ClientSocket)); ;
            switch (command)
            {
                case CommandUtils.LOGIN_RESPONSE:
                    StudentNumber = Int32.Parse(response);
                    break;
                case CommandUtils.SPLITTED_RESPONSE:
                    var splittedResponse = response.Split('#');
                    foreach (var item in splittedResponse)
                    {
                        Console.WriteLine(item);
                    }
                    Console.WriteLine("");
                    break;
                case CommandUtils.SUCCESS_MESSAGE:
                    Console.WriteLine(response);
                    break;
                case CommandUtils.ERROR:
                    Console.WriteLine("Something went wrong: {0}", response);
                    break;
                case CommandUtils.SEND_FILE_PROCEED:
                    SendFile(response);
                    break;
                default:
                    Console.WriteLine("Nothing to do with {0}", command);
                    break;
            }
        }

        private void SendFile(string fileName)
        {
            var fileStream = new FileStream(@fileName, FileMode.Open, FileAccess.Read);
            var lenthOfData = (int)fileStream.Length;
            byte[] dataInBytes = new byte[lenthOfData];
            fileStream.Read(dataInBytes, 0, dataInBytes.Length);
            Protocol.Send(ClientSocket, "REQ", CommandUtils.SEND_FILE, dataInBytes);
            HandleCommunication();
        }
    }
}
