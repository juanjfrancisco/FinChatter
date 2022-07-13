using FinChatter.Application.Model;

namespace FinChatter.Application.Interfaces
{
    public interface IMqSender
    {
        void SendMessage(ChatMessage message);
    }
}
