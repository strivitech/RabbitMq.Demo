using Consumer.Settings;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Consumer.Factories;

public class RmqConnectionFactory : IRmqConnectionFactory
{
    private readonly RabbitMqConfiguration _rabbitMqConfiguration;
    
    public RmqConnectionFactory(IOptions<RabbitMqConfiguration> options)
    {
        _rabbitMqConfiguration = options.Value;
    }
    
    public IConnection CreateConnection()
    {
        var connection = new ConnectionFactory
        {
            UserName = _rabbitMqConfiguration.Username,
            Password = _rabbitMqConfiguration.Password,
            HostName = _rabbitMqConfiguration.HostName,
            Port = _rabbitMqConfiguration.Port,
            DispatchConsumersAsync = true
        };
        var channel = connection.CreateConnection();
        return channel;
    }
}