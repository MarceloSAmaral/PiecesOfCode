using System;

namespace msantana.amaral.LoadGenerator
{
    public class Payload
    {
        public Payload(String userName, int requestsSent)
        {
            name = userName;
            requests_sent = requestsSent;
        }
        public string name { get; }
        public DateTime date { get { return System.DateTime.UtcNow; } }
        public int requests_sent { get; set; }
    }
}
