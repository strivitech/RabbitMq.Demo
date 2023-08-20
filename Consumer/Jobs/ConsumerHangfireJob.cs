using Consumer.Services;

namespace Consumer.Jobs;

public class ConsumerHangfireJob
{
    private readonly IQueueConsumer _queueConsumer;

    public ConsumerHangfireJob(IQueueConsumer queueConsumer)
    {
        _queueConsumer = queueConsumer;
    }

    public async Task DoAsync() => await _queueConsumer.ReceiveAsync();
}