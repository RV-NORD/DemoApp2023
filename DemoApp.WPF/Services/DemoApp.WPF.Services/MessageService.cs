using DemoApp.WPF.Services.Interfaces;

namespace DemoApp.WPF.Services
{
    public class MessageService : IMessageService
    {
        public string GetMessage()
        {
            return "Hello from the Message Service";
        }
    }
}
