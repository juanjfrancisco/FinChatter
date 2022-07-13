﻿
namespace FinChatter.Application.Model
{
    public class ChatMessage
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
