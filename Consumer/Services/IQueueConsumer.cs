namespace Consumer.Services;

public interface IQueueConsumer
{   
    Task ReceiveAsync();  
}