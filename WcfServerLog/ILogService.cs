using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfLogServices
{
    [ServiceContract]
    public interface ILogService
    {

        [OperationContract]
        List<TimestampLog> GetLogs(string filter);
    }


    [DataContract]
    public class TimestampLog
    {
        [DataMember]
        public string Time { get; set; }

        [DataMember]
        public string User { get; set; }

        [DataMember]

        public string Message { get; set; }

        [DataMember]
        public string Label { get; set; }

        public TimestampLog() { }

        public TimestampLog(string time, string user, string message, string label)
        {
            Time = time;
            User = user;
            Message = message;
            Label = label;
        }
    }
}
