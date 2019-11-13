using HttpClientApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientApp
{
    class Program
    {
        public static HttpClient WebClient = new HttpClient();

        static void Main(string[] args)
        {
            Program program = new Program();
            program.HandleLogin();
            //_ = Run();
        }

        private static async Task Run()
        {
            while (true)
            {
                Console.WriteLine("**************  Menu ***************");
                Console.WriteLine("1: View Logs ");
                string strOption = Console.ReadLine();
                int option = Int32.Parse(strOption);
                switch (option)
                {
                    case 1:
                        HandleViewLogs();
                        break;
                    default:
                        break;
                }
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
            }
        }

        private void HandleLogin()
        {
            Console.WriteLine("Email: ");
            string email = Console.ReadLine();
            //Console.WriteLine("Password: ");
            //string password = Console.ReadLine();
            //Credentials credentials = new Credentials()
            //{
            //    Email = email,
            //    Password = password,
            //};
            string endpoint = "http://localhost:44344/api/Teachers";
            // HttpResponseMessage httpResponseMsg = WebClient.PostAsJsonAsync(endpoint, credentials).Result;
            HttpResponseMessage webResponse = WebClient.GetAsync(endpoint).Result;

            Console.ReadLine();
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
            using(LogServices.LogServiceClient client = new LogServices.LogServiceClient())
            {
                Console.WriteLine("ask logs");
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
