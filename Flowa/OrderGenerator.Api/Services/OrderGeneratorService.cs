using OrderGenerator.Api.Fix.IFix;
using OrderGenerator.Api.Models.Dtos;
using OrderGenerator.Api.Services.IService;

namespace OrderGenerator.Api.Services
{
    public class OrderGeneratorService(IFixClient fixClient, IFixMessageBuilder fixBuilder) : IOrderGeneratorService
    {
        public readonly IFixClient _fixClient = fixClient;
        private readonly IFixMessageBuilder _fixBuilder = fixBuilder;

        public bool NewOrderSingle(OrderDto order)
        {
            var message = _fixBuilder.BuildNewOrderSingle(
            order.Symbol.ToString(),
            (char)order.Side,
            order.Price,
            order.Quantity);

            return _fixClient.Send(message);
        }
    }
}
