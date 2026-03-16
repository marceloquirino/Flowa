namespace OrderGenerator.Api.Models.Dtos
{
    public class OrderDto
    {
        public string Symbol { get; set; } = string.Empty;
        public string Side { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
