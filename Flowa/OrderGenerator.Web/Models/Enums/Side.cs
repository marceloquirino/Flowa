using System.ComponentModel.DataAnnotations;

namespace OrderGenerator.Web.Models.Enums
{
    public enum Side
    {
        [Display(Name = "Comprar")]
        Buy = '1',
        [Display(Name = "Vender")]
        Sell = '2'
    }
}
