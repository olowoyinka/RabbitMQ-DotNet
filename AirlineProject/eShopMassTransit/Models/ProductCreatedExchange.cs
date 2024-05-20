using MassTransit;

namespace eShopMassTransit.Models
{
    [EntityName("ProductCreatedExchange")]
    public record ProductCreatedExchange
    {
        public int Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;

        public decimal Price { get; init; }
    }
}