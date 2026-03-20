using OrderGenerator.Api.Fix.IFix;
using OrderGenerator.Api.Models.Dtos;
using OrderGenerator.Api.Services.IService;

namespace OrderGenerator.Api.Services
{
    public class OrderGeneratorService(IFixClient fixClient, IFixMessageBuilder fixBuilder) : IOrderGeneratorService
    {
        public readonly IFixClient _fixClient = fixClient;
        private readonly IFixMessageBuilder _fixBuilder = fixBuilder;

        public async Task<decimal> NewOrderSingle(OrderDto order)
        {
            string clOrdId = Guid.NewGuid().ToString();

            var message = _fixBuilder.BuildNewOrderSingle(
                clOrdId,
                order.Symbol.ToString(),
                (char)order.Side,
                order.Price,
                order.Quantity);

            var response = await _fixClient.SendAndAwait(message, clOrdId);

            // Extracts Exposure from tag 9001
            if (response.IsSetField(9001))
            {
                return response.GetDecimal(9001);
            }

            return 0;
        }
    }
}
