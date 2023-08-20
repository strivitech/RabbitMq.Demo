using System.Text;
using Consumer.Factories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer.Services;

public class CreateUsersConsumer : IQueueConsumer, IDisposable
{
    private readonly IModel _model;
    private readonly IConnection _connection;
    private const string QueueName = "CreateUsers";
    
    public CreateUsersConsumer(IRmqConnectionFactory rmqConnectionFactory)
    {
        _connection = rmqConnectionFactory.CreateConnection();
        _model = _connection.CreateModel();
        _model.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false);
    }
    
    public async Task ReceiveAsync()    
    {
        var consumer = new AsyncEventingBasicConsumer(_model);
        consumer.Received += (_, ea) =>
        {
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());
            Console.WriteLine(message);
            return Task.CompletedTask;
        };
        _model.BasicConsume(QueueName, true, consumer);
        await Task.CompletedTask;
    }

    public void Dispose()
    {
        if (_model.IsOpen)
        {
            _model.Close();
        }

        if (_connection.IsOpen)
        {
            _connection.Close();
        }
        
        _model.Dispose();
        _connection.Dispose();
    }
}