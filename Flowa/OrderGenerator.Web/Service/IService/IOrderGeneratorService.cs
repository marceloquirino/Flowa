using OrderGenerator.Web.Models;

namespace OrderGenerator.Web.Service.IService
{
    public interface IOrderGeneratorService
    {
        Task<ResponseDto?> PostNewOrderSingle(OrderDto orderDto);
    }
}
