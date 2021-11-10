using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserNotification.Shared.Entities
{
    public sealed class Email
    {
        public Email(string toEmail, string subject, string body)
        {
            ToEmail = toEmail;
            Subject = subject;
            Body = body;
        }
        public string ToEmail { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
    }
}
