using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinChatter.Application.Model
{
    public class ChatMessage
    {
        public ChatMessage()
        {
            GroupName = "Public";
        }
        public string UserName { get; set; }

        public DateTime SentDate { get; set; }

        public string Message { get; set; }

        public string GroupName { get; set; }
    }
}
