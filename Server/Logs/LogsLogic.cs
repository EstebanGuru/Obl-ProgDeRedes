using System;
using System.Configuration;
using System.Messaging;
using DTOs;

namespace Server.Logs
{
    public class LogsLogic
    {
        public string QueuePath { get; set; }
        public LogsLogic()
        {
            //QueuePath = ConfigurationManager.AppSettings[ConfigConstants.QueuePath];
            QueuePath = ConfigConstants.QueuePath;
            if (!MessageQueue.Exists(QueuePath))
            {
                MessageQueue.Create(QueuePath);
            }

        }

        public void SendTimestamp(string label, string user, string message)
        {
            string time = DateTime.Now.ToString();
            Timestamp timestamp = new Timestamp(message, time, user);

            using (var messageQueue = new MessageQueue(QueuePath))
            {
                var log = new Message(timestamp);
                messageQueue.Send(log);
            }

        }

    }
}
