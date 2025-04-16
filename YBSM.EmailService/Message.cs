using System.Collections.Generic;
using System.Linq;
using MimeKit;

namespace Infrastructure.EmailService
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public MimeEntity Content { get; set; }

        public Message(IEnumerable<string> to, string subject, MimeEntity content)
        {
            To = new List<MailboxAddress>();

            To.AddRange(to.Select(x => new MailboxAddress("any", x)));
            Subject = subject;
            Content = content;        
        }
    }
}