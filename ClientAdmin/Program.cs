using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ClientAdmin
{

    public class Credentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class Session
    {
        public string TeacherId { get; set; }
        public string Id { get; set; }
    }

    class Program
    {
        public static HttpClient WebClient = new HttpClient();
        public static string token;
        static void Main(string[] args)
        {
            Program program = new Program();
            program.HandleLogin();
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
            string endpoint = "https://localhost:44318/api/login";
            HttpResponseMessage httpResponseMsg = WebClient.PostAsJsonAsync(endpoint, credentials).Result;
            if (httpResponseMsg.IsSuccessStatusCode)
            {
                token = httpResponseMsg.Content.ReadAsAsync<Session>().Result.Id;
            } else
            {
                Console.WriteLine(httpResponseMsg.Content.ReadAsStringAsync());
            }

            //HttpResponseMessage webResponse = WebClient.GetAsync(endpoint).Result;

            Console.ReadLine();
        }
    }
}
