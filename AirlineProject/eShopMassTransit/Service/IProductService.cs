using eShopMassTransit.Models;

namespace eShopMassTransit.Service
{
    public interface IProductService
    {
        Task<bool> AddProduct(Product product);
    }
}
