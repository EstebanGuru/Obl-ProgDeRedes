using DTOs;
using System;
using System.Collections.Generic;
using System.Messaging;

namespace WcfLogServices
{
    public class LogService : ILogService
    {
        public List<TimestampLog> GetLogs(string filter)
        {
            Console.WriteLine("Pidiendo logs ocn filtro {0} ", filter);
            string queuePath = @".\Private$\adminQueue";

            List<TimestampLog> result = new List<TimestampLog>();
            var messageQueue = new MessageQueue(queuePath)
            {
                Formatter = new XmlMessageFormatter(new[] { typeof(DTOs.Timestamp) })
            };
            switch (filter)
            {
                case "All":
                    var messages = messageQueue.GetAllMessages();
                    foreach (var msg in messages)
                    {
                        if (msg != null)
                        {
                            var timestampMessage = (Timestamp)msg.Body;
                            TimestampLog responseTimestamp = new TimestampLog(timestampMessage.Time, timestampMessage.User, timestampMessage.Message, msg.Label);
                            result.Add(responseTimestamp);
                        }
                    }

                    break;
            }
            return result;
        }
    }
}
