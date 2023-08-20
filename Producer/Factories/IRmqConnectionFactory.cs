using RabbitMQ.Client;

namespace Producer.Factories;

public interface IRmqConnectionFactory
{
    IConnection CreateConnection();
}