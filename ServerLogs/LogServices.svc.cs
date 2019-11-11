using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ServerLogs
{
    public class LogServices : ILogContract
    {
        public List<Timestamp> GetLogs(string filter)
        {
            Console.WriteLine("Pidiendo logs ocn filtro {0} ", filter);
            string queuePath = @".\Private$\adminQueue";

            List<Timestamp> result = new List<Timestamp>();
            var messageQueue = new MessageQueue(queuePath)
            {
                Formatter = new XmlMessageFormatter(new[] { typeof(Timestamp) })
            };
            switch (filter)
            {
                case "All":
                    var messages = messageQueue.GetAllMessages();
                    foreach (var msg in messages)
                    {
                        if (msg != null)
                        {
                            DTOs.Timestamp timestampMessage = (DTOs.Timestamp)msg.Body;
                            Timestamp responseTimestamp = new Timestamp(timestampMessage.Time, timestampMessage.User, timestampMessage.Message, msg.Label);
                            result.Add(responseTimestamp);
                        }
                    }

                    break;
            }
            return result;
        }
    }
}
