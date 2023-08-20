using RabbitMQ.Client;

namespace Consumer.Factories;

public interface IRmqConnectionFactory
{
    IConnection CreateConnection();
}