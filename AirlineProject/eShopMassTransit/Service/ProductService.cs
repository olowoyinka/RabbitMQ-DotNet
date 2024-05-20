using eShopMassTransit.EventBus;
using eShopMassTransit.Models;
using MassTransit;
using MassTransit.Transports.Fabric;

namespace eShopMassTransit.Service;

public class ProductService : IProductService
{
    private readonly IEventBus _eventBus;
    private readonly IPublishEndpoint _publishEndpoint;

    public ProductService(IEventBus eventBus, IPublishEndpoint publishEndpoint)
    {
        _eventBus = eventBus;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<bool> AddProduct(Product product)
    {
        await _publishEndpoint.Publish<ProductCreatedExchange>(new ProductCreatedExchange
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price
        }, context => { context.SetRoutingKey("ProductCreatedRouteKey"); });

        return true;
    }
}