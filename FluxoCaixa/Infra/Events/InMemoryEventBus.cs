public class InMemoryEventBus : IEventBus
{
    public List<object> EventosPublicados = new();

    public Task PublishAsync<T>(T message)
    {
        EventosPublicados.Add(message!);
        return Task.CompletedTask;
    }
}
