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
        private Socket NotificationSocket;
        private Protocol Protocol;
        private StudentLogic studentLogic;
        private CourseLogic courseLogic;
        private List<Utils.StudentSocket> clients;

        public ClientMenuHandler(Socket clientSocket, Socket serverSocket, Socket notificationSocket, StudentLogic studentLogicHandler, CourseLogic courseLogicHandler, ref List<Utils.StudentSocket> pClients)
        {
            Protocol = new Protocol();
            ClientSocket = clientSocket;
            ServerSocket = serverSocket;
            NotificationSocket = notificationSocket;
            studentLogic = studentLogicHandler;
            courseLogic = courseLogicHandler;
            clients = pClients;
        }

        public void Run()
        {
            while (true)
            {
                string messageType = Protocol.ReceiveHeader(ClientSocket);
                int command = Protocol.ReceiveCommand(ClientSocket);
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
                case 2:
                    HandleInscription();
                    break;
                case 3:
                    HandleAvailableCourses();
                    break;
                case 5:
                    HandleCalifications();
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
                clients.Add(new Utils.StudentSocket(id, NotificationSocket));
            }
            catch (StudentException e)
            {
                Protocol.Send(ClientSocket, "RES", 99, e.Message);
            }
        }

        private void HandleInscription()
        {
            string data = Protocol.ReceiveData(ClientSocket);
            var arrayData = data.Split('#');
            int studentId = Int32.Parse(arrayData[0]);
            string courseName = arrayData[1];
            try
            {
                courseLogic.AddStudent(studentId, courseName);
                Protocol.Send(ClientSocket, "RES", 80);
            }
            catch (StudentException e)
            {
                Protocol.Send(ClientSocket, "RES", 99, e.Message);
            }
            catch (CourseException e)
            {
                Protocol.Send(ClientSocket, "RES", 99, e.Message);
            }
        }

        private void HandleAvailableCourses()
        {
            string data = Protocol.ReceiveData(ClientSocket);
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
            Protocol.Send(ClientSocket, "RES", 80, String.Join("#", strResponse));
        }

        private void HandleCalifications()
        {
            string data = Protocol.ReceiveData(ClientSocket);
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
            Protocol.Send(ClientSocket, "RES", 80, String.Join("#", strResponse));
        }

    }
}
