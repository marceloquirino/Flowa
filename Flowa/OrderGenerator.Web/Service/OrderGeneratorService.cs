using OrderGenerator.Web.Models;
using OrderGenerator.Web.Service.IService;
using OrderGenerator.Web.Utilities;

namespace OrderGenerator.Web.Service
{
    public class OrderGeneratorService : IOrderGeneratorService
    {
        private readonly IBaseService _baseService;

        public OrderGeneratorService(IBaseService baseService) => _baseService = baseService;
        public async Task<ResponseDto?> PostNewOrderSingle(OrderDto orderDto) => await _baseService.SendAssync(new RequestDto
        {
            ApiType = SD.ApiType.POST,
            Url = SD.OrderGeneratorAPIBase + "/api/orderGenerator/NewOrderSingle",
            Data = orderDto
        });
    }
}
