using OrderGenerator.Api.Models.Dtos;

namespace OrderGenerator.Api.Services.IService
{
    public interface IOrderGeneratorService
    {
        bool NewOrderSingle(OrderDto order);
    }
}
