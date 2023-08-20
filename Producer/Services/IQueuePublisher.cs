namespace Producer.Services;

public interface IQueuePublisher
{
    Task PublishAsync(string queueName, string message);
}