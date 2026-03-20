using OrderGenerator.Api.Models.Dtos;

namespace OrderGenerator.Api.Services.IService
{
    public interface IOrderGeneratorService
    {
        Task<decimal> NewOrderSingle(OrderDto order);
    }
}
