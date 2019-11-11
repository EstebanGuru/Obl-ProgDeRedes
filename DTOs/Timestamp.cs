using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    [Serializable()]
    public class Timestamp
    {
        public string Time { get; set; }
        public string User { get; set; }
        public string Message { get; set; }

        public Timestamp() { }

        public Timestamp(string message, string time, string user)
        {
            Time = time;
            Message = message;
            User = user;
        }
    }
}
