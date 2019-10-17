using ProtocolLibrary;
using Server.BusinessLogic;
using System.IO;
using Server.BusinessLogic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Server.Domain;

namespace Server
{
    public class ClientMenuHandler
    {
        private Socket ClientSocket;
        private Socket NotificationSocket;
        private Protocol Protocol;
        private StudentLogic studentLogic;
        private CourseLogic courseLogic;
        private List<Utils.StudentSocket> clients;
        private int studentId;

        public ClientMenuHandler(Socket clientSocket, Socket notificationSocket, StudentLogic studentLogicHandler, CourseLogic courseLogicHandler, ref List<Utils.StudentSocket> pClients)
        {
            Protocol = new Protocol();
            ClientSocket = clientSocket;
            NotificationSocket = notificationSocket;
            studentLogic = studentLogicHandler;
            courseLogic = courseLogicHandler;
            clients = pClients;
        }

        public void Run()
        {
            while (true)
            {
                try
                {
                    string messageType = Protocol.ReceiveHeader(ClientSocket);
                    string command = Protocol.ReceiveCommand(ClientSocket);
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
                catch (Exception e)
                {
                    Console.WriteLine("ERROR: {0}", e.Message);
                    return;
                }
            }
        }

        private void HandleResponse(string command)
        {
            throw new NotImplementedException();
        }

        private void HandleRequest(string command)
        {
            switch (command)
            {
                case CommandUtils.LOGIN:
                    HandleLogin();
                    break;
                case CommandUtils.INSCRIPTION:
                    HandleInscription();
                    break;
                case CommandUtils.AVAILABLE_COURSES:
                    HandleAvailableCourses();
                    break;
                case CommandUtils.CALIFICATIONS:
                    HandleCalifications();
                    break;
                case CommandUtils.DISCONNECT:
                    HandleDisconnect();
                    break;
                case CommandUtils.SEND_FILE_REQUEST:
                    HandleReceiveFile();
                    break;
                default:
                    Console.WriteLine("Invalid command received {0}", command);
                    break;
            }
        }

        private void HandleLogin()
        {
            string credentials = Encoding.ASCII.GetString(Protocol.ReceiveData(ClientSocket));

            var arrayCredentials = credentials.Split('#');
            studentId = Int32.Parse(arrayCredentials[0]);
            string password = arrayCredentials[1];
            try
            {
                studentLogic.ValidateCredentials(studentId, password);
                Protocol.Send(ClientSocket, "RES", CommandUtils.LOGIN_RESPONSE, Encoding.ASCII.GetBytes(string.Join("#", studentId)));
                clients.Add(new Utils.StudentSocket(studentId, NotificationSocket));
            }
            catch (StudentException e)
            {
                Protocol.Send(ClientSocket, "RES", CommandUtils.ERROR, Encoding.ASCII.GetBytes(e.Message));
            }
        }

        private void HandleInscription()
        {
            string data = Encoding.ASCII.GetString(Protocol.ReceiveData(ClientSocket));
            var arrayData = data.Split('#');
            int studentId = Int32.Parse(arrayData[0]);
            string courseName = arrayData[1];
            try
            {
                courseLogic.AddStudent(studentId, courseName);
                Protocol.Send(ClientSocket, "RES", CommandUtils.SUCCESS_MESSAGE, Encoding.ASCII.GetBytes("Inscription created successfully."));
            }
            catch (StudentException e)
            {
                Protocol.Send(ClientSocket, "RES", CommandUtils.ERROR, Encoding.ASCII.GetBytes(e.Message));
            }
            catch (CourseException e)
            {
                Protocol.Send(ClientSocket, "RES", CommandUtils.ERROR, Encoding.ASCII.GetBytes(e.Message));
            }
        }

        private void HandleAvailableCourses()
        {
            string data = Encoding.ASCII.GetString(Protocol.ReceiveData(ClientSocket));
            int studentId = Int32.Parse(data);

            IList<InscriptionDetail> courses = courseLogic.ListCourses(studentId);
            string[] response = new string[courses.Count];
            int index = 0;
            foreach (var course in courses)
            {
                response[index] = course.CourseName + " - " + course.Status;
                index++;
            }
            string strResponse = String.Join("#", response);
            if (strResponse == "")
            {
                strResponse = "No courses available";
            }
            Protocol.Send(ClientSocket, "RES", CommandUtils.SPLITTED_RESPONSE, Encoding.ASCII.GetBytes(String.Join("#", strResponse)));
        }

        private void HandleCalifications()
        {
            string data = Encoding.ASCII.GetString(Protocol.ReceiveData(ClientSocket));
            int studentId = Int32.Parse(data);

            IList<InscriptionCalification> califications = courseLogic.ListCalifications(studentId);
            string[] response = new string[califications.Count];
            int index = 0;
            foreach (var calification in califications)
            {
                response[index] = calification.CourseName + " - " + calification.Calification;
                index++;
            }
            string strResponse = String.Join("#", response);
            if (strResponse == "")
            {
                strResponse = "No califications available";
            }
            Protocol.Send(ClientSocket, "RES", CommandUtils.SPLITTED_RESPONSE, Encoding.ASCII.GetBytes(String.Join("#", strResponse)));
        }

        private void HandleDisconnect()
        {
            ClientSocket.Shutdown(SocketShutdown.Both);
            NotificationSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Disconnect(true);
            NotificationSocket.Disconnect(true);
        }

        private void HandleReceiveFile()
        {
            try
            {
                string fileInfo = Encoding.ASCII.GetString(Protocol.ReceiveData(ClientSocket));
                var arrayFileInfo = fileInfo.Split('#');
                string courseName = arrayFileInfo[0];
                string fileName = arrayFileInfo[1];
                StudentCourse inscription = courseLogic.Inscriptions.Find(i => i.CourseName == courseName && i.StudentId == studentId);
                if (inscription != null)
                {
                    Protocol.Send(ClientSocket, "RES", CommandUtils.SEND_FILE_PROCEED, Encoding.ASCII.GetBytes(fileName));
                    string messageType = Protocol.ReceiveHeader(ClientSocket);
                    string command = Protocol.ReceiveCommand(ClientSocket);
                    if (command.Equals(CommandUtils.SEND_FILE))
                    {
                        string filePath = courseName + "/" + fileName;
                        FileInfo file = new FileInfo(filePath);
                        file.Directory.Create(); // If the directory already exists, this method does nothing.

                        byte[] data = Protocol.ReceiveData(ClientSocket);
                        File.WriteAllBytes(file.FullName, data);
                        Protocol.Send(ClientSocket, "RES", CommandUtils.SUCCESS_MESSAGE, Encoding.ASCII.GetBytes("File uploaded succesfully."));

                    }
                    else
                    {
                        Protocol.Send(ClientSocket, "RES", CommandUtils.ERROR, Encoding.ASCII.GetBytes("Wrong command received"));
                    }
                }
                else
                {
                    Protocol.Send(ClientSocket, "RES", CommandUtils.ERROR, Encoding.ASCII.GetBytes("Course doesn't exist or user is not registered in it."));
                }
            }
            catch (Exception e)
            {
                Protocol.Send(ClientSocket, "RES", CommandUtils.ERROR, Encoding.ASCII.GetBytes(e.Message));
            }

        }

    }
}
