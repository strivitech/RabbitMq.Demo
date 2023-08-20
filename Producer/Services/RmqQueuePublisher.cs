using System.Text;
using Producer.Factories;
using RabbitMQ.Client;

namespace Producer.Services;

public class RmqQueuePublisher : IQueuePublisher, IDisposable
{
    private readonly IConnection _connection;

    public RmqQueuePublisher(IRmqConnectionFactory rmqConnectionFactory)
    {
        _connection = rmqConnectionFactory.CreateConnection();
    }

    public async Task PublishAsync(string queueName, string message)
    {
        using var channel = _connection.CreateModel();
        channel.ConfirmSelect();
        channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
        var body = Encoding.UTF8.GetBytes(message);
        await Task.Run(() =>
        {
            channel.BasicPublish(exchange: string.Empty, routingKey: queueName, basicProperties: null, body: body);
            channel.WaitForConfirmsOrDie(TimeSpan.FromSeconds(5));
        });
    }

    public void Dispose() => _connection.Close();
}