public interface IEventBus
{
    Task PublishAsync<T>(T message);
}
