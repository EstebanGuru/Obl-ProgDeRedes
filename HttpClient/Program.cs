using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            _ = Run();
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

        private static void HandleViewLogs()
        {
            Console.WriteLine("**************  Logs Menu  *****************");
            Console.WriteLine("1: View all ");
            string strOption = Console.ReadLine();
            int option = Int32.Parse(strOption);
            switch (option)
            {
                case 1:
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
                    var logs = client.GetLogs("All");
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
