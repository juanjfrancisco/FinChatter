namespace FinChatter.Application.Model
{
    public class RabbitMqConfiguration
    {
        public string HostName { get; set; }

        public string QueueName { get; set; }

        public string QueueNameToListen { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
        public int AmqpPort { get; set; }

    }
}
