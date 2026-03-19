using OrderGenerator.Web.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace OrderGenerator.Web.Models
{
    public class OrderDto
    {
        [EnumDataType(typeof(Symbol))]
        public Symbol Symbol { get; set; }

        [EnumDataType(typeof(Side))]
        public Side Side { get; set; }

        [Range(1, 99999)]
        public int Quantity { get; set; }

        [Range(0.01, 999.99)]
        public decimal Price { get; set; }
    }
}
