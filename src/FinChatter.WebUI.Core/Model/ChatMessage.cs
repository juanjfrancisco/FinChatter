using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinChatter.WebUI.Core.Model
{
    internal class ChatMessage
    {
        public ChatMessage()
        {
            GroupName = "Public";
            SentDate = DateTime.Now;
        }
        public string UserName { get; set; }

        public DateTime SentDate { get; set; }

        public string Message { get; set; }

        public string GroupName { get; set; }
    }
}
