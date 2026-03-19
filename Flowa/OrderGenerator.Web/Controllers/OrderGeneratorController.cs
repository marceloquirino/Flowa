using Microsoft.AspNetCore.Mvc;
using OrderGenerator.Web.Models;
using OrderGenerator.Web.Service.IService;

namespace OrderGenerator.Web.Controllers
{
    public class OrderGeneratorController(IOrderGeneratorService orderGeneratorService) : Controller
    {
        private readonly IOrderGeneratorService _orderGeneratorService = orderGeneratorService;
        public IActionResult OrderGeneratorIndex()
        {
            return View(new OrderDto());
        }

        [HttpPost]
        public async Task<IActionResult> GenerateOrder(OrderDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _orderGeneratorService.PostNewOrderSingle(model);

                if (response?.IsSuccess == true)
                {
                    TempData["success"] = "Ordem enviada com sucesso";
                    model = new OrderDto();
                    return RedirectToAction(nameof(OrderGeneratorIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            else
            {
                TempData["error"] = "Valores inválidos";
            }
            return View("OrderGeneratorIndex", model);
        }
    }
}
