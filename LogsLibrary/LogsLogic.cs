using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace LogsLibrary
{
    public class LogsLogic
    {
        public string QueuePath { get; set; }
        public LogsLogic(string queuePath)
        {
            //QueuePath = @".\Private$\adminQueue"; // TODO - mover a archivo config
            QueuePath = queuePath;
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
                log.Label = label;
                messageQueue.Send(log);
            }

        }

    }
}
