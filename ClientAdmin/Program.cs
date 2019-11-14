﻿using ClientAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClientAdmin
{
    class Program
    {
        public static HttpClient WebClient = new HttpClient();
        private string Endpoint = "https://localhost:44318/api/"; //TODO - mover a constante
        public static string token;
        static void Main(string[] args)
        {
            _ = Run();
        }

        private static async Task Run()
        {
            Program program = new Program();
            while (true)
            {
                if (token == null)
                {
                    program.HandleLogin();
                } else
                {
                    Console.WriteLine("**************  Menu ***************");
                    Console.WriteLine("1: View Logs ");
                    Console.WriteLine("2: Create Teacher ");
                    Console.WriteLine("3: Qualify material ");
                    string strOption = Console.ReadLine();
                    int option = Int32.Parse(strOption);
                    switch (option)
                    {
                        case 1:
                            HandleViewLogs();
                            break;
                        case 2:
                            await program.HandleAddTeacher();
                            break;
                        case 3:
                            await program.HandleAddCalification();
                            break;
                        default:
                            break;
                    }
                    Console.WriteLine("");
                }
                
            }
        }

        private async Task HandleAddCalification()
        {
            Console.WriteLine("StudentId: ");
            int studentId = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Course name: ");
            string courseName = Console.ReadLine();
            Console.WriteLine("Calification: ");
            int calification = Int32.Parse(Console.ReadLine());
            StudentCourse calificationDTO = new StudentCourse()
            {
                StudentId = studentId,
                CourseName = courseName,
                Calification = calification,
            };
            string postCalification = Endpoint + "Calification";
            HttpResponseMessage httpResponseMsg = WebClient.PostAsJsonAsync(postCalification, calificationDTO).Result;
            if (httpResponseMsg.IsSuccessStatusCode)
            {
                Console.WriteLine("Teacher added succesfully");
            }
            else
            {
                string msg = await httpResponseMsg.Content.ReadAsStringAsync();
                Console.WriteLine("The following error ocurred {0}", msg);
            }
        }

        private async Task HandleAddTeacher()
        {
            Console.WriteLine("Name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Lastname: ");
            string lastname = Console.ReadLine();
            Console.WriteLine("Email: ");
            string email = Console.ReadLine();
            Console.WriteLine("Password: ");
            string password = Console.ReadLine();
            Teacher teacher = new Teacher()
            {
                Name = name,
                LastName = lastname,
                Email = email,
                Password = password,
            };
            string postTeacher = Endpoint + "Teachers";
            HttpResponseMessage httpResponseMsg = WebClient.PostAsJsonAsync(postTeacher, teacher).Result;
            if (httpResponseMsg.IsSuccessStatusCode)
            {
                Console.WriteLine("Teacher added succesfully");
            }
            else
            {
                string msg = await httpResponseMsg.Content.ReadAsStringAsync();
                Console.WriteLine("The following error ocurred {0}", msg);
            }

        }

        private void HandleLogin()
        {
            Console.WriteLine("Email: ");
            string email = Console.ReadLine();
            Console.WriteLine("Password: ");
            string password = Console.ReadLine();
            Credentials credentials = new Credentials()
            {
                Email = email,
                Password = password,
            };
            string loginEndpoint = Endpoint + "login";
            HttpResponseMessage httpResponseMsg = WebClient.PostAsJsonAsync(loginEndpoint, credentials).Result;
            if (httpResponseMsg.IsSuccessStatusCode)
            {
                token = httpResponseMsg.Content.ReadAsAsync<Session>().Result.Id;
            } else
            {
                Console.WriteLine(httpResponseMsg.Content.ReadAsStringAsync());
            }
        }

        private static void HandleViewLogs()
        {
            Console.WriteLine("**************  Logs Menu  *****************");
            Console.WriteLine("1: View all ");
            Console.WriteLine("2: Alta alumnos");
            Console.WriteLine("3: Alta docentes");
            Console.WriteLine("4: Alta curso");
            Console.WriteLine("5: Baja curso");
            Console.WriteLine("6: Inscripciones a cursos");
            Console.WriteLine("7: Coreccion de materiales");
            string strOption = Console.ReadLine();
            int option = Int32.Parse(strOption);
            switch (option)
            {
                case 1:
                    HandleRequestLogs("All");
                    break;
                case 2:
                    HandleRequestLogs("CreateStudent");
                    break;
                case 3:
                    HandleRequestLogs("CreateCourse"); // Cambiar por docentes.
                    break;
                case 4:
                    HandleRequestLogs("CreateCourse");
                    break;
                case 5:
                    HandleRequestLogs("DeleteCourse");
                    break;
                case 6:
                    HandleRequestLogs("Inscription");
                    break;
                case 7:
                    HandleRequestLogs("All");
                    break;
                default:
                    break;
            }

        }
        private static void HandleRequestLogs(string filter)
        {
            using (LogServices.LogServiceClient client = new LogServices.LogServiceClient())
            {
                try
                {
                    var logs = client.GetLogs(filter);
                    foreach (var item in logs)
                    {
                        Console.WriteLine(item.Message);

                    }
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }
    }
}