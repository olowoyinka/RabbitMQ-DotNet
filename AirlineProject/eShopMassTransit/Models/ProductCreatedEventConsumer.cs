using MassTransit;

namespace eShopMassTransit.Models;

[EntityName("ProductCreatedEventConsumer")]
public class ProductCreatedEventConsumer : IConsumer<ProductCreatedExchange>
{
    private readonly ILogger<ProductCreatedEventConsumer> _logger;

    public ProductCreatedEventConsumer(ILogger<ProductCreatedEventConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<ProductCreatedExchange> context)
    {
        _logger.LogInformation("Product created: {@Product}", context.Message);

        //return context.Message;

        return Task.CompletedTask;
    }
}