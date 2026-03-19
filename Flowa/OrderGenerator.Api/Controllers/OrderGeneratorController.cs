using Microsoft.AspNetCore.Mvc;
using OrderGenerator.Api.Models.Dtos;
using OrderGenerator.Api.Services.IService;

namespace OrderGenerator.Api.Controllers
{
    [ApiController]
    [Route("api/orderGenerator")]
    public class OrderGeneratorController(IOrderGeneratorService orderGeneratorService) : ControllerBase
    {
        private readonly IOrderGeneratorService _orderGeneratorService = orderGeneratorService;
        private readonly ResponseDto _response = new();

        [HttpPost("NewOrderSingle")]
        public async Task<ResponseDto> NewOrderSingle([FromBody] OrderDto order)
        {
            try
            {
                _response.IsSuccess = _orderGeneratorService.NewOrderSingle(order);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }
    }
}
