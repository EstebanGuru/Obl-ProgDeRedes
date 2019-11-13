using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace ServerLogHost
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost studentServiceHost = null;
            try
            {
                Uri httpBaseAddress = new Uri("http://localhost:4321/LogService");

                //Instantiate ServiceHost
                studentServiceHost = new ServiceHost(typeof(WcfLogServices.LogService),
                    httpBaseAddress);

                //Add Endpoint to Host
                studentServiceHost.AddServiceEndpoint(typeof(WcfLogServices.ILogService),
                                                        new WSHttpBinding(), "");

                //Metadata Exchange
                ServiceMetadataBehavior serviceBehavior = new ServiceMetadataBehavior();
                serviceBehavior.HttpGetEnabled = true;
                studentServiceHost.Description.Behaviors.Add(serviceBehavior);

                //Open
                studentServiceHost.Open();
                Console.WriteLine("Service is live now at: {0}", httpBaseAddress);
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                studentServiceHost = null;
                Console.WriteLine("There is an issue with StudentService" + ex.Message);
                Console.ReadKey();
            }
        }
    }
}
