using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ServerLogs
{
    [ServiceContract]
    public interface ILogContract
    {

        [OperationContract]
        List<Timestamp> GetLogs(string filter);
    }


    [DataContract]
    public class Timestamp
    {
        [DataMember]
        public string Time { get; set; }

        [DataMember]
        public string User { get; set; }

        [DataMember]

        public string Message { get; set; }

        [DataMember]
        public string Label { get; set; }

        public Timestamp(string time, string user, string message, string label)
        {
            Time = time;
            User = user;
            Message = message;
            Label = label;
        }
    }
}
