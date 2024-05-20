using MassTransit;

namespace eShopMassTransit.EventBus;

public class EventBus : IEventBus
{
    private readonly IPublishEndpoint _publishEndpoint;

    public EventBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        throw new NotImplementedException();
        //return _publishEndpoint.Publish<T>(message, cancellationToken, callback: e => { e.});
    }
}